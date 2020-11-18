using System.Collections;
using System.Collections.Generic;
using Mirror;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

public class Unit : PlayerObject
{
    [Header("Dependencies")]
    [SerializeField] private AIPath _aiPath;
    public TaskList tasks { get; } = new TaskList();
    [SerializeField] private List<Ability> abilities;

    [Header("Events")]
    [SerializeField] private UnityEvent _onSelected = null;
    [SerializeField] private UnityEvent _onDeselected = null;

    public Ability GetAbility(Task task) 
    {
        foreach(Ability ability in abilities) {
            if(task.GetTaskType() == ability.taskType) {
                return ability;
            }
        }

        return null;
    }

    public bool HasAbility(Task task) 
    {
        foreach(Ability ability in abilities) {
            if(task.GetTaskType() == ability.taskType) {
                return true;
            }
        }

        return false;
    }

    #region Server

    [Command]
    private void CmdMove(Vector3 destination) 
    {
        // Moves the player on the server, therefore server authority logic can be applied.
        // However, may be susceptible to laggy movement for the client. To get past this, the player could also be moved client-side.
        _aiPath.destination = destination;
        _aiPath.SearchPath();
    }

    #endregion

    #region Client

    [Client]
    protected virtual void PerformTasks() 
    {
        Task task = tasks.GetTask();
        if(task == null) return;

        Ability ability = GetAbility(task);

        if(ability != null) {
            ability.PerformTask(task);
        }
        else {
            tasks.FinishTask();
        }
    }

    [Client]
    public void Move(Vector3 destination)
    {
        CmdMove(destination);
    }

    [Client]
    public void Gather(ResourceDeposit resourceDeposit) // TODO: (Some units should be incapable)
    {

    }

    [Client]
    public void Pickup(Resource resource) // TODO: (Some units should be incapable)
    {

    }

    [Client]
    public void Attack(Unit unit) // TODO: Change param input to RTSObject (Some units should be incapable)
    {

    }

    // TODO: Hide method that has a parameter input of an RTSObject (Some units should be incapable)

    // TODO: HideAtNearestObject method - Finds the closest object to hide at that someone isn't already hiding at (Some units should be incapable)

    // TODO: Destroy method that has a parameter input of an RTSObject

    // TODO: TargetAt method that has a parameter input of a Vector3

    [Client]
    public void Select()
    {
        if (!hasAuthority) return;

        _onSelected?.Invoke();
    }

    [Client]
    public void Deselect()
    {
        if (!hasAuthority) return;

        _onDeselected?.Invoke();
    }

    #endregion
}
