using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildAbility : Ability {

    public float buildSpeed;
    public float range;

    float _currentBuildDelay;

    public BuildAbility(Unit unit, float buildSpeed, float range) : base(unit) 
    {
        this.buildSpeed = buildSpeed;
        this.range = range;

        taskType = TaskType.Build;
    }

    public override void PerformTask(Task task)
    {
        // TODO
    }

}