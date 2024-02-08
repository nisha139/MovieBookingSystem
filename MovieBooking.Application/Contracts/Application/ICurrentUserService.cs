namespace MovieBooking.Application.Contracts.Application
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
    }
}
