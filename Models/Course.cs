using static System.Collections.Specialized.BitVector32;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class Course
    {
        [Display(Name = "id")]
        [Required(ErrorMessage = "The Professor should have a ID")]
        public string id { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "The course name is required ")]

        public string name { get; set; }
        [Required(ErrorMessage ="The course hours is required ")]
        [Range(1,5,ErrorMessage ="The course hours should be between 1 and 5 hours")]
        public int hours { get; set; }
        [Required(ErrorMessage = "Department ID is required")]
        public string? departmentId { get; set; }


        // Relation with Professors  
        public virtual List<Professor> professors { get; set; }
        public virtual List<Lecture> lectures { get; set; }

        public virtual Department department { get; set; }

    }
}
