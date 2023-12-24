﻿using LSMS.data_access;
using LSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LSMS.Controllers
{
	[CustomAuthorize("Students")]
	[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
	public class StudentsController : Controller
    {
        private readonly Services.IAuthenticationService authService;
        private readonly ApplicationDbContext dbContext;

        public StudentsController(Services.IAuthenticationService authService, ApplicationDbContext dbContext)
        {
            this.authService = authService;
            this.dbContext = dbContext;
        }
		[CustomAuthorize("Students")]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Profile()
		{
            // Retrieve the currently authenticated professor's userName
            ClaimsPrincipal user = HttpContext.User;
            string username = user.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the full professor details from the database using dbContext
            var loggedIn = dbContext.Students.FirstOrDefault(p => p.SSN == username);
			if (loggedIn != null)
			{
				// Pass the professor model to the view
				return View(loggedIn);
			}
			ViewBag.ErrorMessage = "Invalid userName or password";
			return RedirectToAction("Logout", "Home");
		}
        public IActionResult schedule()
        {
            ClaimsPrincipal user = HttpContext.User;
            string username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedIn = dbContext.Students.FirstOrDefault(s => s.SSN == username);
            List<Lecture> lectures = dbContext.Enrollments.Where(e => e.studentSSN == loggedIn.SSN).Select(e=>e.lecture).ToList(); // Implement GetLectures() to retrieve your lectures from the database or another source
            return View(lectures);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
