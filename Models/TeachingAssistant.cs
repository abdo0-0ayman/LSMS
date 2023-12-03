namespace LSMS.Models
{
    public class TeachingAssistant
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SSN { get; set; }
        public string PhoneNum { get; set; }
        public string Password { get; set; }

        // Relation Proffessor 
        public List<Professor> professors { get; set; }
        public List<ProfessorTeachingAssistant> professorTeachingAssistants { get; set; }

        //// Relation courses 
        public List<Course> courses { get; set; }
        public List<CourseTeachingAssistant> courseTeachingAssistants { get; set; }

        //// Relation sections
        public List<Section> sections { get; set; }
    }
}
