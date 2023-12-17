using LSMS.data_access;
using LSMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace LSMS.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Schedule schedule;
        public ScheduleController(ApplicationDbContext context, Schedule schedule)
        {
            _context = context;
            this.schedule = schedule;
        }

        public IActionResult GenerateSchedule()
        {
            // Call your schedule generation logic
            GenerateLectureSchedule();

            return RedirectToAction("Index", "Home"); // Redirect to the home page or another appropriate view
        }

        private void GenerateLectureSchedule()
        {
            
        }
    }
}
