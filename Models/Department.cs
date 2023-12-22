
namespace LSMS.Models
{
	public class Department
	{
		public string id { get; set; }
		public string name { get; set; }
		//Relation with Student
		public virtual List<Student> students { get; set; }
		//Relation with Professor
		public virtual List<Professor> professores { get; set; }

		public virtual List<Course> courses { get; set; }
	}
}
