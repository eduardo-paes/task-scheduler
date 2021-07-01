using System;
using Microsoft.Win32.TaskScheduler;

class Program
{
  static void Main(string[] args)
  {
    Create_Task("VNU", 1);
    Delete_Task("VNU");
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

  private void Create_Task(string taskName, short daysInterval)
  {
      var scriptPath = @"caminho do executável aqui";
      var description = "Execução planejada de script para tratamento de VNUs.";

      // Get the service on the local machine
      using (TaskService ts = new TaskService())
      {
          // Create a new task definition and assign properties
          TaskDefinition td = ts.NewTask();
          td.RegistrationInfo.Description = description;

          var today = DateTime.Now;
          var specTime = new DateTime(today.Year, today.Month, today.Day, 12, 00, 00);

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
