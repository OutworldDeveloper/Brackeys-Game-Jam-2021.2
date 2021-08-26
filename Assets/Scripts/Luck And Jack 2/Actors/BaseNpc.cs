using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(RotationController))]
public abstract class BaseNpc : Actor
{

    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;

    public float Speed => _speed;
    protected NavMeshAgent Agent { get; private set; }
    protected RotationController RotationController { get; private set; }
    protected Animator Animator => _animator;

    public virtual void MoveTo(FlatVector destination)
    {
        if (IsDead == false)
        {
            Agent.SetDestination(destination);
        }
    }

    public virtual void Stop()
    {
        Agent.ResetPath();
    }

    protected override void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        RotationController = GetComponent<RotationController>();
        Agent.updateRotation = false;
        base.Awake();
    }

    protected override State CreateDeathState()
    {
        return new DeathState(_animator);
    }

}