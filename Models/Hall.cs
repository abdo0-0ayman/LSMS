using System.ComponentModel.DataAnnotations;
using static System.Collections.Specialized.BitVector32;

namespace LSMS.Models
{
    public class Hall
    {
        [Required(ErrorMessage ="Please enter a valid id")]
        public string id { get; set; }
        [Required(ErrorMessage = "The capacity should be more than")]
        [Range(50, int.MaxValue, ErrorMessage = "Please enter a value greater than or equal to 50.")]
        public int capacity { get; set; }

        //Relation Groups 
        public List<Lecture> lectures { get; set; }
    }
}
