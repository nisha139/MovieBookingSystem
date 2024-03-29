﻿using Microsoft.AspNetCore.Authorization;

namespace MovieBooking.Identity.Authorizations.Permissions;
public sealed class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = Permission.NameFor(action, resource);
}
