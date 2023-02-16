// See https://aka.ms/new-console-template for more information
using PunchClock.Core;

Console.WriteLine("Hello, World!");

var maEntry = new TimeSheetEntry();

Console.WriteLine(maEntry.StartTime);

Console.WriteLine($"is running: {maEntry.IsRunning}");


var task = Task.Run(() => { Console.WriteLine($"is running in task: {maEntry.IsRunning}"); });


maEntry.Stop();

Console.WriteLine($"end time: {maEntry.EndTime}");

Console.WriteLine($"is running: {maEntry.IsRunning}");