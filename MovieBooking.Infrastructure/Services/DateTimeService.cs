using MovieBooking.Application.Contracts.Application;

namespace MovieBooking.InfraStructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTimeOffset Now => DateTime.UtcNow;
}
