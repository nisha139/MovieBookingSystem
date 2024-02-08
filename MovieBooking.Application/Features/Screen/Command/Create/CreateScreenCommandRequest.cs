using MediatR;
using MovieBooking.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Screen.Command.Create
{
    public class CreateScreenCommandRequest : IRequest<ApiResponse<int>>
    {
        public Guid TheaterId {  get; set; }
        public int Capacity { get; set; }
    }
}
