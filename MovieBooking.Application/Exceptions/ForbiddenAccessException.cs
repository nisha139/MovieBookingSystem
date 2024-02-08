using System.Net;

namespace MovieBooking.Application.Exceptions;

public class ForbiddenAccessException : CustomException
{
    public ForbiddenAccessException(string message, List<string>? errors = default) 
        : base(message, errors, HttpStatusCode.Forbidden) { }
}
