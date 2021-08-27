using UnityEngine;

public class Jack : PlayerCharacter
{

    protected const string AttackTrigger = "interact";

    [SerializeField] private SoundPlayer _attackSoundPlayer;
    [SerializeField] private SoundPlayer _hitSoundPlayer;
    [SerializeField] private SoundPlayer _wakingUpSoundPlayer;

    public bool IsSleeping { get; private set; }

    public void TryAttack()
    {
        StateMachine.FireTrigger(AttackTrigger);
    }

    public void Sleep()
    {
        IsSleeping = true;
    }

    public void WakeUp()
    {
        IsSleeping = false;
    }

    protected override State CreateDefaultState()
    {
        return new PlayerCharacterMovementState(this, CharacterController, RotationController, Animator);
    }

    protected override void Awake()
    {
        base.Awake();

        var attackState = new JackAttackState(this, RotationController, Animator, _attackSoundPlayer, _hitSoundPlayer);
        var sleepingState = new JackSleeping(CharacterController, Animator, _wakingUpSoundPlayer);

        StateMachine.CreateTransition(DefaultState, attackState, () => StateMachine.CheckTrigger(AttackTrigger) && !attackState.IsOnCooldown);
        StateMachine.CreateTransition(attackState, DefaultState, () => attackState.IsEnded);
        StateMachine.CreateTransition(sleepingState, () => IsSleeping);
        StateMachine.CreateTransition(sleepingState, DefaultState, () => !IsSleeping);
    }

}

public class JackAttackState : State
{

    private const float AttackDuration = 0.3f;
    private const float AttackCooldown = 0.5f;
    private const float AttackRange = 2.5f;
    private const string AttackTrigger = "attack";

    public bool IsEnded => Time.time > _startTime + AttackDuration;
    public bool IsOnCooldown => Time.time < _cooldownEndTime;

    private readonly Jack _jack;
    private readonly RotationController _rotationController;
    private readonly Animator _animator;
    private readonly SoundPlayer _attackSoundPlayer;
    private readonly SoundPlayer _hitSoundPlayer;

    private float _startTime;
    private float _cooldownEndTime;

    public JackAttackState(Jack jack, RotationController rotationController, Animator animator, SoundPlayer attackSoundPlayer, SoundPlayer hitSoundPlayer)
    {
        _jack = jack;
        _rotationController = rotationController;
        _animator = animator;
        _attackSoundPlayer = attackSoundPlayer;
        _hitSoundPlayer = hitSoundPlayer;
    }

    public override void Start()
    {
        _startTime = Time.time;
        _cooldownEndTime = Time.time + AttackCooldown;
        _rotationController.enabled = false;
        _attackSoundPlayer.PlaySound();
        _animator.SetTrigger(AttackTrigger);
        _attackSoundPlayer.PlaySound();
    }

    public override void Tick()
    {
        var hasSetDamage = false;
        var attackHelper = new AttackHelper((FlatVector)_jack.transform.position, _jack.Team, AttackRange, 360f);

        attackHelper.Attack((target, direction) =>
        {
            target.ApplyDamage(999f, direction);
            hasSetDamage = true;
            return true;
        });

        if (hasSetDamage && _hitSoundPlayer.IsPlaying == false)
        {
            _hitSoundPlayer.PlaySound();
        }
    }

    public override void End()
    {
        _rotationController.enabled = true;
    }

}