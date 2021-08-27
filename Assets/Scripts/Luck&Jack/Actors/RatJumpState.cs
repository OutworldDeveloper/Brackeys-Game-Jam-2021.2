using UnityEngine;
using UnityEngine.AI;

public class RatJumpState : State
{

    private const float JumpDuration = 0.5f;
    private const float JumpSpeed = 12f;
    private const float BiteRange = 1.3f;
    private const float BiteAngle = 360f; // 50f

    public bool IsEnded => Time.time > _endTime || _hasSetDamage;

    private readonly Rat _rat;
    private readonly NavMeshAgent _agent;
    private readonly RotationController _rotationController;
    private readonly Animator _animator;
    private readonly SoundPlayer _soundPlayer;

    private float _endTime;
    private bool _hasSetDamage;

    public RatJumpState(Rat rat, NavMeshAgent agent, RotationController rotationController, Animator animator, SoundPlayer soundPlayer)
    {
        _rat = rat;
        _agent = agent;
        _rotationController = rotationController;
        _animator = animator;
        _soundPlayer = soundPlayer;
    }

    public override void Start()
    {
        _agent.ResetPath();

        _endTime = Time.time + JumpDuration;

        _agent.isStopped = true;
        _hasSetDamage = false;
    }

    public override void Tick()
    {
        var direction = _rat.JumpDirection;

        _rotationController.LookIn(direction);

        _agent.Move(direction * JumpSpeed * Time.deltaTime);

        var attackHelper = new AttackHelper((FlatVector)_rat.transform.position, _rat.Team, BiteRange, BiteAngle);

        attackHelper.Attack((target, direction) => 
        {
            target.ApplyDamage(33f, direction);
            _hasSetDamage = true;
            return true;
        }, true);

        if (_hasSetDamage)
        {
            _soundPlayer.PlaySound();
        }
    }

    public override void End()
    {
        _agent.isStopped = false;
    }

}