using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MovieBooking.Application.Contracts.Application;
using MovieBooking.Application.Contracts.Caching;
using MovieBooking.Application.Contracts.Identity;
using MovieBooking.Application.Interfaces;
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

                                  IJobService jobService,

                                  ICacheService cache,
                                  ICacheKeyService cacheKey) : IUserService
    {
        private readonly AppIdentityDbContext _db = db;
        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IJobService _jobService = jobService;

        private readonly ICacheService _cache = cache;
        private readonly ICacheKeyService _cacheKey = cacheKey;

    }
}
