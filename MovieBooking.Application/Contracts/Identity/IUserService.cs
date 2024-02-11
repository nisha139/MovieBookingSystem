using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Interfaces;
using MovieBooking.Application.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Contracts.Identity
{
    public interface IUserService : ITransientService
    {
        Task<ApiResponse<UserDetailsDto>> GetUserDetailsAsync(string userId, CancellationToken cancellationToken);
        //Task<List<string>> GetPermissionAsync(string userId, CancellationToken cancellationToken);
        //Task<bool> HasPermissionAsync(string? userId, string permission, CancellationToken cancellationToken = default);
        Task<ApiResponse<string>> UpdateAsync(UpdateUserRequest request);
        Task<ApiResponse<string>> DeleteAsync(string userId);
        Task<IPagedDataResponse<UserListDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken);
    }
}
