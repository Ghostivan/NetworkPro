﻿using System;
using TaskScheduler;

public class TaskS
{
	public TaskS(String name, String path, String prmtrs, DateTime dateExec, String description)
	{
        if (dateExec < DateTime.Now)
            throw new SystemException("Date non valide : Date anterieur à la date actuelle");
        TaskScheduler.TaskScheduler scheduler = new TaskScheduler.TaskScheduler();
        scheduler.Connect(null, null, null, null); //run as current user.

        ITaskDefinition taskDef = scheduler.NewTask(0);
        taskDef.RegistrationInfo.Author = "Admin";
        taskDef.RegistrationInfo.Description = description;
        //taskDef.Settings.ExecutionTimeLimit = "PT10M"; // 10 minutes
        taskDef.Settings.DisallowStartIfOnBatteries = false;
        taskDef.Settings.StopIfGoingOnBatteries = false;
        taskDef.Settings.WakeToRun = true;

        ITimeTrigger trigger = (ITimeTrigger)taskDef.Triggers.Create(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_TIME);

        DateTime nextRun = dateExec;//DateTime nextRun = DateTime.Now.AddMinutes(2); // one minute from now
        trigger.StartBoundary = nextRun.ToString("s", System.Globalization.CultureInfo.InvariantCulture);

        IExecAction action = (IExecAction)taskDef.Actions.Create(_TASK_ACTION_TYPE.TASK_ACTION_EXEC);
        action.Id = name;
        action.Path = path;
        //action.WorkingDirectory = "working dir";
        action.Arguments = prmtrs;

        ITaskFolder root = scheduler.GetFolder("\\");

        IRegisteredTask regTask = root.RegisterTaskDefinition(
            name,
            taskDef,
            (int)_TASK_CREATION.TASK_CREATE_OR_UPDATE,
            null, // user
            null, // password
            _TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN, //User must already be logged on. The task will be run only in an existing interactive session.
            "" //SDDL
            );
	}

    public void listTask()
    {
        TaskScheduler.TaskScheduler scheduler = new TaskScheduler.TaskScheduler();
        scheduler.Connect(null, null, null, null);
        IRegisteredTaskCollection tsks = scheduler.GetFolder("\\").GetTasks(0);
        
        foreach (IRegisteredTask tsk in tsks)
            Console.WriteLine(tsk.Name);

    }
}
