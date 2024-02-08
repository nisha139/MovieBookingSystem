using FluentValidation;
using MovieBooking.Application.Features.Movie.Command.Create;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Command.Create
{
    public class CreateTheaterCommandValidator : AbstractValidator<CreateTheaterCommandRequest>
    {
        private readonly IQueryUnitOfWork _query;
        public CreateTheaterCommandValidator(IQueryUnitOfWork query)
        {
            _query = query;

            RuleFor(p => p.Name)
          .NotEmpty()
          .WithMessage((_, name) => "Theater Name is required");

            RuleFor(p => p.Location)
                .NotEmpty()
                .WithMessage((_, name) => "Theater Location is required");

            RuleFor(p => p.NoOfScreen)
                .NotEmpty()
                .WithMessage((_, name) => "Location of theater is required");
        }
    }
}
