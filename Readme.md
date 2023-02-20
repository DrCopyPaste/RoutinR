# About
RoutinR makes keeping a timesheet as easy as pushing a button.
(after some initial configuration)

# Limits
- crossing timezones is not supported
- day light savings transitions are not supported (FBT/ SFT)
- start time is expected to be always smaller than end and both are expected to always be in the past
- violating this might crash the application

# Architecture

- Models used to descibe states are located in RoutinR.Services
- Services to provide base functionality are located in RoutinR.Services

- core models are being tested in RoutinR.Core.Tests
- service methods are being tested in RoutinR.Services.Tests

- Using Preferences to persist application settings see https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/preferences?view=net-maui-7.0&tabs=windows

# Maui Stuff
- using CommunityToolkit.Mvvm.ComponentModel to save boilerplate code
- only define private lowercase fields inside VideModel and decorate as ObservableProperty => this generates proper public PascalCase properties
(but sometimes can lead to weird error outputs inside visual studio, despite builds running correctly, bur restarting vs solves problems as always)

# Interesting Maui Links
- https://learn.microsoft.com/en-us/dotnet/maui/xaml/fundamentals/mvvm?view=net-maui-7.0#commanding
- https://learn.microsoft.com/en-us/dotnet/architecture/maui/dependency-injection
- https://learn.microsoft.com/de-de/dotnet/maui/platform-integration/storage/preferences?view=net-maui-7.0&tabs=windows


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



# Features ToDo
- current job for punch clock can be changed (set effectively on stopping, enabling toggling while already having started)
- job timesheets can be filtered by jobname and time interval
- job timesheets can be updated (change assigned job and/ or start/ end time)

# ToDo

0. testing job time sheet entries (and with dataservice)
i. wrap all (core or service calling) client code with exception handling

ii. add unique jobs to a list
iii. persist logged times



done 1. Application can toggle tracking time and not tracking time
done 2. Starting and Stopping tracking time inserts those points in time to an internal collection (tuple<datetime, datetime> something)
3. The internal collection of start- and stop-times can be persisted/ exported (json, database, sth. else?)

4. Work-Items can be created
5. Work-Items consist of
- a name
- an internal collection of start- and stop-times
6. Highlander-principle: there can only ever be one timer running
- starting tracking time for a task-item stops tracking time for all other task-items, 
- this is really only to make creating statistics of per day times easy
- so this might be changed in the future, then reworking how stats work :-)

7. gracefully fail in cases the app encounters something unexpected
(like times from the future, negative timespans, etc.)


- RoutinR is designed to help creating timesheet stats (where the sum of the items may not exceed 24 hours/day)
- A List/Set of work item types can be created
- Of these work item types 3 (?) may be selected to be favorite and visible from the home screen on mobile devices
- a global stop button is visible from the home screen
- play buttons next to those favorite work items start the stopwatch for that work item (and thus stopping any other stopwatch)
- therefore there may only ever be one stopwatch active