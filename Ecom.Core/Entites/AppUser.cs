using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.Entites
{
    public class AppUser :IdentityUser
    {
        public string ?DisplayName { get; set; }
        public Address? Address { get; set; }
    }
}
