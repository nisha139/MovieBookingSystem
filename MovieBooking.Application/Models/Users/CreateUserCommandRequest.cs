using MediatR;
using MovieBooking.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Models.Users
{
    public class CreateUserCommandRequest : IRequest<ApiResponse<int>>
    {
       // public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool EmailConformed { get; set; }
    }
}
