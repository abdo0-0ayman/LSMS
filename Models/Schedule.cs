using static OfficeOpenXml.ExcelErrorValue;

namespace LSMS.Models
{
    public class Schedule
    {
        public string Id { get; set; }  // Optional: Give a Id to the schedule (e.g., "Fall 2023")
        public List<Lecture> Lectures { get; set; }
    }
}
