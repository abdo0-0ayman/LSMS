using LSMS.data_access;
using LSMS.Models;
using Microsoft.AspNetCore.Mvc;

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
			string userName = User.Identity.Name;

			// Retrieve the full professor details from the database using dbContext
			var loggedIn = dbContext.Students.FirstOrDefault(p => p.SSN == userName);
			if (loggedIn != null)
			{
				// Pass the professor model to the view
				return View(loggedIn);
			}
			ViewBag.ErrorMessage = "Invalid userName or password";
			return RedirectToAction("Logout", "Home");
		}

		public IActionResult Index()
        {
            return View();
        }
    }
}
