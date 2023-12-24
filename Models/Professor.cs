using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class Professor
    {
        [Key]
        [Display(Name = "SSN")]
        public string SSN { get; set; }
        [Display(Name="Full Name")]
        public string name { get; set; }
       
        [Display(Name = "Phone Number")]
        public string phoneNumber { get; set; }
        //public string password { get; set; }

        // Relation with Department
		public string departmentId { get; set; }
		public virtual Department department { get; set; }

		//Relation with Courses
		public virtual List<Course> courses { get; set; }
        public virtual List<Lecture> lectures { get; set; }


    }

}
