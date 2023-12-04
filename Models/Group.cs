using System;

namespace LSMS.Models
{
    public class Group
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public List<Student> Students { get; set; }
        public List<StudentGroup> StudentGroups { get; set; }

        public Course Course { get; set; }
        public string CourseID { get; set; }

        public Professor Professor { get; set; }
        public int ProfessorID { get; set; }

        public Hall Hall { get; set; }
        public int HallID { get; set; }
    }
}
