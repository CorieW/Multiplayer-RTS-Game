using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class EntitySelectionHandler : MonoBehaviour
{
    public List<PlayerEntity> selectedPlayerEntities { get; } = new List<PlayerEntity>();
    public List<Unit> selectedUnits 
    { 
        get
        { 
            List<Unit> units = new List<Unit>();
            foreach (PlayerEntity playerEntity in selectedPlayerEntities)
            {
                if (!(playerEntity is Unit)) continue;

                units.Add(playerEntity as Unit);
            }
            return units;
        } 
    }
    public List<Building> selectedBuildings 
    { 
        get
        { 
            List<Building> buildings = new List<Building>();
            foreach (PlayerEntity playerEntity in selectedPlayerEntities)
            {
                if (!(playerEntity is Building)) continue;

                buildings.Add(playerEntity as Building);
            }
            return buildings;
        } 
    }

    [SerializeField] private RectTransform _selectionArea = null;
    [SerializeField] private LayerMask _layerMask = new LayerMask();

    public static event Action<PlayerEntity> OnSelectEntity;
    public static event Action<PlayerEntity> OnDeselectEntity;

    private Vector2 _startPosition;
    private RTSPlayer _player;
    private Camera _mainCamera;

    private void Awake()
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
            while (selectedPlayerEntities.Count > 0) // For each didn't work beacuse I am modifying whilst looping
            {
                DeselectEntity(selectedPlayerEntities[0]);
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
            if (!hit.collider.TryGetComponent<PlayerEntity>(out PlayerEntity playerEntity)) return;
            if (!playerEntity.hasAuthority) return;
            if (selectedPlayerEntities.Contains(playerEntity))
            {
                DeselectEntity(playerEntity);
                return;
            }

            SelectEntity(playerEntity);

            return;
        }

        Vector2 min = _selectionArea.anchoredPosition - (_selectionArea.sizeDelta / 2);
        Vector2 max = _selectionArea.anchoredPosition + (_selectionArea.sizeDelta / 2);

        foreach (PlayerEntity playerEntity in _player.GetPlayerEntities())
        { 
            // Only units can be selected by dragging.
            if (!(playerEntity is Unit)) continue;
            if (!playerEntity.hasAuthority) continue;
            if (selectedPlayerEntities.Contains(playerEntity)) continue;

            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(playerEntity.transform.position);

            if (screenPosition.x > min.x && screenPosition.x < max.x && screenPosition.y > min.y && screenPosition.y < max.y)
            {
                SelectEntity(playerEntity);
            }
        }
    }

    private void SelectEntity(PlayerEntity playerEntity)
    {
        selectedPlayerEntities.Add(playerEntity);
        OnSelectEntity?.Invoke(playerEntity);
        playerEntity.Select();
    }

    private void DeselectEntity(PlayerEntity playerEntity)
    {
        selectedPlayerEntities.Remove(playerEntity);
        OnDeselectEntity?.Invoke(playerEntity);
        playerEntity.Deselect();
    }
}
