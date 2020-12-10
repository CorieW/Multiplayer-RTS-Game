using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraHandler : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Camera _camera;

    [Header("Attributes")]
    [SerializeField] private float _speed;
    [SerializeField] private float _mouseMoveSpeed;

    [Space]

    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;

    private Vector3 _previousDragPos;


    private void Awake()
    {
        if (!_camera) _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        // Keyboard camera movement below
        if (Keyboard.current.wKey.isPressed) // W key is pressed - Move up
            _camera.transform.position += Vector3.up * _speed * Time.deltaTime;
        if (Keyboard.current.aKey.isPressed) // A key is pressed - Move left
            _camera.transform.position += Vector3.left * _speed * Time.deltaTime;
        if (Keyboard.current.sKey.isPressed) // S key is pressed - Move down
            _camera.transform.position += Vector3.down * _speed * Time.deltaTime;
        if (Keyboard.current.dKey.isPressed) // D key is pressed - Move right
            _camera.transform.position += Vector3.right * _speed * Time.deltaTime;

        // Scrolling
        _camera.orthographicSize -= Mouse.current.scroll.ReadValue().y * Time.deltaTime;
        if (_camera.orthographicSize < _minZoom)
            _camera.orthographicSize = _minZoom;
        if (_camera.orthographicSize > _maxZoom)
            _camera.orthographicSize = _maxZoom;

        // Corner mouse movement
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = _camera.ScreenToViewportPoint(mousePos) - new Vector3(0.5f, 0.5f);
        // Todo: Need some better values with when the mouse should consider moving the camera.
        // Todo: Maybe add some smoothness, so that instead of instantly moving at a constant speed, it fades the movement speed in.
        if((Mathf.Abs(mousePos.x) >= 0.4 || Mathf.Abs(mousePos.y) >= 0.4) && (Mathf.Abs(mousePos.x) <= 0.5 && Mathf.Abs(mousePos.y) <= 0.5))
            _camera.transform.position += new Vector3(Mathf.Clamp(mousePos.x, -0.5f, 0.5f), Mathf.Clamp(mousePos.y, -0.5f, 0.5f)) * _mouseMoveSpeed * Time.deltaTime;
    }
}
