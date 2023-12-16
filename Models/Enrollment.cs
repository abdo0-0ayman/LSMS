namespace LSMS.Models
{
    public class Enrollment
    {
        public virtual Student Student { get; set; }
        public int StudentId { get; set; }
        public virtual CourseProfessor CourseProfessor { get; set; }
        public int CourseProfessorId { get; set; }

    }
}
