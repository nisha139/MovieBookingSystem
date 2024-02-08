using MediatR;
using MovieBooking.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Screen.Command.Update
{
    public class UpdateScreenRequestCommand : IRequest<ApiResponse<string>>
    {
        public Guid Id { get; set; }
        public Guid ScreenId { get; set; }
        public int Capacity { get; set; }

    }
}
