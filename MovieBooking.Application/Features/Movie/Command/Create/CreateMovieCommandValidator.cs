using FluentValidation;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Movie.Command.Create
{
    public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommandRequest>
    {
        private readonly IQueryUnitOfWork _query;
        public CreateMovieCommandValidator(IQueryUnitOfWork query)
        {
            _query = query;

            RuleFor(p => p.Title)
          .NotEmpty()
          .WithMessage((_, name) => "Movie Title is required");

            RuleFor(p => p.Genre)
                .NotEmpty()
                .WithMessage((_, name) => "Movie Genre is required");

            RuleFor(p => p.ReleaseDate)
                .NotEmpty()
                .WithMessage((_, name) => "Movie ReleaseDate is required");

            RuleFor(p => p.Duration)
                .NotEmpty()
                .WithMessage((_, name) => "Movie Duration is required");

            RuleFor(p => p.IsActive)
               .NotEmpty()
               .WithMessage((_, name) => "IsActive or not required");

        }
    }
}
