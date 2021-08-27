using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatsSpawnPoint : MonoBehaviour
{

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.DrawRay(transform.position, transform.up * 10f);
    }

}