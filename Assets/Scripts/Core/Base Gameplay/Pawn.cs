using System.Collections;
using UnityEngine;
using Zenject;

public abstract class Pawn : MonoBehaviour
{
    public virtual bool ShowCursor => false;
    public virtual bool DestroyWhenUnpossessed => false; // Test
    public PlayerController PlayerController { get; private set; }
    public Vector3 CameraPosition { get; protected set; }
    public Quaternion CameraRotation { get; protected set; }

    public readonly InputReciver InputReciver = new InputReciver(false);

    private void Start()
    {
        OnPawnStart();
    }

    public virtual UI_BaseHud CreateHud() => null;

    public void OnPossesesed(PlayerController playerController) 
    {
        PlayerController = playerController;
        CameraPosition = playerController.PlayerCamera.transform.position;
        CameraRotation = playerController.PlayerCamera.transform.rotation;
        OnPossesesed();
    }

    public void OnUnpossessed(PlayerController playerController) 
    {
        PlayerController = null;
        OnUnpossessed();
        if (DestroyWhenUnpossessed)
        {
            Destroy(gameObject);
        }
    }

    public virtual void PossessedTick() { }
    protected virtual void OnPossesesed() { }
    protected virtual void OnUnpossessed() { }
    protected virtual void OnPawnStart() { }

}