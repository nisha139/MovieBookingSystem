using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MovieBooking.Application.Contracts.Identity;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Models.Users;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ILogger<UserController> logger, IUserService userService, IConfiguration configuration) : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

       

        [HttpGet("{id}")]
        public async Task<ApiResponse<UserDetailsDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _userService.GetUserDetailsAsync(id, cancellationToken);
        }
    }
}
