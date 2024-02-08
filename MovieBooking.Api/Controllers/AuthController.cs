using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Controllers.Base;
using MovieBooking.Application.Contracts.Identity;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Models.Authentication;
using MovieBooking.Application.Models.Users;

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
        [HttpPost("signup")]
        public async Task<ActionResult<ApiResponse<UserDetailsDto>>> SignUpAsync(CreateUserCommandRequest request)
        {
            // Perform validation if needed
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call the service method for user creation
            try
            {
                var response = await _authService.CreateUserAsync(request, CancellationToken.None);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during user creation
                _logger.LogError(ex, "Failed to create user");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create user");
            }
        }
    }
}
