using LSMS.data_access;
using Microsoft.AspNetCore.Mvc;

namespace LSMS.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data=_context.Students.ToList();
            return View(data);
        }
    }
}
