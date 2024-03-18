using MediatR;
using MovieBooking.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Movie.Command.Create
{
    public class CreateMovieBookingCommandRequest : IRequest<ApiResponse<int>>
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public string Duration { get; set; }
        public bool IsActive { get; set; }
    }
}
