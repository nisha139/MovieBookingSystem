using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Contracts.Identity
{
    public interface IUserService
    {
        Task<bool> HasPermissionAsync(string? userId, string permission, CancellationToken cancellationToken = default);
    }
}
