using static System.Collections.Specialized.BitVector32;

namespace LSMS.Models
{
    public class Hall
    {
        public int ID { get; set; }
        public int Capacity { get; set; }
        public string Type { get; set; }

        // Relation Sections 
        public List<Section> sections { get; set; }

        //// Relation Groups 
        public List<Group> groups { get; set; }
    }
}
