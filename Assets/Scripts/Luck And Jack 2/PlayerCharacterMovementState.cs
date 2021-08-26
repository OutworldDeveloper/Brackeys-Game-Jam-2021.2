using UnityEngine;

public class PlayerCharacterMovementState : State
{

    private const string AnimatorSpeedParameter = "speed";
    
    protected readonly PlayerCharacter PlayerCharacter;
    protected readonly CharacterController CharacterController;
    protected readonly RotationController RotationController;
    protected readonly Animator Animator;

    private float _currentAnimationMagnitude;

    public PlayerCharacterMovementState(PlayerCharacter playerCharacter, CharacterController characterController, RotationController rotationController, Animator animator)
    {
        PlayerCharacter = playerCharacter;
        CharacterController = characterController;
        RotationController = rotationController;
        Animator = animator;
    }

    public override void Tick()
    {
        RotationController.LookIn(PlayerCharacter.DesiredDirection);

        var moveVector = PlayerCharacter.DesiredDirection * PlayerCharacter.Speed + Physics.gravity;
        CharacterController.Move(moveVector * Time.deltaTime);

        // 20 is just a random const that works for some reason
        _currentAnimationMagnitude = Mathf.MoveTowards(_currentAnimationMagnitude, CharacterController.velocity.magnitude, 20f * Time.deltaTime);
        Animator.SetFloat(AnimatorSpeedParameter, _currentAnimationMagnitude);
    }

}
