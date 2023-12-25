using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class EditModel
    {
        public string SSN { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }

        [Required(ErrorMessage = "Old password is required.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
