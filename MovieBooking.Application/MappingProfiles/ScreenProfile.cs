using AutoMapper;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Screen.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.MappingProfiles
{
    public class ScreenProfile : Profile
    {
        public ScreenProfile()
        {
            CreateMap<Domain.Entities.Screen, ScreenDetailDto>();
            CreateMap<Domain.Entities.Screen, ScreenTheaterDetail>();
        }
    }
}
