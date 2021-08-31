using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TutorialWall : MonoBehaviour
{

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        Disable();
    }

    public void Enable()
    {
        _collider.enabled = true;
    }

    public void Disable()
    {
        _collider.enabled = false;
    }

}