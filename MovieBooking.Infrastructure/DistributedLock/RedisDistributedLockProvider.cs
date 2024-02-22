using System;
using System.Threading.Tasks;
using MovieBooking.Application.Contracts.Application;
using StackExchange.Redis;

namespace MovieBooking.Infrastructure.DistributedLock
{
    public class RedisDistributedLockProvider : IDistributedLockProvider
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisDistributedLockProvider(string connectionString)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
        }

        public async Task<IDistributedLock> AcquireLockAsync(string resource, TimeSpan expiryTime)
        {
            var database = _connectionMultiplexer.GetDatabase();
            var lockKey = $"lock:{resource}";

            while (true)
            {
                var lockValue = Guid.NewGuid().ToString();
                if (await database.StringSetAsync(lockKey, lockValue, expiryTime, When.NotExists))
                {
                    return new RedisDistributedLock(database, lockKey, lockValue);
                }
                await Task.Delay(50);
            }
        }
    }

    public class RedisDistributedLock : IDistributedLock, IDisposable
    {
        private readonly IDatabase _database;
        private readonly string _lockKey;
        private readonly string _lockValue;

        public RedisDistributedLock(IDatabase database, string lockKey, string lockValue)
        {
            _database = database;
            _lockKey = lockKey;
            _lockValue = lockValue;
        }

        public async Task ReleaseLockAsync()
        {
            await _database.LockReleaseAsync(_lockKey, _lockValue);
        }

        public void Dispose()
        {
            // Dispose resources if needed
        }
    }
}
