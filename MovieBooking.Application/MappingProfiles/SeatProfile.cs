using AutoMapper;
using MovieBooking.Application.Features.Screen.Dto;
using MovieBooking.Application.Features.Seat.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.MappingProfiles
{
    public class SeatProfile : Profile
    {
        public SeatProfile()
        {
            CreateMap<Domain.Entities.Seat, SeatDetailDto>();
        }
    }
}
