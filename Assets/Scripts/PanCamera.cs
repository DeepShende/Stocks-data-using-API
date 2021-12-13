using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanCamera : MonoBehaviour
{
    [SerializeField]
    private float _mouseSensitivity = 3.0f;

    private float _rotationY;
    private float _rotationX;
    float mouseX;
    float mouseY;

    Vector3 _nextRotation;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _distanceFromTarget = 3.0f;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;
    private Vector3 _XandYoffset = new Vector3(145, 120.5f);

    [SerializeField]
    private float _smoothTime = 0.2f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-40, 40);
    [SerializeField]
    private Vector2 _rotationYMinMax = new Vector2(0, 40);

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

            _rotationY += mouseX;
            _rotationX += mouseY;

            _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);
            _rotationY = Mathf.Clamp(_rotationY, _rotationYMinMax.x, _rotationYMinMax.y);

            _nextRotation = new Vector3(_rotationX, _rotationY);

            _currentRotation = Vector3.SmoothDamp(_currentRotation, _nextRotation, ref _smoothVelocity, _smoothTime);
            transform.localEulerAngles = _currentRotation;

            transform.position = _target.position - transform.forward * _distanceFromTarget + _XandYoffset;
        }
    }
}
