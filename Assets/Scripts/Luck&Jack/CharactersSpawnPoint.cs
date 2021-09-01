using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersSpawnPoint : MonoBehaviour
{

    [SerializeField] private float _distance;

    public FlatVector GetLuckSpawnPosition()
    {
        return (FlatVector)(transform.position + transform.forward * _distance);
    }

    public FlatVector GetJackSpawnPosition()
    {
        return (FlatVector)(transform.position - transform.forward * _distance);
    }

    private void OnDrawGizmosSelected()
    {
        Draw(GetLuckSpawnPosition(), Color.green);
        Draw(GetJackSpawnPosition(), Color.cyan);
    }

    private void Draw(FlatVector position, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(position, 0.25f);
        Gizmos.DrawRay(position, transform.up * 10f);
    }

}