namespace LSMS.Models
{
    public class CourseProfessor
    {
        public string CourseId { get; set; }
        public Course Courses { get; set; }
        public int ProfessorId { get; set; }
        public Professor Professors { get; set; }
    }
}
