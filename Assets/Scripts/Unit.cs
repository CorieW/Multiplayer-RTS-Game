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
    const float RESOURCE_GATHERING_DISTANCE = 0.5f;
    const float RESOURCE_HAULING_DISTANCE = 0.5f;

    [Header("Dependencies")]
    [SerializeField] private AIPath _aiPath;
    [SerializeField] private TaskList _taskList;

    [Header("Attributes")]
    [SerializeField] private float _buildSpeed; // Todo: Implement this an the rest of the _can attributes
    [SerializeField] private float _repairSpeed;
    
    [Space]

    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackDelay;

    [Space]

    [SerializeField] private float _resourceHarvestDamage;
    [SerializeField] private float _resourceHarvestDelay;

    [Space]

    [Tooltip("This is the position at which the hauling item will be positioned at when being hauled.")]
    [SerializeField] private Transform _holdTransform;
    [SerializeField] [SyncVar] private ResourceStack _hauling = null;

    [Space]

    [SerializeField] private bool _canHide;

    private float _currentAttackDelay;
    private float _currentResourceHarvestDelay;

    public bool isHauling { get { return _hauling == null; } }

    protected override void Awake() 
    {
        base.Awake();

        if (!_aiPath) _aiPath = GetComponent<AIPath>();
        if (!_taskList) _taskList = GetComponent<TaskList>();
    }

    public TaskList GetTaskList()
    {
        return _taskList;
    }

    public bool CanBuild()
    {
        return _buildSpeed > 0;
    }

    public bool CanRepair()
    {
        return _repairSpeed > 0;
    }

    public bool CanResourceHarvest()
    {
        return _resourceHarvestDamage > 0;
    }

    public bool CanHaul()
    {
        return _holdTransform;
    }

    public bool CanAttack()
    {
        return _attackDamage > 0;
    }

    public bool CanHide()
    {
        return _canHide;
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

    [Command] // Todo: Test
    private void CmdPerformHaul(ResourceStack resourceStack)
    {
        _hauling = resourceStack;
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
                    PerformMove(task as MoveTask);
                    return;
                case TaskType.Build: // Todo: Do rest of task functions
                    return;
                case TaskType.Repair:
                    return;
                case TaskType.ResourceHarvest:
                    PerformResourceHarvest(task as ResourceHarvestTask);
                    return;
                case TaskType.Haul:
                    PerformHaul(task as HaulTask);
                    return;
                case TaskType.Attack:
                    return;
                default:
                    return;
            }
        }
    }

    [Client]
    private void PerformMove(MoveTask moveTask)
    {
        // The ai is set to go to the task's target position.
        if (_aiPath.destination != moveTask.GetTaskPosition()) CmdPerformMove(moveTask.GetTaskPosition());

        if (_aiPath.reachedDestination) {
            _taskList.FinishTask();
        }

    }

    [Client]
    private void PerformResourceHarvest(ResourceHarvestTask resourceHarvestTask)
    {
        ResourceDeposit resDepo = resourceHarvestTask.GetTaskResourceDeposit();
        if (resDepo == null) 
        { // The resource deposit no longer exists, finish the task.
            _taskList.FinishTask();
            return;
        }

        if (Vector2.Distance(transform.position, resDepo.transform.position) > RESOURCE_GATHERING_DISTANCE)
        { // Unit is not within range of gathering resource - set move task to get closer to it.
           _taskList.AssignPriorityTask(
                new MoveTask(resDepo.transform.position - new Vector3(0, RESOURCE_GATHERING_DISTANCE))
            );
            return;
        }

        // Unit is within range of gathering resource.
        // Reduce the time till the unit next harvests the resource.
        _currentResourceHarvestDelay -= Time.deltaTime;
        if (_currentResourceHarvestDelay > 0) return;

        // Gathers the resource. Does damage to the resource, getting it closer to dropping a resource drop.
        resDepo.CmdGatherResource(_resourceHarvestDamage);
        // Assign a delay to stop instantanious gathering of resource.
        _currentResourceHarvestDelay = _resourceHarvestDelay;
    }

    [Client]
    private void PerformHaul(HaulTask haulTask)
    {
        // ! What if I am already hauling?

        ResourceDrop resDrop = haulTask.GetTaskResource();

        if (!resDrop) 
        { // Resource drop is no longer avaliable, indicating it's been picked up since.
            _taskList.FinishTask();
            return;
        }

        if (Vector2.Distance(transform.position, resDrop.transform.position) > RESOURCE_HAULING_DISTANCE)
        { // Unit is not within range of hauling resource drop - set move task to get closer to it.
           _taskList.AssignPriorityTask(
                new MoveTask(resDrop.transform.position - new Vector3(0, RESOURCE_HAULING_DISTANCE))
            );
            return;
        }

        // Todo: Test 
        CmdPerformHaul(resDrop.resourceStack);
        resDrop.CmdHaulResource();
    }

    #endregion
}
