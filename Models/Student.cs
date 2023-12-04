namespace LSMS.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SSN { get; set; }
        public string PhoneNumber { get; set; }
        public string AcademicEmail { get; set; }

        // Relation Group
        public List<Group> Groups { get; set; }
        public List<StudentGroup> StudentGroups { get; set; }

        // Relation Section
        public List<Section> Sections { get; set; }
        public List<StudentSection> StudentSections { get; set; }

        // Relation Course
        public List<Course> Courses { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
    }
}
