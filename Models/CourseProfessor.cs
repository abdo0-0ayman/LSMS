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
        public Professor Professor { get; set; }

        // relation with Hall
        public int HallId { get; set; }
        public Hall Hall { get; set; }

        // Relation with Students
        public List<Student> Students { get; set; }
        public List<Enrollment> Enrollments { get; set; }

    }
}
