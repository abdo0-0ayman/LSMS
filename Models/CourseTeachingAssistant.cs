namespace LSMS.Models
{
    public class CourseTeachingAssistant
    {
        public string CourseId { get; set; }
        public Course Courses { get; set; }
        public int teachingAssistantId { get; set; }
        public TeachingAssistant TeachingAssistants { get; set; }
    }
}
