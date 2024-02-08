using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Security
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class AuthorizeAttribute : Attribute
    {
        public AuthorizeAttribute() { }

        public string Roles { get; set; } = string.Empty;

        //   public ModuleType Modules { get; set; } = ModuleType.TODOITEM;

        public string ModuleType { get; set; } = string.Empty;


        public string Actions { get; set; } = string.Empty;



        public string Policy { get; set; } = string.Empty;
    }
}
