using LSMS.data_access;
using LSMS.Models;

namespace LSMS.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly ApplicationDbContext _context;

        public ProfessorService(ApplicationDbContext context)
        {
            _context = context;
        }


        public void Add(Professor professor)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Professor Get(string id)
        {
            throw new NotImplementedException();
        }

        public Professor Update(string id, Professor newProfessor)
        {
            throw new NotImplementedException();
        }
    }
}
