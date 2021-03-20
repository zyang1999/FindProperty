using FindProperty.Areas.Identity.Data;
using FindProperty.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FindProperty.Controllers
{
    public class UsersController : Controller
    {
        private UserManager<FindPropertyUser> _userManager;

        public UsersController(UserManager<FindPropertyUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index(string message)
        {
            ViewBag.message = message;
            return View(_userManager.Users.Where(x => x.Id != _userManager.GetUserId(HttpContext.User)).ToList());
        }
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User userObj, string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            ModelState.Remove("Email");
            if (ModelState.IsValid)
            {
                user.name = userObj.name;
                user.PhoneNumber = userObj.PhoneNumber;
                user.role = userObj.role;
                user.dob = userObj.dob;
                await _userManager.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                var duplicatedEmail = _userManager.Users.Where(x => x.Email == user.Email).FirstOrDefault();
                if (duplicatedEmail != null)
                {
                    ViewBag.message = "Email was taken";
                    return View(user);
                }
                FindPropertyUser newUser = new FindPropertyUser
                {
                    Email = user.Email,
                    UserName = user.Email,
                    name = user.name,
                    PhoneNumber = user.PhoneNumber,
                    role = "admin",
                    dob = user.dob,
                    EmailConfirmed = true,
                };
                IdentityResult result = await _userManager.CreateAsync(newUser, "Password@123");
                return RedirectToAction(nameof(Index), new { message = "New User was added." });
            }
            return View(user);
        }

        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index), new { message = "User was deleted" });
        }
    }
}
