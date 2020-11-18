using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskList {
    List<Task> _tasks = new List<Task>();

    public List<Task> GetTasks() 
    {
        return _tasks;
    }

    public Task GetTask() 
    {
        if(_tasks.Count == 0) return null;

        return _tasks[0];
    }

    public void FinishTask() 
    { // Finishes the most recent task
        _tasks.RemoveAt(0);
    }

    public void SetTask(Task task) 
    { // Sets a task
        _tasks.Clear();
        _tasks.Add(task);
    }

    public void AssignTask(Task task) 
    { // Assigns a task on top of the already existing tasks
        _tasks.Add(task);
    }

    public void AssignPriorityTask(Task task) 
    { // Assigns a task to complete before every other task
        _tasks.Insert(0, task);
    }
}