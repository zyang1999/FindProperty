using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FindProperty.Data;
using FindProperty.Models;
using Microsoft.AspNetCore.Identity;
using FindProperty.Areas.Identity.Data;
using FindProperty.Controllers;

namespace FindProperty.Views.Appointments
{
    public class AppointmentsController : Controller
    {
        private readonly FindProperty1Context _context;
        private readonly UserManager<FindPropertyUser> _userManager;
        public List<string> appointments;
        public AppointmentsController(FindProperty1Context context, UserManager<FindPropertyUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            ViewBag.appointmentMessage = ServiceController.appointments.ToList();
            ServiceController.appointments.Clear();
            return View(await _context.Appointment.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .FirstOrDefaultAsync(m => m.id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,user_id,property_id,appointment_date,hour,status")] Appointment appointment, string error2)
        {
            
            {

                if (!User.Identity.IsAuthenticated)
                {

                    return Redirect("/Identity/Account/Login");
                }

                var date = DateTime.Now.ToShortDateString();
                DateTime current_Date = DateTime.Parse(date);
                if (current_Date > appointment.appointment_date)
                {

                    return RedirectToAction("Properties_Detail", "Properties", new { id = appointment.property_id, Message = "Incorrect selected date." });

                }


                appointment.user_id = _userManager.GetUserId(HttpContext.User);
                
                var property = await _context.Appointment.Where(p => p.property_id == appointment.property_id)
                                                         .Where(w => w.appointment_date == appointment.appointment_date)                                                       
                                                         .ToListAsync();

                var appointment_exist = property.Where(z => z.user_id == appointment.user_id).FirstOrDefault();
                var other_exist = property.Where(h=>h.hour == appointment.hour).FirstOrDefault();

                if (appointment_exist != null)
                {
                   
                    return RedirectToAction("Properties_Detail", "Properties", new { id = appointment.property_id ,Message = "Appointment was found at the same date" });

                }
                if (other_exist != null)
                {
                    return RedirectToAction("Properties_Detail", "Properties", new
                    {
                        id = appointment.property_id,Message = "Appointment was booked by others"
                    });
                }
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                ServiceController sc = new ServiceController();
                var Message = "The appointment"+appointment.appointment_date+" "+appointment.hour+"was made by"+ _userManager.FindByIdAsync(appointment.user_id).Result.name;
                sc.Index(Message);
               
            }


            return RedirectToAction("Properties_Detail", "Properties", new { id = appointment.property_id, Message = "The booking was made successful" });
            }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,user_id,property_id,appointment_date,hour,status")] Appointment appointment)
        {
            if (id != appointment.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var appointment = await _context.Appointment
                .FirstOrDefaultAsync(m => m.id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();
            var customername = _userManager.FindByIdAsync(appointment.user_id).Result.name;
            var Message = "The Appointment of " + appointment.appointment_date+ " " + appointment.hour + " was removed by " + customername;
            ServiceController sc = new ServiceController();
            sc.Index(Message);
            return RedirectToAction(nameof(View_Appointment));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointment.Any(e => e.id == id);
        }

        public async Task<IActionResult> View_Appointment(){

            var user_id = _userManager.GetUserId(HttpContext.User);
            var appointment = await _context.Appointment.Where(p => p.user_id == user_id).ToListAsync();
            return View("View_Appointment",appointment);
        }
    }
}
