public class Luck : PlayerCharacter
{

    protected const string InteractTrigger = "interact";

    public Interactable TargetInteractable { get; private set; }

    public void TryInteract()
    {
        if (HasInteractableInRange(out Interactable interactable))
        {
            TargetInteractable = interactable;
            StateMachine.FireTrigger(InteractTrigger);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        var goingToInteractableState = new GoingToInteractableState(this, CharacterController, RotationController, Animator);
        var interactingState = new InteractingState(this, CharacterController, RotationController, Animator);

        StateMachine.CreateTransition(DefaultState, goingToInteractableState, () => StateMachine.CheckTrigger(InteractTrigger));
        StateMachine.CreateTransition(goingToInteractableState, interactingState, () => goingToInteractableState.HasReachedTarget);
        StateMachine.CreateTransition(goingToInteractableState, DefaultState, () => goingToInteractableState.CanBeCanceled && DesiredDirection != FlatVector.zero);
        StateMachine.CreateTransition(interactingState, DefaultState, () => interactingState.IsDoneInteracting);
    }

    protected override State CreateDefaultState()
    {
        return new LuckMovementState(this, CharacterController, RotationController, Animator);
    }

    private bool HasInteractableInRange(out Interactable result)
    {
        foreach (var interactable in FindObjectsOfType<Interactable>())
        {
            if (interactable.IsAvaliable())
            {
                if (FlatVector.Distance(transform.GetFlatPosition(), interactable.RangeCenterPoint) < interactable.Range)
                {
                    result = interactable;
                    return true;
                }
            }
        }
        result = null;
        return false;
    }

}