using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Contracts.Mailing;
public interface IMailService : ITransientService
{
    Task SendAsync(MailRequest request, CancellationToken ct);
}
