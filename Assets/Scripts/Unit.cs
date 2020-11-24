using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AIPath), typeof(TaskList))]
public class Unit : PlayerObject
{
    [Header("Dependencies")]
    [SerializeField] private AIPath _aiPath;
    [SerializeField] private TaskList _taskList;

    [Header("Attributes")]
    [SerializeField] private bool _canBuild; // Todo: Implement this an the rest of the _can attributes
    [SerializeField] private bool _canAttack;
    [SerializeField] private bool _canResourceHarvest;
    [SerializeField] private bool _canHide;

    protected override void Start() 
    {
        base.Start();

        if (!_aiPath) _aiPath = GetComponent<AIPath>();
        if (!_taskList) _taskList = GetComponent<TaskList>();
    }

    public TaskList GetTaskList()
    {
        return _taskList;
    }

    public bool CanBuild()
    {
        return _canBuild;
    }

    public bool CanResourceHarvest()
    {
        return _canResourceHarvest;
    }

    public bool CanAttack()
    {
        return _canAttack;
    }

    #region Server

    [Command]
    private void CmdPerformMove(Vector3 pos)
    {
        // Moves the player on the server, therefore server authority logic can be applied.
        // However, may be susceptible to laggy movement for the client. To get past this, the player could also be moved client-side.
        _aiPath.destination = pos;
        _aiPath.SearchPath();
    }

    #endregion

    #region Client
    
    [ClientCallback]
    private void Update() 
    {
        PerformTasks();
    }

    [Client]
    public void PerformTasks()
    {
        foreach (Task task in _taskList.GetTasks())
        {
            switch (task.GetTaskType())
            {
                case TaskType.Move:
                    StartCoroutine(PerformMove(task as MoveTask));
                    return;
                case TaskType.Build: // Todo: Do rest of task functions
                    return;
                case TaskType.Gather:
                    return;
                case TaskType.Haul:
                    return;
                case TaskType.Attack:
                    return;
                default:
                    return;
            }
        }
    }

    [Client]
    private IEnumerator PerformMove(MoveTask moveTask)
    {
        CmdPerformMove(moveTask.GetTaskPosition());

        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (_aiPath.remainingDistance <= 0) yield break;
        }

    }

    #endregion
}
