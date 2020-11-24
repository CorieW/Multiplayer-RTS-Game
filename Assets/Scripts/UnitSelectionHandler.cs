using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private RectTransform unitSelectionArea = null;

    [SerializeField] private LayerMask layerMask = new LayerMask();

    private Vector2 startPosition;

    private RTSPlayer player;
    private Camera mainCamera;

    public List<PlayerObject> selectedPlayerObjects { get; } = new List<PlayerObject>();

    private void Start()
    {
        mainCamera = Camera.main;
    }
    
    private void Update() {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }

    private void StartSelectionArea()
    {
        if (!Keyboard.current.leftShiftKey.isPressed) DeselectUnits();



        unitSelectionArea.gameObject.SetActive(true);

        startPosition = Mouse.current.position.ReadValue();

        UpdateSelectionArea();
    }

    private void UpdateSelectionArea()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - startPosition.x;
        float areaHeight = mousePosition.y - startPosition.y;

        unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
        unitSelectionArea.anchoredPosition = startPosition + new Vector2(areaWidth / 2, areaHeight / 2);
    }

    private void ClearSelectionArea()
    {
        unitSelectionArea.gameObject.SetActive(false);

        if (unitSelectionArea.sizeDelta.magnitude == 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;
            if (!hit.collider.TryGetComponent<PlayerObject>(out PlayerObject playerObject)) return;
            if (!playerObject.hasAuthority) return;

            selectedPlayerObjects.Add(playerObject);

            foreach (PlayerObject selectedPlayerObj in selectedPlayerObjects)
            {
                selectedPlayerObj.Select();
            }
        }

        Vector2 min = unitSelectionArea.anchoredPosition - (unitSelectionArea.sizeDelta / 2);
        Vector2 max = unitSelectionArea.anchoredPosition + (unitSelectionArea.sizeDelta / 2);

        foreach (PlayerObject playerObj in player.GetPlayerObjects())
        { 
            // Only units can be selected by dragging.
            if (!(playerObj is Unit)) continue;
            Unit unit = playerObj as Unit;

            if (selectedPlayerObjects.Contains(unit)) continue;

            Vector3 screenPosition = mainCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPosition.x > min.x && screenPosition.x < max.x && screenPosition.y > min.y && screenPosition.y < max.y)
            {
                selectedPlayerObjects.Add(unit);
                unit.Select();
            }
        }
    }

    private void DeselectUnits()
    {
        foreach (PlayerObject selectedPlayerObj in selectedPlayerObjects)
        {
            selectedPlayerObj.Deselect();
        }

        selectedPlayerObjects.Clear();
    }
}
