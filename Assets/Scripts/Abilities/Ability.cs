using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability {
    public Unit unit;
    public TaskType taskType;

    public Ability(Unit unit) {
        this.unit = unit;
    }

    public abstract void PerformTask(Task task);

}