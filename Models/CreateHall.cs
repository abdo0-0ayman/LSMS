using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class CreateHall
    {
        [Required(ErrorMessage = "Please enter a valid id")]
        public string id { get; set; }
        [Required(ErrorMessage = "The capacity should be more than")]
        [Range(50, int.MaxValue, ErrorMessage = "Please enter a value greater than or equal to 50.")]
        public int capacity { get; set; }
    }
}
