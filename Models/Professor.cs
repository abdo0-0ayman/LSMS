using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class Professor
    {
        [Key]
        [Display(Name = "SSN")]
        public string SSN { get; set; }
        [Display(Name="Full Name")]
        public string Name { get; set; }
       
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        // Relation with Department
		public string DepartmentId { get; set; }
		public virtual Department Department { get; set; }

		//Relation with Courses
		public virtual List<Course> Courses { get; set; }
        public virtual List<Lecture> Lectures { get; set; }


    }
}
