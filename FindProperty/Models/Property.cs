using Microsoft.AspNetCore.Http;
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
        [Display(Name = "Description")]
        public string description { get; set; }

        [Required]
        [Display(Name = "Prices or Rental Fees (per month)")]
        public int fee { get; set; }

        [Required]
        [Display(Name = "Built-up Size (sq. ft.)")]
        public int size { get; set; }

        [Required]
        [Display(Name = "For Sale OR Rent")]
        public string type { get; set; }

        [Required]
        [Display(Name ="Property Type")]
        public string property_type { get; set; }

        [Required]
        [Display(Name = "Furnishing")]
        public string furnishing { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }

        public string imagePath { get; set; }

        [Required]
        [ForeignKey("Agent")]
        [Display(Name = "Agent")]
        public int AgentID{ get; set; }

        public DateTime created_at { get; set; } = DateTime.Now;

        [NotMapped]
        public List<string> images { get; set; } = new List<string>();
        public virtual Agent Agent { get; set; }

        [NotMapped]
        [Required]
        [Display(Name = "Images of the property")]
        public List<IFormFile> imagesFiles { get; set; }

        [NotMapped]
        public SelectList propertiesList { get; set; }
    }

}
