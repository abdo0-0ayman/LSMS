using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class CustomProfessor
    {
        [Key]
        [Display(Name = "SSN")]
        [Required(ErrorMessage = "The SSN is required")]
        [StringLength(16,MinimumLength =16,ErrorMessage ="The SSN must be of a 16 character")]
        public string SSN { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "The Full Name is required")]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50,MinimumLength =8,ErrorMessage ="The Password should be between 8 and 50 chars")]
        public string Password { get; set; }

        [Display(Name = "Department Id")]
        [Required(ErrorMessage = "Depatment Id is required")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "The Department Id should be between 8 and 50 chars")]
        // Relation with Department
        public string DepartmentId { get; set; }
    }
}
