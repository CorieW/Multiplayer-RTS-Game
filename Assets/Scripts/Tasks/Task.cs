using UnityEngine;

public class Task {
    public TaskType _type; //? When changing this to private I get an error.

    public TaskType GetTaskType() {
        return _type;
    }
}
public class MoveTask : Task {
    Vector2 _pos;
    float _range;

    public MoveTask(Vector2 pos, float range = 0) {
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
public class RepairTask : Task {
    Building _building;

    public RepairTask(Building building) {
        _building = building;
        _type = TaskType.Repair;
    }

    public Building GetTaskBuilding() {
        return _building;
    }
}
public class ResourceHarvestTask : Task {
    ResourceDeposit _resourceDepo;

    public ResourceHarvestTask(ResourceDeposit resourceDepo) {
        _resourceDepo = resourceDepo;
        _type = TaskType.Gather;
    }

    public ResourceDeposit GetTaskResourceDeposit() {
        return _resourceDepo;
    }
}
public class HaulTask : Task {
    Resource _resource;

    public HaulTask(Resource resource) {
        _resource = resource;
        _type = TaskType.Haul;
    }

    public Resource GetTaskResource() {
        return _resource;
    }
}
public class AttackTask : Task {
    PlayerObject _playerObj;

    public AttackTask(PlayerObject playerObj) {
        _playerObj = playerObj;
        _type = TaskType.Attack;
    }

    public PlayerObject GetTaskPlayerObject() {
        return _playerObj;
    }
}

public enum TaskType {
    None, Move, Build, Repair, Gather, Haul, Attack
}