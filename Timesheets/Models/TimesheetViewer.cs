using System.ComponentModel.DataAnnotations;

namespace Timesheets.Models
{
    public class TimesheetViewer
    {
        [Key]
        public string Date { get; set; }
        public string Project { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double TotalHours { get; set; }
    }
}
