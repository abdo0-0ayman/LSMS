using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class CourseProfessor
    {
        public int Id;

        // relation with Courses
        public string CourseId { get; set; }
        public Course Course { get; set; }

        // relation with Professors 
        public int ProfessorId { get; set; }
        public virtual Professor Professor { get; set; }

        // relation with Hall
       // public int HallId { get; set; }
       // public Hall Hall { get; set; }

        // Relation with Students
        public virtual List<Student> Students { get; set; }
        public virtual List<Enrollment> Enrollments { get; set; }

    }
}
