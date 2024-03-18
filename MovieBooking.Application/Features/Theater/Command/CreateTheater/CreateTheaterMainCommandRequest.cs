using MediatR;
using MovieBooking.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Command.CreateTheater
{
    public class CreateTheaterMainCommandRequest : IRequest<ApiResponse<int>>
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int NoOfScreen { get; set; }
        public string ImageUrl { get; set; }
    }
}
