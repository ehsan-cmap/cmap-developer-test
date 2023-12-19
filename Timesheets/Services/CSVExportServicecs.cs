using System.Text;
using Timesheets.Models;

namespace Timesheets.Services
{
    public interface ICsvService
    {
        string CreateCSV(IList<Timesheet> timesheets);
    }

    public class CSVExportServicecs : ICsvService
    {
        public string CreateCSV(IList<Timesheet> timesheets)
        {
       
            return "";
        }
    }
}
