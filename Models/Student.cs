using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class Student
    {
        [Key]
        [Display(Name = "SSN")]
        [Required(ErrorMessage = "The Professor should have a SSN")]
        [StringLength(16, ErrorMessage = "Please Enter a valid SSN", MinimumLength = 16)]
        public string SSN { get; set; }
        public string name { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "The phone number is required")]
        public string phoneNumber { get; set; }
        [Required(ErrorMessage = "Academic Email is required")]
        public string academicEmail { get; set; }
        //public string password { get; set; }

        // Relation with Department
        [Required(ErrorMessage = "Department ID is required")]
        public string departmentId { get; set; }
        public virtual Department department { get; set; }
        //relation with CourseProfessor
        public virtual List<Lecture> lectures { get; set; }
        public virtual List<Enrollment> enrollments { get; set; }

    }
}
