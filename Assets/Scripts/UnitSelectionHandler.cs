using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask = new LayerMask();

    private Camera _mainCamera;

    public List<Unit> selectedUnits { get; } = new List<Unit>();

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    private void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            DeselectUnits();
            // Start selection area
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            SelectUnit();
        }
    }

    private void SelectUnit()
    {
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.zero, _layerMask);

        if(!hit) return;
        if(!hit.collider.TryGetComponent<Unit>(out Unit unit)) return;
        if(!unit.hasAuthority) return;

        selectedUnits.Add(unit);

        foreach(Unit selectedUnit in selectedUnits)
        {
            selectedUnit.Select();
        }
    }

    private void DeselectUnits()
    {
        foreach(Unit selectedUnit in selectedUnits)
        {
            selectedUnit.Deselect();
        }

        selectedUnits.Clear();
    }
}
