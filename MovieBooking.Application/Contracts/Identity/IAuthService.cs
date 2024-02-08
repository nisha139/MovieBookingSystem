using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Interfaces;
using MovieBooking.Application.Models.Authentication;
using MovieBooking.Application.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Contracts.Identity
{
    public interface IAuthService : ITransientService
    {
        Task<IResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<ApiResponse<UserDetailsDto>> GetUserDetailsAsync(string userId, CancellationToken cancellationToken);
        Task<ApiResponse<UserDetailsDto>> CreateUserAsync(CreateUserCommandRequest request, CancellationToken cancellationToken);
        Task<ChangePasswordResponse> ChangePasswordAsync(string userId, string currentPassword, string newPassword, string confirmPassword);
    }
}
