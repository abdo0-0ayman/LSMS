using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class Student
    {
        [Required]
        [Key]
        public string SSN { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string AcademicEmail { get; set; }
        public string Password { get; set; }

		// Relation with Department
		public string DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        //relation with CourseProfessor
        public virtual List<Lecture> Lectures { get; set; }
        public virtual List<Enrollment> Enrollments { get; set; }

    }
}
