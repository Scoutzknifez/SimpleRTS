using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSpawnArea : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
