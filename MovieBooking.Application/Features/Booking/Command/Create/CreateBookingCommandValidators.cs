using FluentValidation;
using MovieBooking.Application.Features.Movie.Command.Create;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Command.Create
{
    public class CreateBookingCommandValidators
   : AbstractValidator<CreateBookingCommandRequest>
    {
        private readonly IQueryUnitOfWork _query;
        public CreateBookingCommandValidators(IQueryUnitOfWork query)
        {
            _query = query;

          

            RuleFor(p => p.SeatsBooked)
                .NotEmpty()
                .WithMessage((_, name) => "SeatsBooked is required");

            RuleFor(p => p.ShowtimeID)
                .NotEmpty()
                .WithMessage((_, name) => "ShowtimeID is required");

        }
    }
}
