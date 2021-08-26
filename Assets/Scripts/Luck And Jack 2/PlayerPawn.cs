using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerPawn : Pawn
{

    [Inject] private Luck _luck;
    [Inject] private Jack _jack;

    private Vector3 _virtualCameraTarget;
    private FlatVector _luckInput;

    protected override void OnPawnStart()
    {
        InputReciver.BindAxis("moveForward", (value) => _luckInput.z = value);
        InputReciver.BindAxis("moveRight", (value) => _luckInput.x = value);
    }

    public override void PossessedTick()
    {
        var luckPosition = _luck.transform.GetFlatPosition();
        var jackPosition = _jack.transform.GetFlatPosition();
        var distance = FlatVector.Distance(luckPosition, jackPosition);
        _virtualCameraTarget = Vector3.Lerp(jackPosition, luckPosition, 0.5f);

        CameraPosition = _virtualCameraTarget - Vector3.forward * 6.8f + Vector3.up * 7.5f;
        CameraRotation = Quaternion.Euler(46.13f, 0f, 0f);

        _luck.Move(_luckInput);
        _jack.Move(_luckInput);
    }

}