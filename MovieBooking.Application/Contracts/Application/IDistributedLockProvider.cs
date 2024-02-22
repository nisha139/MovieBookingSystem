using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Contracts.Application
{
    public interface IDistributedLockProvider
    {
        Task<IDistributedLock> AcquireLockAsync(string resource, TimeSpan expiryTime);
    }
    public interface IDistributedLock : IDisposable
    {
        Task ReleaseLockAsync();
    }
}
