using Castle.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Timesheets.Infrastructure;
using Timesheets.Models;
using Timesheets.Repositories;
using Timesheets.Services;

namespace Timesheets.Test
{
	public class TimesheetTests
	{

		private readonly TimesheetService _timesheetService;
		private readonly Mock<ITimesheetRepository> _mockRepository;

		public TimesheetTests()
		{
			_mockRepository = new Mock<ITimesheetRepository>();
			_timesheetService = new TimesheetService(_mockRepository.Object);
		}

		[Fact]
		public void GivenAValidTimesheet_ThenAddTimesheetToInMemoryDatabase()
		{
			//Arrange
			var timesheet = new Timesheet();
			var timesheetEntry = new TimesheetEntry()
			{
				Id = 1,
				Date = "01/09/2023",
				Project = "Test Project",
				FirstName = "Test",
				LastName = "Test",
				Hours = "7.5"
			};
			timesheet.Id = 1;
			timesheet.TimesheetEntry = timesheetEntry;
			timesheet.TotalHours = timesheetEntry.Hours;

			var mockRepository = new Mock<ITimesheetRepository>();
			var timesheetService = new TimesheetService(mockRepository.Object);

			// Act
			_timesheetService.Add(timesheet);

			// Assert
			_mockRepository.Verify(repo => repo.AddTimesheet(It.IsAny<Timesheet>()), Times.Once);
		}


		[Fact]
		public void GivenFetchAllTimesheets_ReturnAllTimeSheetsFromDatabase()
		{
			// Arrange
			//Creates Mock Data
			var RandomHours = new Random();

			var timesheetsList = Enumerable.Range(1, 10).Select(i => new Timesheet
			{
				Id = i,
				TimesheetEntry = new TimesheetEntry
				{
					Id = i,
					Project = $"Project {i}",
					FirstName = "Louis",
					LastName = "Thompson",
					Hours = RandomHours.Next(1, 9).ToString("0.0")
				},

			}).ToList();

			foreach (var timesheet in timesheetsList)
			{
				_timesheetService.Add(timesheet);
			}

			Console.WriteLine(timesheetsList.Count);

			//Sets up all timesheets from a Mock TimesheetRepository 
			_mockRepository.Setup(repo => repo.GetAllTimesheets()).Returns(timesheetsList);

			// Act
			var result = _timesheetService.GetAll();
			Console.WriteLine(result);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(10, result.Count);
		}


		[Fact]
		public void GetAll_ReturnsTimesheetsInSizeOrder()
		{
			// Arrange
			//Creates Mock Data
			var RandomHours = new Random();

			var timesheetsList = Enumerable.Range(1, 10).Select(i => new Timesheet
			{
				Id = i,
				TimesheetEntry = new TimesheetEntry
				{
					Id = i,
					Project = $"Project {i}",
					FirstName = "Louis",
					LastName = "Thompson",
					Hours = RandomHours.Next(1, 9).ToString("0.0")
				},

			}).ToList();

			foreach (var timesheet in timesheetsList)
			{
				_timesheetService.Add(timesheet);
			}

			_mockRepository.Setup(repo => repo.GetAllTimesheets()).Returns(timesheetsList);

			// Act

			var timesheets = _timesheetService.GetAll();


			// Assert
			Assert.True(timesheets.OrderBy(timesheet => timesheet.TotalHours).SequenceEqual(timesheets));
		}



		[Fact]
		public void GivenAListOfTimesheets_ReturnStringInCSVFormat()
		{
			var csvService = new CSVExportServicecs();
			var timesheets = new List<Timesheet>()
		{ new Timesheet{ TimesheetEntry = new TimesheetEntry { Date ="19/12/2023", FirstName="Louis", Hours = "7.5", LastName ="Thompson", Project ="Test Project" }, TotalHours = "7.5" } };

			var csv = csvService.CreateCSV(timesheets);

			Assert.Equal("Id, FirstName, LastName, Date, Project, Hours, TotalHours\r\n0, first, last, 01/01/2001, proj, 8, 8\r\n", csv);
		}

		[Fact]
		public void GivenAnEmptyListOfTimesheets_RetrunAnEmptyString()
		{
			var csvService = new CSVExportServicecs();
			var timesheets = new List<Timesheet>();
			var csv = csvService.CreateCSV(timesheets);
			Assert.Equal(string.Empty, csv);
		}
	}
}
