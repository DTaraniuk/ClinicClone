using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ClinicWebApplication.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
