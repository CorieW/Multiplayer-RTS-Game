using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] private UnitSelectionHandler unitSelectionHandler = null;
    [SerializeField] private LayerMask layerMask = new LayerMask();

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (!Physics2D.Raycast(mousePos, Vector2.zero, layerMask));

        if (Keyboard.current.leftShiftKey.isPressed) GiveMoveOrder(mousePos, OrderType.Assign);
        else GiveMoveOrder(mousePos, OrderType.Set);
    }

    // pos is the position that the unit should move to. 
    // orderType indicates whether the user wants to add that task to the current tasks or make that the only task the unit should perform.
    private void GiveMoveOrder(Vector3 pos, OrderType orderType)
    {
        foreach(PlayerObject selectedPlayerObj in unitSelectionHandler.selectedPlayerObjects)
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
        foreach(PlayerObject selectedPlayerObj in unitSelectionHandler.selectedPlayerObjects)
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
        foreach(PlayerObject selectedPlayerObj in unitSelectionHandler.selectedPlayerObjects)
        {
            if (!(selectedPlayerObj is Unit)) continue;
            
            Unit unit = selectedPlayerObj as Unit;
            if (!unit.CanBuild()) continue; // Todo: Replace with CanRepair

            if (orderType == OrderType.Set) unit.GetTaskList().SetTask(new RepairTask(building));
            else unit.GetTaskList().AssignTask(new RepairTask(building));
        }
    }

    private void GiveResourceHarvestOrder(ResourceDeposit resourceDepo, OrderType orderType)
    {
        foreach(PlayerObject selectedPlayerObj in unitSelectionHandler.selectedPlayerObjects)
        {
            if (!(selectedPlayerObj is Unit)) continue;
            
            Unit unit = selectedPlayerObj as Unit;
            if (!unit.CanResourceHarvest()) continue;

            if (orderType == OrderType.Set) unit.GetTaskList().SetTask(new ResourceHarvestTask(resourceDepo));
            else unit.GetTaskList().AssignTask(new ResourceHarvestTask(resourceDepo));
        }
    }

    private void GiveHaulOrder(Resource resource, OrderType orderType)
    {
        foreach(PlayerObject selectedPlayerObj in unitSelectionHandler.selectedPlayerObjects)
        {
            if (!(selectedPlayerObj is Unit)) continue;
            
            Unit unit = selectedPlayerObj as Unit;
            if (!unit.CanResourceHarvest()) continue; // Todo: Replace with CanHaul

            if (orderType == OrderType.Set) unit.GetTaskList().SetTask(new HaulTask(resource));
            else unit.GetTaskList().AssignTask(new HaulTask(resource));
        }
    }

    private void GiveAttackOrder(PlayerObject playerObj, OrderType orderType)
    {
        foreach(PlayerObject selectedPlayerObj in unitSelectionHandler.selectedPlayerObjects)
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