using UnityEngine;

public class JackSleepingState : State
{

    private const string AnimatorSleepingTrigger = "sleep";

    private readonly CharacterController _characterController;
    private readonly Animator _animator;
    private readonly SoundPlayer _soundPlayer;

    public JackSleepingState(CharacterController characterController, Animator animator, SoundPlayer wakingUpSoundPlayer)
    {
        _characterController = characterController;
        _animator = animator;
        _soundPlayer = wakingUpSoundPlayer;
    }

    public override void Start()
    {
        _characterController.enabled = false;
        _animator.SetBool(AnimatorSleepingTrigger, true);
    }

    public override void End()
    {
        _characterController.enabled = true;
        _animator.SetBool(AnimatorSleepingTrigger, false);
        _soundPlayer.PlaySound();
    }

}