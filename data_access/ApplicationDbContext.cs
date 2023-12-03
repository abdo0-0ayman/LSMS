using Microsoft.EntityFrameworkCore;

namespace LSMS.data_access
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
