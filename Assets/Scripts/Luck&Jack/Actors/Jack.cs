using UnityEngine;

[RequireComponent(typeof(JackShiner))]
public class Jack : PlayerCharacter
{

    protected const string AttackTrigger = "interact";

    [SerializeField] private SoundPlayer _attackSoundPlayer;
    [SerializeField] private SoundPlayer _hitSoundPlayer;
    [SerializeField] private SoundPlayer _wakingUpSoundPlayer;
    [SerializeField] private SoundPlayer _startedShiningSoundPlayer;

    public bool IsSleeping { get; private set; }

    private JackShiner _shiner;
    private bool _wantsToShine;

    public void TryAttack()
    {
        StateMachine.FireTrigger(AttackTrigger);
    }

    public void StartShining()
    {
        _wantsToShine = true;
    }

    public void StopShining()
    {
        _wantsToShine = false;
    }

    public void Sleep()
    {
        IsSleeping = true;
    }

    public void WakeUp()
    {
        IsSleeping = false;
    }

    public bool IsInLight(Transform other)
    {
        return _shiner.IsInLight(other);
    }    

    protected override State CreateDefaultState()
    {
        return new PlayerCharacterMovementState(this, CharacterController, RotationController, Animator);
    }

    public override bool ApplyDamage(int damage, FlatVector direction) 
    {
        return false;
    }

    protected override void Awake()
    {
        base.Awake();

        _shiner = GetComponent<JackShiner>();

        var attackState = new JackAttackState(this, RotationController, Animator, _attackSoundPlayer, _hitSoundPlayer);
        var sleepingState = new JackSleepingState(CharacterController, Animator, _wakingUpSoundPlayer);
        var shiningState = new JackShiningState(this, CharacterController, RotationController, Animator, _shiner, _startedShiningSoundPlayer);

        StateMachine.CreateTransition(DefaultState, attackState, () => StateMachine.CheckTrigger(AttackTrigger) && !attackState.IsOnCooldown);
        StateMachine.CreateTransition(DefaultState, shiningState, () => _wantsToShine);
        StateMachine.CreateTransition(shiningState, DefaultState, () => !_wantsToShine);
        StateMachine.CreateTransition(attackState, DefaultState, () => attackState.IsEnded);
        StateMachine.CreateTransition(sleepingState, () => IsSleeping);
        StateMachine.CreateTransition(sleepingState, DefaultState, () => !IsSleeping);
    }

}