namespace LSMS.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SSN { get; set; }
        public string PhoneNumber { get; set; }
        public string AcademicEmail { get; set; }
        public string Password { get; set; }


        //relation with CourseProfessor
        public List<CourseProfessor> CourseProfessors { get; set; }
        public List<Enrollment> Enrollments { get; set; }

    }
}
