using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FindProperty.Models
{
    public class Agent
    {
        public int AgentId { get; set; }
        
        [Required]
        [DisplayName("Name")]
        public string name { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        [RegularExpression("^(\\+?6?01)[0-46-9]-*[0-9]{7,8}$", ErrorMessage ="Please provide a provide phone number.")]
        public string phone_number { get; set; }

        public string profile_picture { get; set; }

        [NotMapped]
        [Required]
        [DisplayName("Profile Picture")]
        public IFormFile profilePicture { get; set; } 
    }
}
