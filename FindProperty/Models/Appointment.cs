using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FindProperty.Models
{
    public class Appointment
    {
        public int id { get; set; }

        public int user_id { get; set; }

        public int property_id { get; set; }

        public DateTime appointment_date{get;set;}

        public string hour { get; set; }

        public string status { get; set; }
    }
}
