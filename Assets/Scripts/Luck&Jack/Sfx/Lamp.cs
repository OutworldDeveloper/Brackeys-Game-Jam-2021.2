using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Lamp : MonoBehaviour
{

    private Rigidbody _rigidbody;
    private Collider[] _colliders;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _colliders = GetComponents<Collider>();
    }

    private void Start()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
    }

    public void Fall()
    {
        transform.SetParent(null);
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        foreach (var collider in _colliders)
        {
            collider.enabled = true;
        }
    }

}