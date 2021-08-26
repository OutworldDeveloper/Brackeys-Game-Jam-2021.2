using UnityEngine;

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