using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class Lecture
    {
        public string id;

        // relation with Courses
        public string courseId { get; set; }
        public Course course { get; set; }

        // relation with Professors 
        public string professorSSN { get; set; }
        public virtual Professor professor { get; set; }

        // relation with Hall
        public int? hallId { get; set; }
        public Hall hall { get; set; }

        // Relation with Students
        public virtual List<Student> students { get; set; }
        public virtual List<Enrollment> enrollments { get; set; }
        public int lectureNum { get; set; }
       


    }
}
