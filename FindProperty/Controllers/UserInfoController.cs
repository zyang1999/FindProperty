using FindProperty.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindProperty.Controllers
{
    public class UserInfoController : Controller
    {
        private UserManager<FindPropertyUser> userManager;
        
        public UserInfoController(UserManager<FindPropertyUser> usrMgr)
        {
            userManager = usrMgr;
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }
    }
}
