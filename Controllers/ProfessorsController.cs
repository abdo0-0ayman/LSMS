using LSMS.data_access;
using LSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // For Session
using LSMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace LSMS.Controllers
{
	[CustomAuthorize("Professors")]
	[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
	public class ProfessorsController : Controller
    {

        private readonly IAuthenticationService authService;
        private readonly ApplicationDbContext dbContext;

        public ProfessorsController(Services.IAuthenticationService authService, ApplicationDbContext dbContext)
        {
            this.authService = authService;
            this.dbContext = dbContext;
        }
		[CustomAuthorize("Professors")]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Profile()
		{
			string username = User.Identity.Name;
			// Retrieve the full professor details from the database using dbContext
			var loggedIn = dbContext.Professors.FirstOrDefault(p => p.SSN == username);
			if (loggedIn != null)
			{
				// Pass the professor model to the view
				return View(loggedIn);
			}
			ViewBag.ErrorMessage = "Invalid username or password";
			return RedirectToAction("Logout", "Home");
		}

		public IActionResult Index()
        {
            return View();
        }
    }
}
