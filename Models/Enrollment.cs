namespace LSMS.Models
{
    public class Enrollment
    {
        public virtual Student Student { get; set; }
        public int StudentId { get; set; }
        public virtual Lecture Lecture { get; set; }
        public string LectureId { get; set; }

    }
}
