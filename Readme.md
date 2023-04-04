# About
RoutinR makes keeping a timesheet as easy as pushing a button.
(after some initial configuration)

# Limits
- crossing timezones is not supported
- day light savings transitions are not supported (FBT/ SFT)
- start time is expected to be always smaller than end and both are expected to always be in the past
- violating this might crash the application

# Architecture
- Models used to descibe states are located in RoutinR.Core
- Services to provide base functionality are located in RoutinR.Services

- core models are being tested in RoutinR.Core.Tests
- service methods are being tested in RoutinR.Services.Tests

- Using Preferences to persist application settings see https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/preferences?view=net-maui-7.0&tabs=windows

# Maui Stuff
- using CommunityToolkit.Mvvm.ComponentModel to save boilerplate code:
	only define private lowercase fields inside ViewModel and decorate as ObservableProperty => this generates proper public PascalCase properties
	(but sometimes can lead to weird error outputs inside visual studio, despite builds running correctly, but restarting vs solves problems as always)

- dotnet workload install maui-android maui-ios maui-maccatalyst
- dotnet workload update

# Interesting Maui Links
- android permissions in manifest: https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-picker?view=net-maui-7.0&tabs=android
- https://github.com/jsuarezruiz/awesome-dotnet-maui
- https://learn.microsoft.com/en-us/dotnet/maui/user-interface/layouts/?view=net-maui-7.0
- https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation?view=net-maui-7.0
- https://learn.microsoft.com/en-us/dotnet/maui/xaml/fundamentals/mvvm?view=net-maui-7.0#commanding
- https://learn.microsoft.com/en-us/dotnet/architecture/maui/dependency-injection
- https://learn.microsoft.com/de-de/dotnet/maui/platform-integration/storage/preferences?view=net-maui-7.0&tabs=windows
- https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/markup/extensions/bindable-object-extensions
- https://www.youtube.com/watch?v=ts-gqSFFlvE&list=PLNfklfxfM6YiO7fA0fJNO4rrbFLVIk3af&index=1 (MAUI Samples Playlist)
- https://stackoverflow.com/questions/72598734/entity-framework-core-in-net-maui-xamarin-forms
- https://github.com/MicrosoftDocs/xamarin-docs/issues/481 (SQLite.NET PCL Vs Entity Framework Core)

# Interesting Testing Links
- https://medium.com/asos-techblog/testing-authorization-scenarios-in-asp-net-core-web-api-484bc95d5f6f 
- https://www.youtube.com/watch?v=GrJJXixjR8M (Implementing API Key Authentication in ASP.NET Core)
- https://www.youtube.com/watch?v=VuFQtyRmS0E How to unit test Minimal APIs in .NET 6 (and why it's hard)
- https://www.youtube.com/watch?v=7roqteWLw4s Integration testing | ASP.NET Core 5 REST API Tutorial 15


# Maui issues:
- crashes when scrollable content in swipeview on windows exceeds display: https://github.com/dotnet/maui/issues/8870
- Picker width on Windows not filling container: https://github.com/dotnet/maui/issues/6391
- AndroidManifest file does not exist: https://github.com/dotnet/maui/issues/1129
- Frame-Tag must have background color set to transparent

# Features
- Hit the Punch Clock Button to start tracking time
- a stopwatch like clock appears showing the total current running time
- current starting time is shown
- Hit the Punch Clock Button again to stop tracking time and log it
- previous start and end time are shown
- previous start time of a running session is being restored when application is closed and restarted
- use the default idle job to track time
- or add custom job names
- timesheets for all jobs can be viewed in a list
- current job for punch clock can be changed (set effectively on stopping, enabling toggling while already having started)
- saved job timesheets can be updated (change assigned job and/ or start/ end time)
- api export profiles can be added with individual templates for jobs to export to an external rest api
- the application db can be exported to a folder
- the application db can be imported and overridden from a selected file

# Installing
- use the msix file from github action for windows
- use the apk file from github action for android
-- before trying to upgrade you should export your database and reimport that after upgrading
-- upgrading does not really work
-- (something to do with the non signed package?)

# Features ToDo

(pending) - external apis can be linked by creating api export profiles containing post url, api user, api key
(pending) -- jobs can then be assigned an individual post template (i.e. json with placeholders for start and end time)

- huge delay when adding to dictionaryeditor?!? (can provoke showing by pressing a key inside another dictionary editors key editor)
- job picker is not preselected when leaving and reentering view (should we delete pending values on leave? yeah KISS!!)

- exporting creates an export log entry with date and reference to that timesheet entry
- export individual timesheet entries
- move db file info to settings
- add clear all settings button
- add clear database button
- pretty format time sheet entries

- (import) buttons change color on settings page after pressing?
- sometimes desktop (win) app does only react to moving and closing (window does not refresh, links/buttons are dead)
- form field validation
- wrap all (core or service calling) client code with exception handling

- overthink precision (ui allows only for minute precision, but backend uses second(??)-precision)
- change timespan format (seconds and/or ms should always be visible to indicate progress)

- testing TimeSheetEntry
- testing ApiExportProfile
- testing ExportService.ExportToApi extreme cases
- prevent adding post templates that dont contain placeholders (make readonly, add getter/adder?)
- visualize adjacent/ potentially overlapping time sheet entries when editing job time sheet entries
- consistent datetime formatters
- job timesheets can be filtered by jobname and time interval (also today/ this week/ this month filter for time sheet entries)
- enable merging adjacent time sheet entries if they share the same job
- Test "Equals"/"GetHashCode" ?
- ?allow apiprofile without job templates (in case jobs are created afterwards?)? => WHAT IF ALL JOBS GET DELETED that a profile references???
- is it dangerous to override Equals memorywise? should we use value types/ records/ structs?
- concurrency tests for dataservices

- gracefully fail in cases the app encounters something unexpected

# Misc
 Highlander-principle: there can only ever be one timer running
- starting tracking time for a task-item stops tracking time for all other task-items, 
- this is really only to make creating statistics of per day times easy
- so this MIGHT be changed in the future, then reworking how stats work :-)

7. 
(like times from the future, negative timespans, etc.)


- RoutinR is designed to help creating timesheet stats (where the sum of the items may not exceed 24 hours/day)
- A List/Set of work item types can be created
- Of these work item types 3 (?) may be selected to be favorite and visible from the home screen on mobile devices
- a global stop button is visible from the home screen
- play buttons next to those favorite work items start the stopwatch for that work item (and thus stopping any other stopwatch)
- therefore there may only ever be one stopwatch active