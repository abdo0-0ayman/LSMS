namespace LSMS.Models
{
    public class Enrollment
    {
        public virtual Student student { get; set; }
        public string studentSSN { get; set; }
        public virtual Lecture lecture { get; set; }
        public string lectureId { get; set; }

    }
}
