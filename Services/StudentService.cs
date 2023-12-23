using LSMS.data_access;
using LSMS.Models;

namespace LSMS.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        
        public StudentService (ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Student student)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Student GetStudent(string id)
        {
            throw new NotImplementedException();
        }

        public Student Update(string id, Student student)
        {
            throw new NotImplementedException();
        }
    }
}
