using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{

    [SerializeField] private float _initialRotationSpeed = 900f;
    [SerializeField] private float _rotationSpeed = 1200f;
    [SerializeField] private float _acceleration = 1500f;

    private Quaternion _desiredRotation;
    private float _currentRotationSpeed = 0f;
    private bool _isStopped;

    public void SetDesiredRotation(Quaternion rotation)
    {
        _desiredRotation = rotation;
    }

    public void LookAt(FlatVector targetPosition)
    {
        FlatVector direction = (targetPosition - (FlatVector)transform.position).normalized;
        LookIn(direction);
    }

    public void LookIn(FlatVector direction)
    {
        if (direction != FlatVector.zero)
        {
            _isStopped = false;
            _desiredRotation = Quaternion.LookRotation(direction.Vector3, Vector3.up);
        }
    }

    public void Stop()
    {
        _isStopped = true;
    }

    private void Start()
    {
        _currentRotationSpeed = _initialRotationSpeed;
    }

    private void Update()
    {
        if (transform.rotation == _desiredRotation || _isStopped)
        {
            _currentRotationSpeed = _initialRotationSpeed;
            return;
        }
        _currentRotationSpeed = Mathf.Min(_rotationSpeed, _currentRotationSpeed + _acceleration * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _desiredRotation, _currentRotationSpeed * Time.deltaTime);
    }

}