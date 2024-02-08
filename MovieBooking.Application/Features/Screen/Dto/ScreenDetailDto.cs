using MovieBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Screen.Dto
{
    public class ScreenDetailDto
    {
        public Guid Id { get; set; }
        public Guid TheaterId { get; set; }
        public int Capacity { get; set; }
      
    }
}
