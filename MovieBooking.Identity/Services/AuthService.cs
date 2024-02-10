using AutoMapper.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieBooking.Application.Contracts.Application;
using MovieBooking.Application.Contracts.Identity;
using MovieBooking.Application.Contracts.Mailing;
using MovieBooking.Application.Contracts.Mailing.Models;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Interfaces;
using MovieBooking.Application.Models.Authentication;
using MovieBooking.Application.Models.Authentication.ChangePassword;
using MovieBooking.Application.Models.Users;
using MovieBooking.Domain.Events;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly IMailService _mailService;
        private readonly AppIdentityDbContext _db ;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;
        private readonly IEmailTemplateService _templateService;
        private readonly IJobService _jobService;
    private readonly IConfiguration _configuration ;

    public AuthService(
        IAuthorizationService authorizationService,
        UserManager<User> userManager,
        IOptions<JwtSettings> jwtSettings,
        SignInManager<User> signInManager,
        AppIdentityDbContext db,
        ICurrentUserService currentUserService,
        IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
        IConfiguration configuration)
    {
        _authorizationService = authorizationService;
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _signInManager = signInManager;
        _db = db;
        _currentUserService = currentUserService;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _configuration = configuration;
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
                var user = await _userManager.FindByEmailAsync(request.Email);


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
    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }
    public async Task<ChangePasswordResponse> ChangePasswordAsync(string userId, string currentPassword, string newPassword, string confirmPassword)
    {
        //Nisha13! current pass
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

    public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            throw new NotFoundException("User", request.Email);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        const string route = "reset-password";
        string passwordResetUrl = QueryHelpers.AddQueryString(route, QueryStringKeys.Token, token);
        passwordResetUrl = QueryHelpers.AddQueryString(passwordResetUrl, "email", request.Email);

        EmailContent eMailModel = new EmailContent(passwordResetUrl)
        {
            Subject = "Reset Your Password!",
            HeyUserName = "User",
            ButtonText = "Reset my password",
        };
        eMailModel.RowData.Add("Click the link below for change and password reset.");
        eMailModel.RowData.Add("If you didn't request this, just ignore this message.");

        var mailRequest = new MailRequest(
            new List<string> { request.Email },
            eMailModel.Subject,
            _templateService.GenerateDefaultEmailTemplate(eMailModel));

        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
    }



    public async Task ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            throw new InvalidOperationException($"User with email '{email}' not found.");
        }

        try
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                return;
            }
            else
            {
                throw new InvalidOperationException("Invalid token for password reset.");
            }
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException("Invalid token for password reset.");
        }
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        string? userEmail = userPrincipal?.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(userEmail))
        {
            throw new AuthenticationException("Authentication Failed.");
        }

        var user = await _userManager.FindByEmailAsync(userEmail);

        _ = user ?? throw new NotFoundException("User ", userEmail);

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new AuthenticationException("Invalid Refresh Token.");
        }

        JwtSecurityToken jwtSecurityToken = await GenerateToken(user!);

        if (string.IsNullOrEmpty(user.RefreshToken) || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
            await _userManager.UpdateAsync(user);
        }

        return new RefreshTokenResponse(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), user.RefreshToken, user.RefreshTokenExpiryTime);
    }
    //public async Task<ApiResponse<UserDetailsDto>> CreateUserAsync(CreateUserCommandRequest request, CancellationToken cancellationToken)
    //    {
    //        var existingUser = await _userManager.FindByEmailAsync(request.Email);

    //        if (existingUser != null)
    //        {
    //            throw new Exception("User with this email already exists.");
    //        }

    //        var newUser = new User
    //        {
    //            FirstName = request.FirstName,
    //            LastName = request.LastName,
    //            Email = request.Email,
    //            UserName = request.UserName,
    //            IsActive = request.IsActive, // Assuming 1 represents active status
    //            IsDeleted = request.IsDeleted, // Assuming 0 represents not deleted
    //            EmailConfirmed = request.EmailConformed, // Assuming 1 represents email confirmed
    //        };

    //        var result = await _userManager.CreateAsync(newUser, request.Password);

    //        if (!result.Succeeded)
    //        {
    //            // Handle the case where user creation fails
    //            // You may throw an exception or return an appropriate response
    //            throw new Exception($"Failed to create user. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    //        }

    //        // Optionally, assign roles to the new user if needed
    //        // Example: await _userManager.AddToRoleAsync(newUser, "UserRole");

    //        var userDetails = new UserDetailsDto
    //        {
    //            FirstName = newUser.FirstName,
    //            LastName = newUser.LastName,
    //            Email = newUser.Email,
    //            RoleId = null, // You may assign role information here if applicable
    //            RoleName = null // You may assign role information here if applicable
    //        };

    //        var response = new ApiResponse<UserDetailsDto>
    //        {
    //            Success = true,
    //            StatusCode = HttpStatusCodes.Created, // Assuming user creation results in a 201 Created status
    //            Data = userDetails,
    //            Message = "User created successfully."
    //        };

    //        return response;
    //    }


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

    public async Task<ApiResponse<string>> UpdateAsync(UpdateUserRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.user.Id);

        _ = user ?? throw new NotFoundException("UserId ", request.user.Id);

        var existingEmail = await _userManager.FindByEmailAsync(request.user.Email!);

        if (existingEmail != null && existingEmail.Id != request.user.Id)
        {
            throw new Exception($"Email {request.user.Email} already exists.");
        }

        user.FirstName = request.user.FirstName;
        user.LastName = request.user.LastName;

        if (request.user.Email != user.Email)
        {
            user.UserName = user.Email = request.user.Email;
            user.NormalizedUserName = user.NormalizedEmail = request.user.Email!.ToUpper();

            //if (user.IsInvitationAccepted == false)
            //{
            //    await UserInvitationEmailSend(request.Origin, user);
            //}
        }

        //var newRole = await _roleManager.FindByIdAsync(request.RoleId) ?? throw new NotFoundException("RoleId ", request.RoleId);

        //var currentRoles = await _userManager.GetRolesAsync(user);
        //await _userManager.RemoveFromRolesAsync(user, currentRoles);
        //await _userManager.AddToRoleAsync(user, newRole.Name);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return new ApiResponse<string>
        {
            Success = result.Succeeded,
            Data = "User updated successfully.",
            StatusCode = result.Succeeded ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Message = result.Succeeded ? $"User {ConstantMessages.UpdatedSuccessfully}" : $"{ConstantMessages.FailedToCreate} user."
        };
    }
    public async Task<ApiResponse<string>> DeleteAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException("UserId ", userId);

        if (user.IsSuperAdmin == true && user.Email == _configuration["AppSettings:UserEmail"])
        {
            throw new Exception($"Not allowed to deleted member.");
        }

        //Check for any task assigned to user
      // var userTask = await _taskService.IsTaskAssignedToUser(userId);

      //  if (userTask)
      //  {
      //      throw new Exception($"Cannot delete as Task is assigned to user.");
      //  }

        user.NormalizedUserName = user.UserName = user.UserName + "_" + Guid.NewGuid().ToString();
        var result = await _userManager.DeleteAsync(user);

        return new ApiResponse<string>
        {
            Success = result.Succeeded,
            Data = "User deleted successfully.",
            StatusCode = result.Succeeded ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
            Message = result.Succeeded ? $"User {ConstantMessages.DeletedSuccessfully}" : $"{ConstantMessages.FailedToCreate} user."
        };
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
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new UnauthorizedAccessException("Invalid Token.");
        }

        return principal;
    }
    #endregion



}
