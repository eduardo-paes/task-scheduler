using System;
using Microsoft.Win32.TaskScheduler;

class Program
{
  static void Main(string[] args)
  {
    Create_Task("Task Name", 1, new DateTime(2000, 01, 01, 12, 00, 00));
    Delete_Task("Task Name");
  }
   
  private void Delete_Task(string taskName)
  {
      try
      {
          // Get the service on the local machine
          using (TaskService ts = new TaskService())
          {
              // Create a new task definition and assign properties
              TaskDefinition td = ts.NewTask();

              // Remove the task of name 'taskName'
              ts.RootFolder.DeleteTask(taskName);
          }
      }
      catch (Exception)
      {
          // If there isn't any task for the name
      }
  }

  private void Create_Task(string taskName, short daysInterval, DateTime time)
  {
      var scriptPath = @"script.exe";
      var description = "Description of task here.";

      // Get the service on the local machine
      using (TaskService ts = new TaskService())
      {
          // Create a new task definition and assign properties
          TaskDefinition td = ts.NewTask();
          td.RegistrationInfo.Description = description;

          var today = DateTime.Now;
          var specTime = new DateTime(today.Year, today.Month, today.Day, time.Hour, time.Minute, time.Second);

          // Create a trigger that will fire the task at this time every other day
          td.Triggers.Add(new DailyTrigger { 
              DaysInterval = daysInterval,
              StartBoundary = specTime
          });

          // Create an action that will launch Notepad whenever the trigger fires
          td.Actions.Add(new ExecAction(scriptPath, null, null));

          // Register the task in the root folder
          ts.RootFolder.RegisterTaskDefinition(taskName, td);
      }
  }
}
