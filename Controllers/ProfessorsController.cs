using LSMS.data_access;
using Microsoft.AspNetCore.Mvc;

namespace LSMS.Controllers
{
    public class ProfessorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProfessorsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var data=_context.Professors.ToList();
            return View();
        }
    }
}
