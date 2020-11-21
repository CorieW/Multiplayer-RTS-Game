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

        TryMove(mousePos);
    }

    private void TryMove(Vector3 pos)
    {
        foreach(Unit unit in unitSelectionHandler.selectedPlayerObjects) //? I am not sure this line will work, might have to do PlayerObject as Unit.
        {
            unit.CmdMove(pos);
        }
    }
}
