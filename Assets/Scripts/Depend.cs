using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depend : MonoBehaviour
{
    public Transform dependObject;
    public bool x, y, z;

    private void Update()
    {
        Vector3 new_pos = dependObject.transform.position;
        if (!z) new_pos.z = 0;
        if (!x) new_pos.x = 0;
        if (!y) new_pos.y = 0;
        transform.position = new_pos;
    }
}
