namespace LSMS.Models
{
	public class Department
	{
		public string Id { get; set; }
		public string Name { get; set; }

		//Relation with Student
		List<Student> Students { get; set; }

		//Relation with Professor
		List<Professor> Professores { get; set; }
	}
}
