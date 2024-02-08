using FluentValidation;
using MovieBooking.Application.Features.Theater.Command.Create;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Seat.Command.Create
{
    public class CreateSeatCommandValidator : AbstractValidator<CreateSeatCommandRequest>
    {
        private readonly IQueryUnitOfWork _query;
        public CreateSeatCommandValidator(IQueryUnitOfWork query)
        {
            _query = query;

            RuleFor(p => p.ScreenId)
          .NotEmpty()
          .WithMessage((_, name) => "ScreenId Name is required in seat");

            RuleFor(p => p.Column)
                .NotEmpty()
                .WithMessage((_, name) => "Number of Columns is require to for seat");

            RuleFor(p => p.Row)
                .NotEmpty()
                .WithMessage((_, name) => "Number of Rows is require to for seat");
            RuleFor(p => p.Status)
                .NotEmpty()
                .WithMessage((_, name) => "Status  require to for seat :  Reserved or Available,");
        }
    }
}
