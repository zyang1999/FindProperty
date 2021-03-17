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
            ViewBag.appointmentMessage = ServiceController.appointments;
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
        public async Task<IActionResult> Create([Bind("id,user_id,property_id,appointment_date,hour,status")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                if (!User.Identity.IsAuthenticated)
                {

                    return Redirect("/Identity/Account/Login");
                }
                //check property got appointment
                //check user book appointment on that day or not

                appointment.user_id = _userManager.GetUserId(HttpContext.User);
                var property = await _context.Appointment.Where(p => p.property_id == appointment.property_id)
                                                         .Where(w => w.appointment_date == appointment.appointment_date)
                                                         .Where(x => x.hour == appointment.hour)
                                                         .Where(z => z.user_id == appointment.user_id)
                                                         .FirstOrDefaultAsync();
                                                         
                if (property != null)
                {
                    ViewBag.error2 = "The property was rent at the time";
                    return View("../Properties/Property_Detail");
                }
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                ViewBag.Message = "The booking was made successful";
            }

            return View("../Properties/Property_Detail");
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
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointment.Any(e => e.id == id);
        }
    }
}
