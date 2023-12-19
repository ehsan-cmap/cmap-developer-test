using System.Text;
using Timesheets.Models;

namespace Timesheets.Services
{
    public interface ICsvService
    {
        string CreateCSV(IList<Timesheet> timesheets);
    }

    public class CSVExportService : ICsvService
    {
        public string CreateCSV(IList<Timesheet> timesheets)
        {
            if (timesheets is null || timesheets.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($"{nameof(Timesheet.Id)}, {nameof(TimesheetEntry.FirstName)}, {nameof(TimesheetEntry.LastName)}, {nameof(TimesheetEntry.Date)}, {nameof(TimesheetEntry.Project)}, {nameof(TimesheetEntry.Hours)}, {nameof(Timesheet.TotalHours)}");
            foreach (var timesheet in timesheets)
            {
                stringBuilder.AppendLine($"{timesheet.Id}, {timesheet.TimesheetEntry.FirstName}, {timesheet.TimesheetEntry.LastName}, {timesheet.TimesheetEntry.Date}, {timesheet.TimesheetEntry.Project}, {timesheet.TimesheetEntry.Hours}, {timesheet.TotalHours}");
            }
            return stringBuilder.ToString();
        }
    }
}
