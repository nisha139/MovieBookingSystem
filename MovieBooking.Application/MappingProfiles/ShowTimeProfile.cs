using AutoMapper;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.ShowTimes.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.MappingProfiles
{
    public class ShowTimeProfile : Profile
    {
        public ShowTimeProfile()
        {
            CreateMap<Domain.Entities.Showtime, ShowTimeDetailDto>();
        }
    }
}
