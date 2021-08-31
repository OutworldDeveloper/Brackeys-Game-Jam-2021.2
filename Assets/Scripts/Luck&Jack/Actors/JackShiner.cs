using UnityEngine;

public class JackShiner : MonoBehaviour
{

    [SerializeField] private float _deathZone = 1.5f;
    [SerializeField] private float _range = 8f;
    [SerializeField] private float _angle = 40f;
    [SerializeField] private Light _visualLight;

    private bool _isShining;

    private void Start()
    {
        _visualLight.enabled = false;
    }

    public void EnableShining()
    {
        _isShining = true;
        _visualLight.enabled = true;
    }

    public void DisableShining()
    {
        _isShining = false;
        _visualLight.enabled = false;
    }

    public bool IsInLight(Transform other)
    {
        if (_isShining == false)
            return false;

        var distance = FlatVector.Distance(transform.position, other.position);

        if (distance < _deathZone)
            return false;

        if (distance > _range)
            return false;

        FlatVector targetDirection = (FlatVector)(other.position - transform.position);
        float angle = FlatVector.Angle(targetDirection, (FlatVector)transform.forward);

        return angle < _angle;
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Transform target = transform;

        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(target.transform.position, target.transform.up, 8f);
        UnityEditor.Handles.DrawWireDisc(target.transform.position, target.transform.up, 2.5f);
        UnityEditor.Handles.color = Color.green;

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(
            target.transform.position + Quaternion.AngleAxis(-_angle, Vector3.up) * target.transform.forward * _deathZone,
            target.transform.position + Quaternion.AngleAxis(-_angle, Vector3.up) * target.transform.forward * _range);

        Gizmos.DrawLine(
            target.transform.position + Quaternion.AngleAxis(_angle, Vector3.up) * target.transform.forward * _deathZone,
            target.transform.position + Quaternion.AngleAxis(_angle, Vector3.up) * target.transform.forward * _range);
    }

#endif

}