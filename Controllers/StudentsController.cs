using LSMS.data_access;
using Microsoft.AspNetCore.Mvc;

namespace LSMS.Controllers
{
    public class StudentsController : Controller
    {
        private readonly Services.IAuthenticationService authService;
        private readonly ApplicationDbContext dbContext;

        public StudentsController(Services.IAuthenticationService authService, ApplicationDbContext dbContext)
        {
            this.authService = authService;
            this.dbContext = dbContext;
        }

        public IActionResult Profile()
        {
            // Retrieve the currently authenticated professor's username
            string username = User.Identity.Name;

            // Retrieve the full professor details from the database using dbContext
            var loggedInStudent = dbContext.Students.FirstOrDefault(p => p.SSN == username);

            if (loggedInStudent != null)
            {
                // Pass the professor model to the view
                return View(loggedInStudent);
            }

            // Handle the case where the professor is not found
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            authService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
