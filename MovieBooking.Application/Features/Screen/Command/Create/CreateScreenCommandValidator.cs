using FluentValidation;
using MovieBooking.Application.Features.Movie.Command.Create;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Screen.Command.Create
{
    public class CreateScreenCommandValidator : AbstractValidator<CreateScreenCommandRequest>
    {
        private readonly IQueryUnitOfWork _query;

        public CreateScreenCommandValidator(IQueryUnitOfWork query)
        {
            _query = query;

            RuleFor(p => p.Capacity)
         .NotEmpty()
         .WithMessage((_, name) => "Screen Number is required");
        }
    }
}
