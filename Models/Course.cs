using static System.Collections.Specialized.BitVector32;
using System.Text.RegularExpressions;

namespace LSMS.Models
{
    public class Course
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }


        // Relation with Professors  
        public List<Professor> Professors { get; set; }
        public List<CourseProfessor> CourseProfessors { get; set; }

    }
}
