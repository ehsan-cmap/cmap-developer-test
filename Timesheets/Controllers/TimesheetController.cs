using Microsoft.AspNetCore.Mvc;
using SoftCircuits.CsvParser;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Globalization;
using Timesheets.Models;
using Timesheets.Services;

namespace Timesheets.Controllers
{
    public class TimesheetController : Controller
    {
        private ITimesheetService _timesheetService;

        public TimesheetController(ITimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(TimesheetEntry timesheetEntry)
        {
            var timesheet = new Timesheet()
            {
                TimesheetEntry = timesheetEntry,
                TotalHours = timesheetEntry.Hours
            };

            _timesheetService.Add(timesheet);

            ViewData["TimesheetViewer"] = _timesheetService.ViewTimeSheets();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult DownloadCsv()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (var writer = new CsvWriter(memoryStream))
            {
                var timesheetViewers = _timesheetService.ViewTimeSheets();

                writer.Write("Project", "First Name", "Last Name", "Date", "Total Hours");

                foreach (var timesheet in timesheetViewers)
                {
                        writer.Write($"{timesheet.Project}, {timesheet.TotalHours}, {timesheet.FirstName}, {timesheet.LastName}, {timesheet.Date}");
                }

                writer.Flush(); 

                memoryStream.Seek(0, SeekOrigin.Begin);
                byte[] fileContents = memoryStream.ToArray(); 

                // Specify the content type and file download name
                FileContentResult result = new FileContentResult(fileContents, "text/csv")
                {
                    FileDownloadName = "Filename.csv"
                };

                return result;
            }
        }

    }
}