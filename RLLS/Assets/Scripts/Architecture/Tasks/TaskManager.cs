using System.Collections.Generic;
using UnityEngine;

public class TaskManager
{
    private readonly List<Task> tasks = new List<Task>();


    public void AddTask(Task task)
    {
        tasks.Add(task);
        task.SetStatus(Task.TaskStatus.Pending);
    }


    public void Tick()
    {
        for (int i = tasks.Count - 1; i >= 0; --i)
        {
            Task task = tasks[i];

            if (task.IsPending) { task.SetStatus(Task.TaskStatus.Working); }

            if (task.IsFinished)
            {
                HandleCompletion(task, i);
            }
            else
            {
                task.Tick();

                if (task.IsFinished)
                {
                    HandleCompletion(task, i);
                }
            }
        }
    }

    /// <summary>
    /// If the task succeeded, and is meant to be followed by another task, start that next task.
    /// 
    /// No matter what, remove the completed task from the list of tasks to run, and tell the task that it has detached.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="taskIndex"></param>
    private void HandleCompletion(Task task, int taskIndex)
    {
        if (task.NextTask != null && task.IsSuccessful) { AddTask(task.NextTask); }

        tasks.RemoveAt(taskIndex);
        task.SetStatus(Task.TaskStatus.Detached);
    }
}
