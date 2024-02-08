﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Identity.Models
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public string? CreatedBy { get; init; }
        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;
    }
}
