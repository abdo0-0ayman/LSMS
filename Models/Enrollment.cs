namespace LSMS.Models
{
    public class Enrollment
    {
        public Student Student { get; set; }
        public int StudentId { get; set; }
        public CourseProfessor CourseProfessor { get; set; }
        public int CourseProfessorId { get; set; }

    }
}
