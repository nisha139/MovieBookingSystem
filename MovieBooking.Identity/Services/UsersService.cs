using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieBooking.Application.Contracts.Application;
using MovieBooking.Application.Contracts.Caching;
using MovieBooking.Application.Contracts.Identity;
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
                                  IConfiguration configuration,
                                  ICacheService cache
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
        #endregion

    }
}
