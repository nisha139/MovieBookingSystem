using MediatR;
using MovieBooking.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Seat.Command.Create
{
    public class CreateSeatCommandRequest : IRequest<ApiResponse<int>>
    {
        public Guid ScreenId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Status { get; set; } // Reserved, Available, etc.
    }
}
