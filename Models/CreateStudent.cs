using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class CreateStudent
    {
        [Key]
        [Display(Name = "SSN")]
        [Required(ErrorMessage = "The Professor should have a SSN")]
        [StringLength(14, ErrorMessage = "Please Enter a valid SSN", MinimumLength = 14)]
        public string SSN { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "The full name is required ")]
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


    }
}
