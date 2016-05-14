using System;

public class TaskS
{
	public TaskS(String name, String path, DateTime dateExec, String description)
	{
        TaskScheduler ts = new TaskScheduler();
        using (TaskService ts = new TaskService())
        {
            // Create a new task definition and assign properties
            TaskDefinition td = ts.NewTask();
            td.RegistrationInfo.Description = "Does something";

            // Create a trigger that will fire the task at this time every other day
            td.Triggers.Add(new DailyTrigger { DaysInterval = 2 });

            // Create an action that will launch Notepad whenever the trigger fires
            td.Actions.Add(new ExecAction("notepad.exe", "c:\\test.log", null));

            // Register the task in the root folder
            ts.RootFolder.RegisterTaskDefinition(@"Test", td);

            // Remove the task we just created
            ts.RootFolder.DeleteTask("Test");
        }
	}


}
