namespace LSMS.Models
{
    public class Professor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SSN { get; set; }
        public string PhoneNum { get; set; }
        public string Password { get; set; }

        public List<Course> Courses { get; set; }
        public List<CourseProfessor> CourseProfessors { get; set; }


        public List<TeachingAssistant> TeachingAssistants { get; set; }

        public List<ProfessorTeachingAssistant> ProfessorTeachingAssistants { get; set; }


        public List<Group> groups { get; set; }
    }
}
