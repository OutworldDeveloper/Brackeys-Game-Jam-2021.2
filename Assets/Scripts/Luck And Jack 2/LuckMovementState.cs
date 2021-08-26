using UnityEngine;

public class LuckMovementState : PlayerCharacterMovementState
{

    private const string IdleTrigger = "idle";
    private const float IdleAnimationCooldownMin = 4f;
    private const float IdleAnimationCooldownMax = 20f;
    private const float IdleTimeToAfk = 3f;

    private readonly CharacterController _characterController;
    private readonly Animator _animator;

    private float _idleTime;
    private float _idleAnimationAvaliableTime;
    private bool _wasAfkLastFrame;

    public LuckMovementState(
        PlayerCharacter playerCharacter, 
        CharacterController characterController, 
        RotationController rotationController, 
        Animator animator) : 
        base(playerCharacter, characterController, rotationController, animator)
    {
        _characterController = characterController;
        _animator = animator;
    }

    public override void Tick()
    {
        base.Tick();

        if (_characterController.velocity.magnitude == 0f)
        {
            _idleTime += Time.deltaTime;
        }
        else
        {
            _idleTime = 0f;
        }

        bool isAfk = _idleTime > IdleTimeToAfk;

        if (!_wasAfkLastFrame && isAfk)
        {
            ResetIdleAnimationCooldown();
        }

        if (isAfk && Time.time > _idleAnimationAvaliableTime)
        {
            _animator.SetTrigger(IdleTrigger);
            ResetIdleAnimationCooldown();
        }

        _wasAfkLastFrame = isAfk;
    }

    private void ResetIdleAnimationCooldown()
    {
        _idleAnimationAvaliableTime = Time.time + Random.Range(IdleAnimationCooldownMin, IdleAnimationCooldownMax);
    }

}