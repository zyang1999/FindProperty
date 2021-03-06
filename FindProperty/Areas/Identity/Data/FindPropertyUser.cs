using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FindProperty.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the FindPropertyUser class
    public class FindPropertyUser : IdentityUser
    {
        [PersonalData]
        public string name { get; set; }
        [PersonalData]
        public DateTime dob { get; set; }
        [PersonalData]
        public string address { get; set; }
        [PersonalData]
        public string role { get; set; }
    }
}
