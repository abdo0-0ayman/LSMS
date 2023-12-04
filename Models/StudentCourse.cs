namespace LSMS.Models
{
    public class StudentCourse
    {
        public String CourseId { get; set; }
        public Course Courses { get; set; }
        public int StudentId { get; set; }
        public Student Students { get; set; }
    }
}
