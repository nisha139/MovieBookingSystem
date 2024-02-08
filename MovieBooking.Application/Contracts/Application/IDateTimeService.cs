namespace MovieBooking.Application.Contracts.Application
{
    public interface IDateTimeService
    {
        DateTimeOffset Now { get; }
    }
}
