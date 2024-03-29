﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Controllers.Base;
using MovieBooking.Application.Contracts.Identity;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Models.Users;
using MovieBooking.Identity.Authorizations.Permissions;
using MovieBooking.Identity.Authorizations;
using MovieBooking.Identity.Services;
using Action = MovieBooking.Identity.Authorizations.Action;

[Route("api/[controller]")]
[ApiController]
public class UserController : BaseApiController
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(ILogger<UserController> logger, IUserService userService, IConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    //[Authorize(Roles = "Administrator,User")]
    [HttpGet("{id}")]
    public async Task<ApiResponse<UserDetailsDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _userService.GetUserDetailsAsync(id, cancellationToken);
    }

    //[Authorize(Roles = "Administrator")]
    [HttpPost("search")]
    [MustHavePermission(Action.Search, Resource.Users)]
    public async Task<IPagedDataResponse<UserListDto>> GetListAsync(UserListFilter filter, CancellationToken cancellationToken)
    {
        return await _userService.SearchAsync(filter, cancellationToken);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateUserRequest request)
    {
        try
        {
            var response = await _userService.CreateUserAsync(request);
            return StatusCode((int)response.StatusCode, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return StatusCode((int)HttpStatusCodes.InternalServerError, new ApiResponse<string>
            {
                Success = false,
                Message = "An error occurred while creating the user."
            });
        }
    }

    //[Authorize(Roles = "Administrator")]
    [HttpPut("{id}")]
    [MustHavePermission(Action.Update, Resource.Users)]
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
        return await _userService.UpdateAsync(new UpdateUserRequest() { user = request, Origin = GetOriginFromRequest(_configuration) });
    }

    //[Authorize(Roles = "Administrator")]
    [HttpDelete("{id}")]
    [MustHavePermission(Action.Delete, Resource.Users)]
    public async Task<ApiResponse<string>> DeleteAsync(string id)
    {
        return await _userService.DeleteAsync(id);
    }
}
