using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FindProperty.Models
{
    public class Property
    {
        public int id { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "The title should be between 3 to 60 words", MinimumLength = 3)]
        [Display(Name = "Title")]
        public string title { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "The description should be between 3 to 60 words", MinimumLength = 3)]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Required]
        [Display(Name = "Rental Fees (per month)")]
        public int fee { get; set; }

        [Required]
        [Display(Name = "Built-up Size (sq. ft.)")]
        public int size { get; set; }

        [Required]
        [Display(Name = "Property Type")]
        public string type { get; set; }

        [Required]
        [Display(Name = "Furnishing")]
        public string furnishing { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }

        [Display(Name = "Images")]
        public string imagePath { get; set; }

        public string status { get; set; } = "ACTIVE";
        public DateTime created_at { get; set; } = DateTime.Now;

        [NotMapped]
        public List<string> images { get; set; } = new List<string>();
    }

}
