using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerPawn : Pawn
{

    [Inject] private Luck _luck;
    [Inject] private Jack _jack;

    private Vector3 _virtualCameraTarget;

    protected override void OnPawnStart()
    {
        /*
        InputReciver.BindAxis("moveForward", (value) => _cameraController.Move(new FlatVector(0, value)));
        InputReciver.BindAxis("moveRight", (value) => _cameraController.Move(new FlatVector(value, 0)));
        InputReciver.BindAxis("qe", (value) => _cameraController.Rotate((int)value));
        InputReciver.BindAxis("scroll", _cameraController.Zoom);

        InputReciver.BindInputActionPressed("mouse0", ClearSelection, _cameraController.StartRotating);
        InputReciver.BindInputActionRelesed("mouse0", _cameraController.StopRotating);

        InputReciver.BindInputActionPressed("mouse2", _cameraController.StartMoving, ClearSelection);
        InputReciver.BindInputActionRelesed("mouse2", _cameraController.StopMoving);

        InputReciver.BindInputActionPressed("mouse1", ClearSelection, Raycast);

        InputReciver.BindInputActionPressed("jump", SelectNextCharacter); // TODO: Change
        InputReciver.BindInputActionPressed("pause", _pauseMenu.Show, ClearSelection); 
        */
    }

    public override void PossessedTick()
    {
        var luckPosition = _luck.transform.GetFlatPosition();
        var jackPosition = _jack.transform.GetFlatPosition();
        var distance = FlatVector.Distance(luckPosition, jackPosition);
        _virtualCameraTarget = Vector3.Lerp(jackPosition, luckPosition, 0.5f);

        CameraPosition = _virtualCameraTarget + Vector3.forward * -3.5f + Vector3.up * 10f;
        CameraRotation = Quaternion.Euler(75f, 0f, 0f);
    }

}