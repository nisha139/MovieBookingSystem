using MediatR;
using MovieBooking.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Command.Create
{
    public class CreateShowTimeCommandRequest : IRequest<ApiResponse<int>>
    {
        public Guid ScreenID { get; set; }
        public Guid MovieId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
