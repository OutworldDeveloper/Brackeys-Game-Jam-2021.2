using UnityEngine;
using Zenject;

public class PlayerPawn : Pawn
{

    [Inject] private Luck _luck;
    [Inject] private Jack _jack;

    private FlatVector _luckInput;
    private FlatVector _jackInput;

    protected override void OnPawnStart()
    {
        InputReciver.BindAxis("luck_forward", (value) => _luckInput.z = value);
        InputReciver.BindAxis("luck_right", (value) => _luckInput.x = value);
        InputReciver.BindAxis("jack_forward", (value) => _jackInput.z = value);
        InputReciver.BindAxis("jack_right", (value) => _jackInput.x = value);

        InputReciver.BindInputActionPressed("luck_interact", _luck.TryInteract);
        InputReciver.BindInputActionPressed("jack_attack", _jack.TryAttack);
    }

    public override void PossessedTick()
    {
        var luckPosition = _luck.transform.GetFlatPosition();
        var jackPosition = _jack.transform.GetFlatPosition();

        var distance = FlatVector.Distance(luckPosition, jackPosition);

        var virtualCameraTarget = Vector3.Lerp(jackPosition, luckPosition, 0.5f);

        CameraPosition = virtualCameraTarget - Vector3.forward * 8f + Vector3.up * 7.5f;
        CameraRotation = Quaternion.Euler(46.13f, 0f, 0f);

        _luck.Move(_luckInput);
        _jack.Move(_jackInput);
    }

}