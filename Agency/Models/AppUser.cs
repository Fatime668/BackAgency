using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agency.Models
{
    public class AppUser:IdentityUser
    {
        [Required,StringLength(maximumLength:15)]
        public string Firstname { get; set; }
        [Required,StringLength(maximumLength:15)]
        public string Lastname { get; set; }
    }
}
