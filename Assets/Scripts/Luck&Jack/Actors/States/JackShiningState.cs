using UnityEngine;

public class JackShiningState : State
{

    private readonly Jack _jack;
    private readonly CharacterController _characterController;
    private readonly RotationController _rotationController;
    private readonly Animator _animator;
    private readonly JackShiner _shiner;
    private readonly SoundPlayer _soundPlayer;

    public JackShiningState(Jack jack, CharacterController characterController, RotationController rotationController, Animator animator, JackShiner shiner, SoundPlayer soundPlayer)
    {
        _jack = jack;
        _characterController = characterController;
        _rotationController = rotationController;
        _animator = animator;
        _shiner = shiner;
        _soundPlayer = soundPlayer;
    }

    public override void Start()
    {
        _rotationController.enabled = false;
        _shiner.EnableShining();
        if (!_soundPlayer.IsPlaying)
        {
            _soundPlayer.PlaySound();
        }
    }

    public override void Tick()
    {
        _jack.transform.forward = Quaternion.Euler(0, _jack.DesiredDirection.x * 130f * Time.deltaTime, 0) * _jack.transform.forward;
        _animator.SetFloat(PlayerCharacter.AnimatorSpeedParameter, _jack.DesiredDirection.x);
    }

    public override void End()
    {
        _rotationController.enabled = true;
        _shiner.DisableShining();
    }

}