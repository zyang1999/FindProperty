using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FindProperty.Data;
using FindProperty.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using FindProperty.Controllers;
using FindProperty.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace FindProperty.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly UserManager<FindPropertyUser> _userManager;
        private readonly FindProperty1Context _context;
        

        public AppointmentController(FindProperty1Context context,UserManager<FindPropertyUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Insert([Bind("id,property_id,user_id,appointment_date")] Appointment appointment)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return Redirect("/Identity/Account/Login");
            }

            
            
                var userid = _userManager.GetUserId(HttpContext.User);
                _context.Appointment.Add(appointment);
            
               
            
            return View("PropertiesDetail");
        }
        
    }
}
