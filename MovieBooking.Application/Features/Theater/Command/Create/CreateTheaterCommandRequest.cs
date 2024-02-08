using MediatR;
using MovieBooking.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Command.Create
{
    public class CreateTheaterCommandRequest : IRequest<ApiResponse<int>>
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int NoOfScreen { get; set; }
    }
}
