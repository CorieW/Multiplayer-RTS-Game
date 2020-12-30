using UnityEngine;

public class Task {
    public TaskType _taskType; //? When changing this to private I get an error.
    
    public TaskType taskType { get { return _taskType; } }
}
public class MoveTask : Task {
    Vector3 _pos;
    float _range;

    public Vector3 pos { get { return _pos; } }
    public float range { get { return _range; } }

    public MoveTask(Vector3 pos, float range = 0) {
        _pos = pos;
        _range = range;
        _taskType = TaskType.Move;
    }
}
public class BuildTask : Task {
    Building _building;

    public Building building { get { return _building; } }

    public BuildTask(Building building) {
        _building = building;
        _taskType = TaskType.Build;
    }
}
public class RepairTask : Task {
    Building _building;

    public Building building { get { return _building; } }

    public RepairTask(Building building) {
        _building = building;
        _taskType = TaskType.Repair;
    }
}
public class ResourceHarvestTask : Task {
    ResourceDeposit _resourceDeposit;

    public ResourceDeposit resourceDeposit { get { return _resourceDeposit; } }

    public ResourceHarvestTask(ResourceDeposit resourceDeposit) {
        _resourceDeposit = resourceDeposit;
        _taskType = TaskType.ResourceHarvest;
    }
}
public class HaulTask : Task {
    ResourceDrop _resourceDrop;

    public ResourceDrop resourceDrop { get { return _resourceDrop; } }

    public HaulTask(ResourceDrop resourceDrop) {
        _resourceDrop = resourceDrop;
        _taskType = TaskType.Haul;
    }
}
public class StoreTask : Task {
    Stockpile _stockpile;

    public Stockpile stockpile { get { return _stockpile; } }

    public StoreTask(Stockpile stockpile) {
        _stockpile = stockpile;
        _taskType = TaskType.Store;
    }
}
public class AttackTask : Task {
    PlayerEntity _playerEntity;

    public PlayerEntity playerEntity { get { return _playerEntity; } }

    public AttackTask(PlayerEntity playerEntity) {
        _playerEntity = playerEntity;
        _taskType = TaskType.Attack;
    }
}

public enum TaskType {
    None, Move, Build, Repair, ResourceHarvest, Haul, Store, Attack
}