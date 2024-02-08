using MediatR;
using MovieBooking.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Command.Update
{
    public class UpdateShowTimeCommandRequest : IRequest<ApiResponse<string>>
    {
        public Guid ShowTimeId { get; set; }
        public Guid ScreenID { get; set; }
        public Guid MovieId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
