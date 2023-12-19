using Microsoft.EntityFrameworkCore;
using Timesheets.Infrastructure;
using Timesheets.Models;

namespace Timesheets.Repositories
{
    public interface ITimesheetRepository
    {
        void AddTimesheet(Timesheet timesheet);
        IList<Timesheet> GetAllTimesheets();
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
            try
            {
                _context.Timesheets.Add(timesheet);

                _context.SaveChanges();
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
            }
           
        }

		public IList<Timesheet> GetAllTimesheets()
		{
			var timesheets = _context.Timesheets.Include(x => x.TimesheetEntry).ToList();

            timesheets = timesheets.OrderBy(timesheet => timesheet.TotalHours).ToList();


            return timesheets;
		}
	}
}
