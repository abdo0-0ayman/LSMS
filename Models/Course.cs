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
        public virtual List<Professor> Professors { get; set; }
        public virtual List<CourseProfessor> CourseProfessors { get; set; }

    }
}
