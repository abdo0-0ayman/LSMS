namespace LSMS.Models
{
    public class ProfessorTeachingAssistant
    {
        public int TeachingAssistantId { get; set; }
        public TeachingAssistant TeachingAssistants { get; set; }
        public int ProffessorId { get; set; }
        public Professor Professors { get; set; }
    }
}
