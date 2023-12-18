using Microsoft.EntityFrameworkCore;
using Timesheets.Infrastructure;
using Timesheets.Models;

namespace Timesheets.Repositories
{
    public interface ITimesheetRepository
    {
        void AddTimesheet(Timesheet timesheet);
        IList<Timesheet> GetAllTimesheets();
        IList<TimesheetViewer> ViewTimeSheets();
    }

    public class TimesheetRepository : ITimesheetRepository
    {
        private DataContext _context;

        public TimesheetRepository(DataContext context)
        {
            _context = context;
        }
        public void AddTimesheet(Timesheet timesheet)
        {
            _context.Timesheets.Add(timesheet);
            _context.SaveChanges();
        }

        public IList<Timesheet> GetAllTimesheets()
        {
            var timesheets = _context.Timesheets.ToList();
            return timesheets;
        }
        public IList<TimesheetViewer> ViewTimeSheets()
        {
            var timesheetData = _context.Timesheets
                .Include(t => t.TimesheetEntry)
                .ToList();

            var result = _context.Timesheets
                .Include(t => t.TimesheetEntry)
                .GroupBy(te => new
                {
                    te.TimesheetEntry.Date,
                    te.TimesheetEntry.Project,
                    te.TimesheetEntry.FirstName,
                    te.TimesheetEntry.LastName
                })
                .Select(group => new TimesheetViewer
                {
                    Date = group.Key.Date,
                    Project = group.Key.Project,
                    FirstName = group.Key.FirstName,
                    LastName = group.Key.LastName,
                    TotalHours = group.Sum(te => Convert.ToDouble(te.TimesheetEntry.Hours))
                })
                .ToList();

            result = result
                .OrderByDescending(te => te.TotalHours)
                .ToList();

            return result;
        }
    }
}
