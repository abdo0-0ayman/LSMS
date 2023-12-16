namespace LSMS.Models
{
	public class Department
	{
		public string Id { get; set; }
		public string Name { get; set; }

		//Relation with Student
		public virtual List<Student> Students { get; set; }

		//Relation with Professor
		public virtual List<Professor> Professores { get; set; }
	}
}
