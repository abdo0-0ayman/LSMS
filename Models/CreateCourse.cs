using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class CreateCourse
    {
        [Display(Name = "id")]
        [Required(ErrorMessage = "The Professor should have a ID")]
        public string id { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "The course name is required ")]

        public string name { get; set; }
        [Required(ErrorMessage = "The course hours is required ")]
        [Range(1, 5, ErrorMessage = "The course hours should be between 1 and 5 hours")]
        public int hours { get; set; }
        [Required(ErrorMessage = "Department ID is required")]
        public string? departmentId { get; set; }
    }
}
