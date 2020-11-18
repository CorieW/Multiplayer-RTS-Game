using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAbility : Ability {

    public float speed;
    
    public MoveAbility(Unit unit, float speed) : base(unit) 
    {
        this.speed = speed;

        taskType = TaskType.Move;
    }

    public override void PerformTask(Task task)
    {
        MoveTask moveTask = (task as MoveTask);
        Vector3 targetPos = moveTask.GetTaskPosition();

        if(Vector2.Distance(unit.transform.position, targetPos) <= moveTask.GetTaskRange()) {
            unit.tasks.FinishTask();
            return;
        }

        unit.transform.position = Vector2.MoveTowards(unit.transform.position, targetPos, speed * Time.deltaTime);
        unit.transform.position = PositionCorrector.CorrectPosition(unit.transform.position);
    }

}