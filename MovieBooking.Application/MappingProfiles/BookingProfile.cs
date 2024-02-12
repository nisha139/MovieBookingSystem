using AutoMapper;
using MovieBooking.Application.Features.Booking.Dto;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.MappingProfiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Domain.Entities.Booking, BookingDetailDto>();

            CreateMap<PaymentMethod, PaymentMethodDto>();
            CreateMap<Transaction, TransactionDto>();

        }
    }
}
