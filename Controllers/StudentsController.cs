using LSMS.data_access;
using LSMS.Models;
using LSMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace LSMS.Controllers
{
	[CustomAuthorize("Students")]
	[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
	public class StudentsController : Controller
    {
        private readonly Services.IAuthenticationService authService;
        private readonly ApplicationDbContext dbContext;
        private readonly IUpdateService updateService;
        public StudentsController(Services.IAuthenticationService authService, ApplicationDbContext dbContext,IUpdateService updateService)
        {
            this.authService = authService;
            this.dbContext = dbContext;
            this.updateService = updateService;
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
        private bool checkForSchedule()
        {
            var lectures = dbContext.Lectures.Where(e => e.hallId == null).Select(x => x).ToList();
            if (lectures.Count() != 0 || !dbContext.Lectures.Any())
            {
                return true;
            }
            return false;
        }
        public IActionResult schedule()
        {
            var lectures = new List<Lecture>();
            if (checkForSchedule())
            {
                ViewBag.ErrorMessage = "The schedule not generated yet";
                return View(lectures);
            }
            ClaimsPrincipal user = HttpContext.User;
            string username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedIn = dbContext.Students.FirstOrDefault(s => s.SSN == username);
            lectures = dbContext.Enrollments.Where(e => e.studentSSN == loggedIn.SSN).Select(e=>e.lecture).ToList(); // Implement GetLectures() to retrieve your lectures from the database or another source
            return View(lectures);
        }
        [HttpGet]
        public IActionResult editProfile()
        {
            // Retrieve the user's current profile information from the database
            // You may use the logged-in user's username or ID to fetch the user data
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = dbContext.Students.FirstOrDefault(p => p.SSN == username);
            var model = new EditModel()
            {
                name = student.name,
                phoneNumber = student.phoneNumber,
                SSN=student.SSN,
                OldPassword="00000000"
                ,ConfirmPassword= "00000000",
                NewPassword= "00000000"
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult editProfile (EditModel student)
        {
            if(ModelState.IsValid) {
                updateService.UpdateStudent(student);
                return RedirectToAction("Profile");
            }

            return View(student);
        }

        public IActionResult ChangePassword()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = dbContext.Students.FirstOrDefault(p => p.SSN == username);
            var model = new EditModel()
            {
                name = student.name,
                phoneNumber = student.phoneNumber,
                SSN = student.SSN,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(EditModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the current user from the database
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = dbContext.Users.FirstOrDefault(p => p.userName == username);

                // Verify the old password
                var hashedOldPassword = Encoding.UTF8.GetString(user.PasswordHash);
                if (!(BCrypt.Net.BCrypt.Verify(model.OldPassword, hashedOldPassword)))
                {
                    ViewBag.ErrorMessage = "Incorrect old password.";
                    return View(model);
                }

                // Update the password
                updateService.ResetPassword(user.userName, model.NewPassword);

                // Redirect to the profile or another secure page
                return RedirectToAction("Profile", User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
            }

            return View(model);
        }
    }
}
