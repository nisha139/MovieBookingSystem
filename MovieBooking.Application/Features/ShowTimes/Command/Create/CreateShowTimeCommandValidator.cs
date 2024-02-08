using FluentValidation;
using MovieBooking.Application.Features.Theater.Command.Create;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Command.Create
{
    public class CreateShowTimeCommandValidator : AbstractValidator<CreateShowTimeCommandRequest>
    {
        private readonly IQueryUnitOfWork _query;
        public CreateShowTimeCommandValidator(IQueryUnitOfWork query)
        {
            _query = query;

            RuleFor(p => p.ScreenID)
          .NotEmpty()
          .WithMessage((_, name) => "ScreenID is required");

            RuleFor(p => p.MovieId)
                .NotEmpty()
                .WithMessage((_, name) => "MovieId is required");

            RuleFor(p => p.DateTime)
                .NotEmpty()
                .WithMessage((_, name) => "DateTime is required");
        }
    }
}
