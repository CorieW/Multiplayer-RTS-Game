using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAbility : Ability {

    public float attackDamage;
    public float attackDelay;
    public float range;

    float _currentAttackDelay;

    public AttackAbility(Unit unit, float attackDamage, float attackDelay, float range) : base(unit) 
    {
        this.attackDamage = attackDamage;
        this.attackDelay = attackDelay;
        this.range = range;

        taskType = TaskType.Build;
    }

    public override void PerformTask(Task task)
    {
        // TODO
    }

}