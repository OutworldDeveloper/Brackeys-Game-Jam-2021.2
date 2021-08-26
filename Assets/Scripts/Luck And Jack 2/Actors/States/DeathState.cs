using UnityEngine;
using UnityEngine.AI;

public class DeathState : State
{

    private const string DeathTrigger = "death";
    private readonly Animator _animator;

    public DeathState(Animator animator)
    {
        _animator = animator;
    }

    public override void Start()
    {
        _animator.SetTrigger(DeathTrigger);
    }

}

public class RatDeathState : DeathState
{

    private const float KnockbackDuration = 0.2f;
    private const float KnockbackForce = 12f;

    private readonly Rat _rat;
    private readonly NavMeshAgent _agent;
    private readonly RotationController _rotationController;

    private float _endTime;

    public RatDeathState(Rat rat, NavMeshAgent agent, RotationController rotationController, Animator animator) : base(animator)
    {
        _rat = rat;
        _agent = agent;
        _rotationController = rotationController;
    }

    public override void Start()
    {
        base.Start();
        _endTime = Time.time + KnockbackDuration;
        _agent.ResetPath();
        _agent.isStopped = true;
        _rotationController.LookIn(-_rat.LastDamageDirection);
    }

    public override void Tick()
    {
        base.Tick();
        if (Time.time < _endTime)
        {
            _rat.transform.position += _rat.LastDamageDirection * KnockbackForce * Time.deltaTime;
        }
    }

}