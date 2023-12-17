using LSMS.data_access;
using LSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LSMS.Controllers
{
	public class DisplayCourseProfessors : Controller
	{
		private readonly ApplicationDbContext dbContext;
		public DisplayCourseProfessors(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public ActionResult ProfessorTeachCourse()
        {
            return View();
        }
        [HttpPost]
		public ActionResult ProfessorTeachCourse(string courseId)
        {
            var course = dbContext.Lectures.Where(e => e.CourseId == courseId).Select(e => e.Professor).ToList();

            if (course.Count()!=0)
            {
                return View(course.ToList());
            }
            ViewBag.ErrorMessage = "there is no course";
            return View(course);
        }
    }
}

		
		