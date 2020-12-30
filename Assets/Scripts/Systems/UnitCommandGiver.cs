using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] private EntitySelectionHandler _entitytSelectionHandler = null;
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
        OrderType orderType = Keyboard.current.leftShiftKey.isPressed ? OrderType.Assign : OrderType.Set;

        if (Physics.Raycast(mousePos, Vector3.forward, out RaycastHit hit, Mathf.Infinity, _layerMask))
        {
            ResourceDeposit resourceDeposit = hit.collider.GetComponent<ResourceDeposit>();
            ResourceDrop resourceDrop = hit.collider.GetComponent<ResourceDrop>();

            if (resourceDeposit) GiveOrder(new ResourceHarvestTask(resourceDeposit), orderType);
            else if (resourceDrop) GiveOrder(new HaulTask(resourceDrop), orderType);
        }
        else
        {
            GiveOrder(new MoveTask(mousePos), orderType);
            return;
        }
    }

    private void GiveOrder(Task task, OrderType orderType)
    {
        foreach(Unit selectedUnit in _entitytSelectionHandler.selectedUnits)
        {
            if (orderType == OrderType.Set) selectedUnit.GetTaskList().SetTask(task);
            else selectedUnit.GetTaskList().AssignTask(task);
        }
    }
}

public enum OrderType {
    Set, Assign
}