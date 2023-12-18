using Castle.Core.Configuration;
using Microsoft.AspNetCore.Identity;
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
            timesheetService.Add(timesheet);

            // Assert
            mockRepository.Verify(repo => repo.AddTimesheet(It.IsAny<Timesheet>()), Times.Once);
        }

        [Fact]
        public void GivenNoTimesheets_WhenViewTimeSheets_ThenReturnEmptyList()
        {
            var mockRepository = new Mock<ITimesheetRepository>();
            mockRepository.Setup(repo => repo.ViewTimeSheets()).Returns(new List<TimesheetViewer>());

            var timesheetService = new TimesheetService(mockRepository.Object);

            var result = timesheetService.ViewTimeSheets();

            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GivenTimesheets_WhenViewTimeSheets_ThenReturnListWithCorrectData()
        {
            var timesheets = new List<TimesheetViewer>
        {
            new TimesheetViewer
            {

                    Project = "ProjectA",
                    FirstName = "John",
                    LastName = "Doe",
                    TotalHours = 5

            },
            new TimesheetViewer
            {
                
                    Project = "ProjectB",
                    FirstName = "Jane",
                    LastName = "Smith",
                    TotalHours = 7.5

            }
        };

            var mockRepository = new Mock<ITimesheetRepository>();
            mockRepository.Setup(repo => repo.ViewTimeSheets()).Returns(timesheets);

            var timesheetService = new TimesheetService(mockRepository.Object);

            var result = timesheetService.ViewTimeSheets();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("ProjectA", result[0].Project);
            Assert.Equal("John", result[0].FirstName);
            Assert.Equal("Doe", result[0].LastName);
            Assert.Equal(5.0, result[0].TotalHours);
        }

    }
}
