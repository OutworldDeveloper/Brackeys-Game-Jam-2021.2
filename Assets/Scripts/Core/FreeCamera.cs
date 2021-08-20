using UnityEngine;

public class FreeCamera : Pawn
{

    private float _rotationX;
    private float _rotationY;

    protected override void OnPawnStart()
    {
        InputReciver.BindAxis("qe", (value) => Move(0, value, 0));
        InputReciver.BindAxis("moveForward", (value) => Move(0, 0, value));
        InputReciver.BindAxis("moveRight", (value) => Move(value, 0, 0));
        InputReciver.BindAxis("mouseX", (value) => _rotationX += value * 5f);
        InputReciver.BindAxis("mouseY", (value) => _rotationY += value * 5f);
    }

    protected override void OnPossesesed()
    {
        _rotationX = CameraRotation.eulerAngles.y;
        _rotationY = -CameraRotation.eulerAngles.x;
    }

    public override void PossessedTick()
    {
        _rotationY = Mathf.Clamp(_rotationY, -85f, 85f);
        CameraRotation = Quaternion.Euler(-_rotationY, _rotationX, 0f);
    }

    private void Move(float x, float y, float z)
    {
        var direction = new Vector3(x, y, z);
        direction = PlayerController.PlayerCamera.transform.TransformDirection(direction);
        CameraPosition += direction * 7f * Time.unscaledDeltaTime;
    }

}
