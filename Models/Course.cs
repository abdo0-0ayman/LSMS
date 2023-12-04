using static System.Collections.Specialized.BitVector32;
using System.Text.RegularExpressions;

namespace LSMS.Models
{
    public class Course
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }
        public int DepartmentId { get; set; }

        //Relation Group
        public List<Group> groups { get; set; }

        // Relation student with shit
        public List<Student> Students { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }

        // Relation section 
        public List<Section> sections { get; set; }

        // Relation Proffessor with shit 
        public List<Professor> professors { get; set; }
        public List<CourseProfessor> CourseProfessors { get; set; }

        // Relation TeachingAssistant with shit 
        public List<TeachingAssistant> teachingAssistants { get; set; }
        public List<CourseTeachingAssistant> courseTeachingAssistants { get; set; }
    }
}
