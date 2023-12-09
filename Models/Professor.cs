namespace LSMS.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SSN { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        //Relation with Courses
        public List<Course> Courses { get; set; }
        public List<CourseProfessor> CourseProfessors { get; set; }


    }
}
