using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Contracts.Mailing;
public interface IEmailTemplateService : ITransientService
{
    string GenerateDefaultEmailTemplate<T>(T mailTemplateModel);
}
