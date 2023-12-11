using LSMS.data_access;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace LSMS.Controllers
{
    public class AdminsController : Controller
    {
        private readonly Services.IAuthenticationService authService;
        private readonly ApplicationDbContext dbContext;

        public AdminsController(Services.IAuthenticationService authService ,ApplicationDbContext dbContext)
        {
            this.authService = authService;
            this.dbContext = dbContext;
        }

        public IActionResult Profile()
        {
            // Retrieve the currently authenticated professor's username
            string username = User.Identity.Name;

            // Retrieve the full professor details from the database using dbContext
            var loggedInAdmin = dbContext.Admins.FirstOrDefault(p => p.UserName == username);

            if (loggedInAdmin != null)
            {
                // Pass the professor model to the view
                return View(loggedInAdmin);
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
