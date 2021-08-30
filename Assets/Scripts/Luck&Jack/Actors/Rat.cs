using System;
using UnityEngine;
using Zenject;

public class Rat : BaseNpc
{

    private const string JumpTrigger = "jump";

    [SerializeField] private int _damage;
    [SerializeField] private SoundPlayer _biteSoundPlayer;

    public int Damage => _damage;
    public FlatVector JumpDirection { get; private set; }

    public void Jump(FlatVector direction)
    {
        if (direction == FlatVector.zero)
        {
            throw new ArgumentException("Jump direction cannot be zero");
        }

        JumpDirection = direction;
        StateMachine.FireTrigger(JumpTrigger);
    }

    protected override State CreateDefaultState()
    {
        return new NpcMovementState(this, Agent, RotationController, Animator);
    }

    protected override State CreateDeathState()
    {
        return new RatDeathState(this, Agent, RotationController, Animator);
    }

    protected override void Awake()
    {
        base.Awake();

        var jumpState = new RatJumpState(this, Agent, RotationController, Animator, _biteSoundPlayer);

        StateMachine.CreateTransition(DefaultState, jumpState, () => StateMachine.CheckTrigger(JumpTrigger));
        StateMachine.CreateTransition(jumpState, DefaultState, () => jumpState.IsEnded);
    }

    public class Factory : PlaceholderFactory<Rat> { }

}