using UnityEngine;

public class Task {
    protected TaskType _type;

    public TaskType GetTaskType() {
        return _type;
    }
}
public class MoveTask : Task {
    Vector2 _pos;
    float _range;

    public MoveTask(Vector2 pos) {
        _pos = pos;
        _type = TaskType.Move;
    }

    public MoveTask(Vector2 pos, float range) {
        _pos = pos;
        _range = range;
        _type = TaskType.Move;
    }

    public Vector2 GetTaskPosition() {
        return _pos;
    }

    public float GetTaskRange() 
    {
        return _range;
    }
}
public class BuildTask : Task {
    Building _building;

    public BuildTask(Building building) {
        _building = building;
        _type = TaskType.Build;
    }

    public Building GetTaskBuilding() {
        return _building;
    }
}
public class GatherTask : Task {
    ResourceDeposit _resourceDepo;

    public GatherTask(ResourceDeposit resourceDepo) {
        _resourceDepo = resourceDepo;
        _type = TaskType.Gather;
    }

    public ResourceDeposit GetTaskResourceDeposit() {
        return _resourceDepo;
    }
}
public class AttackTask : Task {
    RTSObject _obj;

    public AttackTask(RTSObject obj) {
        _obj = obj;
        _type = TaskType.Attack;
    }

    public RTSObject GetTaskObject() {
        return _obj;
    }
}

public enum TaskType {
    Move, Build, Gather, Attack
}