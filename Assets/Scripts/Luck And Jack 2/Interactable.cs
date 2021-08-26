using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    [SerializeField] private Vector3 _pointOffset = Vector3.zero;
    [SerializeField] private Vector3 _rangeCenterPointOffset = Vector3.zero;
    [SerializeField] private Vector3 _direction = Vector3.forward;
    [SerializeField] private float _range = 2f;
    [SerializeField] private Vector3 _textPosition = Vector3.up * 2f;

    public float Range => _range;
    public FlatVector Point => (FlatVector)transform.TransformPoint(_pointOffset);
    public FlatVector RangeCenterPoint => (FlatVector)transform.TransformPoint(_rangeCenterPointOffset);
    public FlatVector FacingDirection => (FlatVector)transform.TransformDirection(_direction);
    public Vector3 TextPosition => transform.position + _textPosition;

    public abstract string InteractionText { get; }
    public abstract string AnimationTrigger { get; }

    public bool IsInteracting { get; private set; }
    protected float InteractionStartTime;
    protected Luck Interactor;

    public virtual bool IsAvaliable()
    {
        return !IsInteracting;
    }

    public void StartInteraction(Luck luck)
    {
        Interactor = luck;
        InteractionStartTime = Time.time;
        IsInteracting = true;
        OnInteractionStarted();
    }

    private void Update()
    {
        if (IsInteracting)
        {
            if (Interact() == true)
            {
                IsInteracting = false;
            }
        }
    }

    protected virtual void OnInteractionStarted() { }
    protected abstract bool Interact();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Point, Vector3.up);
        Gizmos.DrawRay(Point, FacingDirection);
        Gizmos.DrawWireSphere(RangeCenterPoint, Range);
        Gizmos.DrawWireSphere(TextPosition, 0.2f);
    }

}