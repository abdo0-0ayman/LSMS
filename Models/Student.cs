using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class Student
    {
        [Required]
        [Key]
        public string SSN { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public string academicEmail { get; set; }
        //public string password { get; set; }

		// Relation with Department
		public string departmentId { get; set; }
        public virtual Department department { get; set; }
        //relation with CourseProfessor
        public virtual List<Lecture> lectures { get; set; }
        public virtual List<Enrollment> enrollments { get; set; }

    }
}
