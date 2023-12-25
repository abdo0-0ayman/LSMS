using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
    public class CreateDepartment
    {
        [Required(ErrorMessage = "Please enter a valid id")]
        public string id { get; set; }
        [Required(ErrorMessage = "Please enter a valid department name")]
        public string name { get; set; }
    }
}
