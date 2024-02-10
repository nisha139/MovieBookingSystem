using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Controllers.Base;
using MovieBooking.Application.Contracts.Identity;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Models.Authentication;
using MovieBooking.Application.Models.Authentication.ChangePassword;
using MovieBooking.Application.Models.Users;
using MovieBooking.Identity.Authorizations.Permissions;
using MovieBooking.Identity.Authorizations;
using MovieBooking.Identity.Services;
using System.Security.Claims;
using Action = MovieBooking.Identity.Authorizations.Action;

namespace MovieBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ILogger<AuthController> logger, IConfiguration configuration,IAuthService authService) : BaseApiController
    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly IAuthService _authService = authService;
        private readonly IConfiguration _configuration = configuration;

        [HttpGet("{id}")]
       
        public async Task<ApiResponse<UserDetailsDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _authService.GetUserDetailsAsync(id, cancellationToken);
        }


        [HttpPost("signin")]
        public async Task<IResponse> SignInAsync(AuthenticationRequest request)
        {
            return await _authService.AuthenticateAsync(request);
        }
        [HttpPut("{id}")]
        //[MustHavePermission(Action.Update, Resource.Users)]
        public async Task<ApiResponse<string>> UpdateAsync(string id, UpdateUserDto request)
        {
            if (id != request.Id)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Data = "The provided ID in the route does not match the ID in the request body.",
                    StatusCode = HttpStatusCodes.BadRequest
                };
            }
            return await _authService.UpdateAsync(new UpdateUserRequest() { user = request, Origin = GetOriginFromRequest(_configuration) });
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            return Ok(await _authService.RefreshTokenAsync(request));
        }

        [Authorize(Roles ="Administrator")]
        [HttpPost("changePassword")]
        public async Task<ActionResult<ChangePasswordResponse>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var userId = User.FindFirstValue("uid");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            return Ok(await _authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword, request.ConfirmPassword));
        }

        [HttpPost("forgotPassword")]
        public async Task<ApiResponse<string>> ForgotPassword(ForgotPasswordRequest request)
        {
            // Assuming _configuration is an instance of IConfiguration
            //var origin = _configuration["CorsSettings:CorsURLs"]; // Retrieve origin from configuration

            await _authService.ForgotPasswordAsync(request);

            return new ApiResponse<string>
            {
                Success = true,
                Data = "Password reset link sent successfully.",
                StatusCode = HttpStatusCodes.OK
            };
        }


        [HttpPost("resetPassword")]
        public async Task<ApiResponse<string>> ResetPassword(ResetPasswordRequest request)
        {
            await _authService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword);
            return new ApiResponse<string>
            {
                Success = true,
                Data = "Password reset successful.",
                StatusCode = HttpStatusCodes.OK
            };
        }

        [Authorize(Roles ="AdminiStrator")]
        [HttpDelete("{id}")]
        //[MustHavePermission(Action.Delete, Resource.Users)]
        public async Task<ApiResponse<string>> DeleteAsync(string id)
        {
            return await _authService.DeleteAsync(id);
        }

        //[HttpPost("signup")]
        //public async Task<ActionResult<ApiResponse<UserDetailsDto>>> SignUpAsync(CreateUserCommandRequest request)
        //{
        //    // Perform validation if needed
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Call the service method for user creation
        //    try
        //    {
        //        var response = await _authService.CreateUserAsync(request, CancellationToken.None);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions that occur during user creation
        //        _logger.LogError(ex, "Failed to create user");
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create user");
        //    }

        //}
    }
}
