using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

public class Unit : PlayerObject
{
    [Header("Dependencies")]
    [SerializeField]
    private AIPath aiPath;

    [Header("Attributes")]
    [SerializeField] private TaskList tasks = new TaskList();

    public TaskList GetTasks()
    {
        return tasks;
    }

    #region Server

    [Command]
    public void CmdMove(Vector3 destination) 
    {
        // Moves the player on the server, therefore server authority logic can be applied.
        // However, may be susceptible to laggy movement for the client. To get past this, the player could also be moved client-side.
        destination = new Vector3(destination.x, destination.y, 0);
        aiPath.destination = destination;
        aiPath.SearchPath();
    }

    #endregion

    #region Client
    
    #endregion
}
