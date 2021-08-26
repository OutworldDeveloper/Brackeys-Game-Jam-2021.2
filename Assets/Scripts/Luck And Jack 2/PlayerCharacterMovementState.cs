using UnityEngine;

public class PlayerCharacterMovementState : State
{

    private readonly PlayerCharacter _playerCharacter;
    private readonly CharacterController _characterController;
    private readonly RotationController _rotationController;
    private readonly Animator _animator;

    public PlayerCharacterMovementState(PlayerCharacter playerCharacter, CharacterController characterController, RotationController rotationController, Animator animator)
    {
        _playerCharacter = playerCharacter;
        _characterController = characterController;
        _rotationController = rotationController;
        _animator = animator;
    }

    public override void Tick()
    {
        _rotationController.LookIn(_playerCharacter.InputVector);

        var moveVector = _playerCharacter.InputVector * _playerCharacter.Speed + Physics.gravity;
        _characterController.Move(moveVector * Time.deltaTime);
    }

}
