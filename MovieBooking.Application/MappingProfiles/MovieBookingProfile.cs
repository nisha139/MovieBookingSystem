using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.MappingProfiles
{
    public class MovieBookingProfile : Profile
    {
        public MovieBookingProfile() 
        {
            CreateMap<Domain.Entities.MovieBooking, MovieBooking.Application.Features.Movie.Dto.MovieBooking>();

        }
    }
}
