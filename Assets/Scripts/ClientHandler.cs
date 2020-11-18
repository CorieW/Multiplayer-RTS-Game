using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClientHandler : NetworkBehaviour 
{ // Handles all the basic client-side actions, such as inputs and related non-networked variables.

    [Header("Dependencies")]
    private Camera _mainCamera;
    [SerializeField] private UnitSelectionHandler _unitSelectionHandler = null;

    [Header("Settings")]
    [Tooltip("The layer mask for which object layers will register a select event and other events. For example, exclude the chunks as they can't be selected.")]
    [SerializeField] private LayerMask _selectLayerMask = new LayerMask();

    [Client]
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    [Client]
    private void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) return;
        Debug.Log(_mainCamera);

        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (!Physics2D.Raycast(mousePos, Vector2.zero, _selectLayerMask))

        Move(mousePos);
    }

    [Client]
    private void Move(Vector3 pos)
    {
        foreach(Unit unit in _unitSelectionHandler.selectedUnits)
        {
            unit.Move(pos);
        }
    }
}