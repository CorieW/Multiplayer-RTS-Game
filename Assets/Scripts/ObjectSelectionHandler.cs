using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectSelectionHandler : MonoBehaviour
{
    public List<PlayerObject> selectedPlayerObjects { get; } = new List<PlayerObject>();

    [SerializeField] private RectTransform _selectionArea = null;
    [SerializeField] private LayerMask _layerMask = new LayerMask();

    public static event Action<PlayerObject> OnSelectObject;
    public static event Action<PlayerObject> OnDeselectObject;

    private Vector2 _startPosition;
    private RTSPlayer _player;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }
    
    private void Update() {
        if (_player == null)
        {
            _player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            EndSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }

    private void StartSelectionArea()
    { // Performs all of the tasks that occur when the selection is began.
        if (!Keyboard.current.leftShiftKey.isPressed) 
        {
            while (selectedPlayerObjects.Count > 0) // For each didn't work beacuse I am modifying whilst looping
            {
                DeselectObject(selectedPlayerObjects[0]);
            }
        }


        _selectionArea.gameObject.SetActive(true);
        _startPosition = Mouse.current.position.ReadValue();
        UpdateSelectionArea();
    }

    private void UpdateSelectionArea()
    { // Performs all of the tasks that occur when the selection is happening.
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - _startPosition.x;
        float areaHeight = mousePosition.y - _startPosition.y;

        _selectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
        _selectionArea.anchoredPosition = _startPosition + new Vector2(areaWidth / 2, areaHeight / 2);
    }

    private void EndSelectionArea()
    { // Performs all of the tasks that occur when the selection is ended.
        _selectionArea.gameObject.SetActive(false);

        if (_selectionArea.sizeDelta.magnitude == 0)
        { // The mouse was clicked rather than dragged
            Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(mousePos, Vector3.forward, out RaycastHit hit, Mathf.Infinity, _layerMask)) return;
            if (!hit.collider.TryGetComponent<PlayerObject>(out PlayerObject playerObj)) return;
            if (!playerObj.hasAuthority) return;
            if (selectedPlayerObjects.Contains(playerObj))
            {
                DeselectObject(playerObj);
                return;
            }

            SelectObject(playerObj);

            return;
        }

        Vector2 min = _selectionArea.anchoredPosition - (_selectionArea.sizeDelta / 2);
        Vector2 max = _selectionArea.anchoredPosition + (_selectionArea.sizeDelta / 2);

        foreach (PlayerObject playerObj in _player.GetPlayerObjects())
        { 
            // Only units can be selected by dragging.
            if (!(playerObj is Unit)) continue;
            if (!playerObj.hasAuthority) continue;
            if (selectedPlayerObjects.Contains(playerObj)) continue;

            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(playerObj.transform.position);

            if (screenPosition.x > min.x && screenPosition.x < max.x && screenPosition.y > min.y && screenPosition.y < max.y)
            {
                SelectObject(playerObj);
            }
        }
    }

    private void SelectObject(PlayerObject playerObj)
    {
        selectedPlayerObjects.Add(playerObj);
        OnSelectObject?.Invoke(playerObj);
        playerObj.Select();
    }

    private void DeselectObject(PlayerObject playerObj)
    {
        selectedPlayerObjects.Remove(playerObj);
        OnDeselectObject?.Invoke(playerObj);
        playerObj.Deselect();
    }
}
