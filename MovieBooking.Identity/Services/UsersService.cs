using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieBooking.Application.Contracts.Application;
using MovieBooking.Application.Contracts.Caching;
using MovieBooking.Application.Contracts.Identity;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Interfaces;
using MovieBooking.Application.Models.Users;
using MovieBooking.Identity.Database;
using MovieBooking.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Identity.Services
{
    public partial class UsersService(UserManager<User> userManager,
                                  RoleManager<ApplicationRole> roleManager,
                                  AppIdentityDbContext db,
                                  ICurrentUserService currentUserService,
                                  IConfiguration configuration
                                  // ICacheService cache
                                  //ICacheKeyService cacheKey
                                  ) : IUserService
    {
        private readonly AppIdentityDbContext _db = db;
        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IConfiguration _configuration = configuration;


        //private readonly ICacheService _cache = cache;
        //private readonly ICacheKeyService _cacheKey = cacheKey;

        #region public methods
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

        public async Task<IPagedDataResponse<UserListDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken)
        {
            var usersList = (from u in _db.Users.AsNoTracking()
                             join ur in _db.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                             join r in _db.Roles.AsNoTracking() on ur.RoleId equals r.Id
                             where u.Id != _currentUserService.UserId
                             select new UserListDto()
                             {
                                 Id = u.Id,
                                 FirstName = u.FirstName ?? string.Empty,
                                 LastName = u.LastName ?? string.Empty,
                                 Email = u.Email ?? string.Empty,
                                 FullName = u.FirstName + " " + u.LastName,
                                 // Status = u.IsInvitationAccepted == false ? UserStatus.Invited.ToString() : (u.IsActive ? UserStatus.Active.ToString() : UserStatus.Inactive.ToString()),
                                 RoleId = r.Id,
                                 Role = r.Name ?? string.Empty,
                                 CreatedOn = u.CreatedOn
                             }
                        );

            var spec = new GetSearchUserRequestSpec(filter);

            var users = await usersList.ApplySpecification(spec);

            int count = await usersList.ApplySpecificationCount(spec);

            return new PagedApiResponse<UserListDto>(count, filter.PageNumber, filter.PageSize) { Data = users };
        }
        #endregion

    }
}
