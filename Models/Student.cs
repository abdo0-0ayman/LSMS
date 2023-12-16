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

		// Relation with Department
		public string DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        //relation with CourseProfessor
        public virtual List<CourseProfessor> CourseProfessors { get; set; }
        public virtual List<Enrollment> Enrollments { get; set; }

    }
}
