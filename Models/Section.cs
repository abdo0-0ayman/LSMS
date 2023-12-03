namespace LSMS.Models
{
    public class Section
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public List<Student> Students { get; set; }
        public List<StudentSection> StudentSections { get; set; }

        public Course Course { get; set; }
        public string CourseID { get; set; }

        public TeachingAssistant TeachingAssistant { get; set; }
        public int TeachingAssistantID { get; set; }

        public Hall Hall { get; set; }
        public int HallID { get; set; }
    }
}
