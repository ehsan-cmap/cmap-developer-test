Models

In the models you could use double or decimal instead of strings for the TotalHours in Timesheet and Hours in TimesheetEntry

You could change Date in TimesheetEntry to DateTime for better date handling.

Controller

- In the Index POST method validate TimesheetEntry using data annotations and ModelState.IsValid
If it is not in use then you could remove unnecessary GetAll call if the result isn't used.

Test

If the namespaces are not used, remove unnecessary namespaces.
Could have more testing scenarios for null, duplicates IDs, and return values.
Test Service and Repository layers separately for secure data insertion.
Use specific instances of Timesheet for accurate testing.