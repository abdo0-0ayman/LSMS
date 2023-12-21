using static System.Collections.Specialized.BitVector32;

namespace LSMS.Models
{
    public class Hall
    {
        public int id { get; set; }
        public int capacity { get; set; }

        //Relation Groups 
        public List<Lecture> lectures { get; set; }
    }
}
