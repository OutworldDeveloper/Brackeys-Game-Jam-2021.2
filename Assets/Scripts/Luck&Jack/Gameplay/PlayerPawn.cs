using UnityEngine;
using Zenject;

public class PlayerPawn : Pawn
{

    [SerializeField] private float _cameraSoloModeDistance = 7.5f;
    [SerializeField] private float _cameraPartyModeDistance = 9.5f;
    [SerializeField] private Setting_Float _cameraSpeed;
    [Inject] private Luck _luck;
    [Inject] private Jack _jack;
    [Inject] private PauseMenu _pauseMenu;
    [Inject] private IConsole _console;

    private FlatVector _luckInput;
    private FlatVector _jackInput;

    private bool _isCameraInSoloMode = true;
    private Vector3 _virtualCameraTarget;
    private Vector3 _virtualCameraPosition;
    private Vector3 _cameraVelocity;

    protected override void OnPawnStart()
    {
        InputReciver.BindAxis("luck_forward", (value) => _luckInput.z = value);
        InputReciver.BindAxis("luck_right", (value) => _luckInput.x = value);
        InputReciver.BindAxis("jack_forward", (value) => _jackInput.z = value);
        InputReciver.BindAxis("jack_right", (value) => _jackInput.x = value);
        InputReciver.BindInputActionPressed("luck_interact", _luck.TryInteract);
        InputReciver.BindInputActionPressed("jack_attack", _jack.TryAttack);
        InputReciver.BindInputActionPressed("jack_shine", _jack.StartShining);
        InputReciver.BindInputActionRelesed("jack_shine", _jack.StopShining);
        InputReciver.BindInputActionPressed("pause", _pauseMenu.Show);
    }

    protected override void OnPossesesed()
    {
        _virtualCameraTarget = (FlatVector)_luck.transform.position;
        _virtualCameraPosition = _virtualCameraTarget;
        _cameraVelocity = Vector3.zero;
        _console.RegisterObject(_luck);
        _console.RegisterObject(_jack);
    }

    public override void PossessedTick()
    {
        _luck.Move(_luckInput);
        _jack.Move(_jackInput);

        var luckPosition = _luck.transform.GetFlatPosition();
        var jackPosition = _jack.transform.GetFlatPosition();

        var distance = FlatVector.Distance(luckPosition, jackPosition);

        if (_isCameraInSoloMode)
        {
            if (distance < _cameraSoloModeDistance)
                _isCameraInSoloMode = false;
        }
        else
        {
            if (distance > _cameraPartyModeDistance)
                _isCameraInSoloMode = true;
        }

        if (_isCameraInSoloMode)
        {
            _virtualCameraTarget = luckPosition;
        }
        else
        {
            _virtualCameraTarget = Vector3.Lerp(jackPosition, luckPosition, 0.5f);
        }

        var invertedSpeed = 1f - _cameraSpeed.GetValue();
        _virtualCameraPosition = Vector3.SmoothDamp(_virtualCameraPosition, _virtualCameraTarget, ref _cameraVelocity, invertedSpeed);

        CameraPosition = _virtualCameraPosition - Vector3.forward * 8f + Vector3.up * 7.5f;
        CameraRotation = Quaternion.Euler(46.13f, 0f, 0f);
    }

    protected override void OnUnpossessed()
    {
        _luck.Move(FlatVector.zero);
        _jack.Move(FlatVector.zero);
        _console.DeregisterObject(_luck);
        _console.DeregisterObject(_jack);
    }

}