using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] private ObjectSelectionHandler _objectSelectionHandler = null;
    [SerializeField] private LayerMask _layerMask = new LayerMask();

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) return;

        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (Physics.Raycast(mousePos, Vector3.forward, out RaycastHit hit, Mathf.Infinity, _layerMask))
        {
            ResourceDeposit resourceDeposit = hit.collider.GetComponent<ResourceDeposit>();
            ResourceDrop resourceDrop = hit.collider.GetComponent<ResourceDrop>();

            if (resourceDeposit) GiveResourceHarvestOrder(resourceDeposit, Keyboard.current.leftShiftKey.isPressed ? OrderType.Assign : OrderType.Set);
            else if (resourceDrop) GiveHaulOrder(resourceDrop, Keyboard.current.leftShiftKey.isPressed ? OrderType.Assign : OrderType.Set);
        }
        else
        {
            GiveMoveOrder(mousePos, Keyboard.current.leftShiftKey.isPressed ? OrderType.Assign : OrderType.Set);
            return;
        }
    }

    // pos is the position that the unit should move to. 
    // orderType indicates whether the user wants to add that task to the current tasks or make that the only task the unit should perform.
    private void GiveMoveOrder(Vector3 pos, OrderType orderType)
    {
        foreach(PlayerObject selectedPlayerObj in _objectSelectionHandler.selectedPlayerObjects)
        {
            // The selected player object is not a unit, so it can't be given a move order - return.
            if (!(selectedPlayerObj is Unit)) continue;

            // The selected player object is a unit, give it a move order.
            Unit unit = selectedPlayerObj as Unit;

            if (orderType == OrderType.Set) unit.GetTaskList().SetTask(new MoveTask(pos));
            else unit.GetTaskList().AssignTask(new MoveTask(pos));
        }
    }

    private void GiveBuildOrder(Building building, OrderType orderType)
    {
        foreach(PlayerObject selectedPlayerObj in _objectSelectionHandler.selectedPlayerObjects)
        {
            if (!(selectedPlayerObj is Unit)) continue;
            
            Unit unit = selectedPlayerObj as Unit;
            // The selected unit can't build - continue.
            if (!unit.CanBuild()) continue;

            if (orderType == OrderType.Set) unit.GetTaskList().SetTask(new BuildTask(building));
            else unit.GetTaskList().AssignTask(new BuildTask(building));
        }
    }

    private void GiveRepairOrder(Building building, OrderType orderType)
    {
        foreach(PlayerObject selectedPlayerObj in _objectSelectionHandler.selectedPlayerObjects)
        {
            if (!(selectedPlayerObj is Unit)) continue;
            
            Unit unit = selectedPlayerObj as Unit;
            if (!unit.CanRepair()) continue;

            if (orderType == OrderType.Set) unit.GetTaskList().SetTask(new RepairTask(building));
            else unit.GetTaskList().AssignTask(new RepairTask(building));
        }
    }

    private void GiveResourceHarvestOrder(ResourceDeposit resourceDepo, OrderType orderType)
    {
        foreach(PlayerObject selectedPlayerObj in _objectSelectionHandler.selectedPlayerObjects)
        {
            if (!(selectedPlayerObj is Unit)) continue;
            
            Unit unit = selectedPlayerObj as Unit;
            if (!unit.CanResourceHarvest()) continue;

            if (orderType == OrderType.Set) unit.GetTaskList().SetTask(new ResourceHarvestTask(resourceDepo));
            else unit.GetTaskList().AssignTask(new ResourceHarvestTask(resourceDepo));
        }
    }

    private void GiveHaulOrder(ResourceDrop resourceDrop, OrderType orderType)
    {
        foreach(PlayerObject selectedPlayerObj in _objectSelectionHandler.selectedPlayerObjects)
        {
            if (!(selectedPlayerObj is Unit)) continue;
            
            Unit unit = selectedPlayerObj as Unit;
            if (!unit.CanHaul()) continue; // Todo: Replace with CanHaul

            if (orderType == OrderType.Set) unit.GetTaskList().SetTask(new HaulTask(resourceDrop));
            else unit.GetTaskList().AssignTask(new HaulTask(resourceDrop));
        }
    }

    private void GiveAttackOrder(PlayerObject playerObj, OrderType orderType)
    {
        foreach(PlayerObject selectedPlayerObj in _objectSelectionHandler.selectedPlayerObjects)
        {
            if (!(selectedPlayerObj is Unit)) continue;
            
            Unit unit = selectedPlayerObj as Unit;
            if (!unit.CanAttack()) continue;

            if (orderType == OrderType.Set) unit.GetTaskList().SetTask(new AttackTask(playerObj));
            else unit.GetTaskList().AssignTask(new AttackTask(playerObj));
        }
    }
}

public enum OrderType {
    Set, Assign
}