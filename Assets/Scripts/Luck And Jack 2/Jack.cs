public class Jack : PlayerCharacter
{

    protected const string AttackTrigger = "interact";

    public void TryAttack()
    {
        StateMachine.FireTrigger(AttackTrigger);
    }

    protected override State CreateDefaultState()
    {
        return new PlayerCharacterMovementState(this, CharacterController, RotationController, Animator);
    }

    protected override State CreateDeathState()
    {
        return new DeathState(Animator);
    }

}