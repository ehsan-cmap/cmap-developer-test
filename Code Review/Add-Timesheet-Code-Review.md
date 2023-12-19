


Controller: 
1) Validation for the timesheet service is missing. Take the TimesheetEntry object as an example. There could be data annotations on the object. and the the instansiatiating the object. Can it be nullable? Prehaps we could have a ModelState.IsValid in here. 

2) After getting al the timesheets. The call is not used. 


Models: 
1) Timesheet 
TotalHours should be a string? If it's going to be a decimal. Concider using a double, decimal or timespan.
2) TimesheetEntry
Both date and hours are strings. Changing this to a double or a timespan could offer better validation.

Service: 
1) Consider using some validation when adding a timesheet to avoid database errors. We can do this using a try catch. 
2) To make the code easier to use and to follow conventions. We can use xml documentation. You can see some examples here. 
https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
3) In the future. We could clone the list GetAll timesheets or use a readonly collection to prevent external modification. But this works well for now. 

TimesheetRepository: 
1) If I had to nitpick. I suppose we could have log statements in here to help with debugging and the maintanance.


Test: 
1) Unnessesary namespaces can be removed
2) Both the service and the respository need to be tested seperately. 
3) There is no tear down or Set up annotations
4) We could check the state of the timesheet after it's added, or verify that the repository method is called with the correct parameter. There are a few more aspects like this we could also test against. 

General Observations: 
1) We could implement more consise operations with the timesheet service moving forward. Getting all timesheets will in time be very costly on performance. 

2) Introducing a ViewModel will allow data that is not present in the Timesheet or TimesheetEntry models to be perscribed. It also offers seperation of concerns with your data entities and the data that is presented on the views. 