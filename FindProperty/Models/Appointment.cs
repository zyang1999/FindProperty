using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FindProperty.Models
{
    public class Appointment
    {
        public int id { get; set; }

        [ForeignKey("FindPropertyUser")]
        public string user_id { get; set; }

        public int property_id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime appointment_date { get; set; }

        public string hour { get; set; }

        public string status { get; set; } = "pending";

        [NotMapped]
        public string customerName { get; set; }

        [NotMapped]
        public string propertyTitle { get; set; }

        [ForeignKey("property_id")]
        public virtual Property property { get; set; }

        [NotMapped]
        public virtual FindProperty.Areas.Identity.Data.FindPropertyUser customer { get; set; }

    }
}
