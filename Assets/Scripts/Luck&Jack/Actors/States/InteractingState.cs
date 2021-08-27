using UnityEngine;

public class InteractingState : State
{

    public bool IsDoneInteracting => _luck.TargetInteractable.IsInteracting == false;

    private readonly Luck _luck;
    private readonly CharacterController _characterController;
    private readonly RotationController _rotationController;
    private readonly Animator _animator;

    public InteractingState(Luck luck, CharacterController characterController, RotationController rotationController, Animator animator)
    {
        _luck = luck;
        _characterController = characterController;
        _rotationController = rotationController;
        _animator = animator;
    }

    public override void Start()
    {
        _characterController.enabled = false;
       
        var interactable = _luck.TargetInteractable;

        _rotationController.LookIn(interactable.FacingDirection);
        _animator.SetTrigger(interactable.AnimationTrigger);

        interactable.StartInteraction(_luck);
    }

    public override void End()
    {
        _characterController.enabled = true;
    }

}