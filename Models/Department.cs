
using System.ComponentModel.DataAnnotations;

namespace LSMS.Models
{
	public class Department
	{
        [Required(ErrorMessage = "Please enter a valid id")]
        public string id { get; set; }
        [Required(ErrorMessage = "Please enter a valid department name")]
        public string name { get; set; }
		//Relation with Student
		public virtual List<Student> students { get; set; }
		//Relation with Professor
		public virtual List<Professor> professores { get; set; }

		public virtual List<Course> courses { get; set; }
	}
}
