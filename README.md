# MyBudget

This project is meant to be a personal budget tool application. It is monolith by design and uses a local SQLite database.
The user imports data via excel or .csv (currently bank statements from my particular bank). The home page shows income 
levels vs expenses in various graphs, as well as a breakdown of expenses by category. I use MudBlazor components
wherever I can, and develop my own otherwise.

Of particular note is the calendar feature and the excel import feature.
Calendar component page: MyBudget\MyBudget\Pages\CalendarPage.razor
Calendar component .cs:  MyBudget\MyBudget\Pages\CalendarPage.razor.cs

Excel Import Component page: MyBudget\MyBudget\Pages\ExcelImport.razor
Excel Import Component .cs:  MyBudget\MyBudget\Pages\ExcelImport.razor.cs

I also enjoyed learning how to dynamically create database tables, given the database exists:
MyBudget\MyBudget\Helpers\DatabaseHelper.cs
