using LSMS.data_access;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace LSMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        
        public AdminController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
