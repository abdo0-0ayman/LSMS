using static System.Collections.Specialized.BitVector32;

namespace LSMS.Models
{
    public class Hall
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public string Type { get; set; }

        //Relation Groups 
        public List<Lecture> Lectures { get; set; }
    }
}
