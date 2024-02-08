using MovieBooking.Application.Contracts.Responses;
using Starter.Application.Features.Common;

namespace MovieBooking.Application.Features.Common;
public class ApiResponse<T> : Response,IDataResponse<T>
{
    public T Data { get; set; }

    public List<string>? Messages { get; set; }
    public IDictionary<string, object>? Metadata { get; set; } = new Dictionary<string, object>();
}
