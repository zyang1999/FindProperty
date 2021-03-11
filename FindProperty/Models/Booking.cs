using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FindProperty.Models
{
    public class Booking
    {
        public int id { get; set; }

        
        public string user_id { get; set; }

        public int property_id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime appointment_date { get; set; }

        public string hour { get; set; }


        public string status { get; set; } = "pending";

    }
}
