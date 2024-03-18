using AutoMapper;
using MovieBooking.Application.Features.Theater.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.MappingProfiles
{
    public class TheaterProfile : Profile
    {
        public TheaterProfile()
        {
            CreateMap<Domain.Entities.Theater,TheaterDetailDto>();
            CreateMap<Domain.Entities.Theater, TheaterDto>();
            CreateMap<Domain.Entities.Showtime,ShowtimeDto>();
            CreateMap<Domain.Entities.Screen, ScreenDto>();
            CreateMap<Domain.Entities.TheaterMain, TheaterMainDetailDto>();
        }
    }
}
