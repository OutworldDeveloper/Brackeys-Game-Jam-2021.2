using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersSpawnPoint : MonoBehaviour
{

    [SerializeField] private float _distance;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.DrawRay(transform.position, transform.up * 10f);
    }

}