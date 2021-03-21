using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FindProperty.Models
{
    public class User
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?i)^[a-z ,.'-]+$", ErrorMessage ="Please provide a valid name")]
        [DisplayName("Name")]
        public string name { get; set; }
        [Required]
        [DisplayName("Date of Birth")]
        public DateTime dob { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        [RegularExpression("^(\\+?6?01)[0-46-9]-*[0-9]{7,8}$", ErrorMessage = "Please provide a valid phone number")]
        public string PhoneNumber { get; set; }

        public string role { get; set; }
    }
}
