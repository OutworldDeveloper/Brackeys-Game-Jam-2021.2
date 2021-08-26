public class Luck : PlayerCharacter
{

    protected const string InteractTrigger = "interact";

    public void TryInteract()
    {
        StateMachine.FireTrigger(InteractTrigger);
    }

    protected override void Awake()
    {
        base.Awake();
        //var interactionState = new State();
        //StateMachine.CreateTransition(DefaultState, interactionState, () => StateMachine.CheckTrigger(InteractTrigger));
    }

    protected override State CreateDefaultState()
    {
        return new LuckMovementState(this, CharacterController, RotationController, Animator);
    }

    protected override State CreateDeathState()
    {
        return new DeathState(Animator);
    }

}