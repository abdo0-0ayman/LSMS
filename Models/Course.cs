using static System.Collections.Specialized.BitVector32;
using System.Text.RegularExpressions;

namespace LSMS.Models
{
    public class Course
    {
        public string id { get; set; }
        public string name { get; set; }
        public int hours { get; set; }
        

        // Relation with Professors  
        public virtual List<Professor> professors { get; set; }
        public virtual List<Lecture> lectures { get; set; }

    }
}
