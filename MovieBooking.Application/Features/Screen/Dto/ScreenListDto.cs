using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Screen.Dto
{
    public class ScreenListDto
    {
        public Guid Id { get; set; }
        public int Capacity { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public string? CreatedBy { get; set; }

        public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;

        public string? ModifiedBy { get; set; }

        public DateTimeOffset DeletedOn { get; set; } = DateTimeOffset.UtcNow;

        public string? DeletedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
        public List<ShowtimeDto> Showtimes { get; set; }
        public List<Domain.Entities.Seat> Seats { get; set; }

    }
}
