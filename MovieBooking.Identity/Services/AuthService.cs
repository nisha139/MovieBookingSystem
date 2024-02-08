using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieBooking.Application.Contracts.Application;
using MovieBooking.Application.Contracts.Identity;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Models.Authentication;
using MovieBooking.Application.Models.Users;
using MovieBooking.Identity.Database;
using MovieBooking.Identity.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Identity.Services;
    public class AuthService : IAuthService
    {

        private readonly AppIdentityDbContext _db ;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtSettings _jwtSettings;
    private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;

    public AuthService(
        UserManager<User> userManager,
        IOptions<JwtSettings> jwtSettings,
        SignInManager<User> signInManager,
        AppIdentityDbContext db,
        ICurrentUserService currentUserService,
        IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _signInManager = signInManager;
        _db = db;
        _currentUserService = currentUserService;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
    }
    #region Public Methods
    public async Task<IResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            try
            {
                // Log the email being searched for
                Console.WriteLine($"Authenticating user with email: {request.Email}");

                // Check if email is provided
                if (string.IsNullOrEmpty(request.Email))
                {
                    throw new ArgumentException("Email is required for authentication.");
                }

                // Find user by email
                var user = await _userManager.FindByEmailAsync(request.Email.ToLower());


                // Handle case where user is not found
                if (user == null)
                {
                    Console.WriteLine($"User not found with email: {request.Email}");
                    throw new NotFoundException("User", request.Email);
                }

                // Authenticate user
                var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

                // Handle authentication failure
                if (!result.Succeeded)
                {
                    throw new AuthenticationException("Invalid credentials. Please provide valid username and password.");
                }

                // Rest of the authentication logic...
                JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

                // Generate and store refresh token
                user.RefreshToken = GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
                await _userManager.UpdateAsync(user);

                // Prepare authentication response
                AuthenticationResponse response = new AuthenticationResponse
                {
                    Id = user.Id,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    RefreshToken = user.RefreshToken,
                    Email = user.Email,
                    UserName = user.UserName
                };

                return new ApiResponse<AuthenticationResponse>
                {
                    Data = response,
                    StatusCode = HttpStatusCodes.OK,
                    Success = true,
                    Message = "Logged in successfully."
                };
            }
            catch (Exception ex)
            {
                // Log any errors encountered during authentication
                Console.WriteLine($"Authentication failed: {ex.Message}");

                // Handle and return appropriate response for the error
                return new ApiResponse<AuthenticationResponse>
                {
                    StatusCode = HttpStatusCodes.BadRequest,
                    Success = false,
                    Message = "Authentication failed. Please try again later."
                };
            }
        }

    public async Task<ChangePasswordResponse> ChangePasswordAsync(string userId, string currentPassword, string newPassword, string confirmPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new NotFoundException("User", userId);
        }

        if (newPassword != confirmPassword)
        {
            throw new CustomException("New password does not match the confirmed password.");
        }

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (!result.Succeeded)
        {
            throw new CustomException($"Failed to change password: {string.Join(",", result.Errors.Select(p => p.Description))}");
        }

        return new ChangePasswordResponse
        {
            Success = true,
            Message = "Password changed successfully"
        };
    }
    public async Task<ApiResponse<UserDetailsDto>> CreateUserAsync(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }

            var newUser = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                IsActive = request.IsActive, // Assuming 1 represents active status
                IsDeleted = request.IsDeleted, // Assuming 0 represents not deleted
                EmailConfirmed = request.EmailConformed, // Assuming 1 represents email confirmed
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                // Handle the case where user creation fails
                // You may throw an exception or return an appropriate response
                throw new Exception($"Failed to create user. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // Optionally, assign roles to the new user if needed
            // Example: await _userManager.AddToRoleAsync(newUser, "UserRole");

            var userDetails = new UserDetailsDto
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                RoleId = null, // You may assign role information here if applicable
                RoleName = null // You may assign role information here if applicable
            };

            var response = new ApiResponse<UserDetailsDto>
            {
                Success = true,
                StatusCode = HttpStatusCodes.Created, // Assuming user creation results in a 201 Created status
                Data = userDetails,
                Message = "User created successfully."
            };

            return response;
        }


        public async Task<ApiResponse<UserDetailsDto>> GetUserDetailsAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await (from u in _db.Users.AsNoTracking()
                              join ur in _db.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                              join r in _db.Roles.AsNoTracking() on ur.RoleId equals r.Id
                              where u.Id == userId
                              select new UserDetailsDto()
                              {
                                  //Id = u.Id,
                                  FirstName = u.FirstName ?? string.Empty,
                                  LastName = u.LastName ?? string.Empty,
                                  Email = u.Email ?? string.Empty,
                                  RoleId = r.Id,
                                  RoleName = r.Name ?? string.Empty,
                              }).FirstOrDefaultAsync();

            _ = user ?? throw new NotFoundException("User ", userId);

            var response = new ApiResponse<UserDetailsDto>
            {
                Success = user != null,
                StatusCode = user != null ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = user,
                Message = user != null ? $"User {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} user."
            };
            return response;
        }
        #endregion

        #region private methods
        private string GenerateRefreshToken()
        {
            var numbers = new byte[32];
            using RandomNumberGenerator randomNumber = RandomNumberGenerator.Create();
            randomNumber.GetBytes(numbers);
            return Convert.ToBase64String(numbers);
        }
        private async Task<JwtSecurityToken> GenerateToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name,user.FirstName+" "+ user.LastName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
        #endregion



    }
