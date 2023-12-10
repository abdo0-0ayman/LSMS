using LSMS.data_access;
using Microsoft.AspNetCore.Mvc;

namespace LSMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var data = _context.Admins.ToList();
            return View(data);
        }
    }
}
