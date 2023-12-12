using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class Professor
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="Full Name")]
        public string Name { get; set; }
        [Display(Name = "SSN")]
        public string SSN { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        // Relation with Department
		public string DepartmentId { get; set; }
		public Department Department { get; set; }

		//Relation with Courses
		public List<Course> Courses { get; set; }
        public List<CourseProfessor> CourseProfessors { get; set; }


    }
}
