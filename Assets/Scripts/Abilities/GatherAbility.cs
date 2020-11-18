using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherAbility : Ability {

    public int gatherYield;
    public float gatherDelay;
    public float range;

    float _currentGatherDelay;

    public GatherAbility(Unit unit, int gatherYield, float gatherDelay, float range) : base(unit) 
    {
        this.gatherYield = gatherYield;
        this.gatherDelay = gatherDelay;
        this.range = range;

        taskType = TaskType.Gather;
    }

    public override void PerformTask(Task task)
    {
        ResourceDeposit resourceDepo = (task as GatherTask).GetTaskResourceDeposit();

        if (!resourceDepo) {
            _currentGatherDelay = 0;
            unit.tasks.FinishTask();
            return;
        }

        if (Vector2.Distance(unit.transform.position, resourceDepo.transform.position) > range)
        { // Get within range of resource, so assign move task to get there as a priority
            unit.tasks.AssignPriorityTask(new MoveTask(resourceDepo.transform.position, range));
            return;
        }

        if(_currentGatherDelay > 0) 
        { // Gather delay is active
            _currentGatherDelay -= Time.deltaTime;
            return;
        }

        unit.GetOwner().GetResourceCollection().CmdAddResources(resourceDepo.Gather(gatherYield));
        _currentGatherDelay = gatherDelay;
    }

}