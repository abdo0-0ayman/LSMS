using LSMS.Models;

namespace LSMS.Services
{
    public interface IProfessorService
    {
        void Add(Professor professor);
        Professor Update(string id, Professor newProfessor);
        void Delete(string id);
        Professor Get(string id);
    }
}
