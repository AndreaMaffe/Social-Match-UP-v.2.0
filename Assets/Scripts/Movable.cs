using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Rotate(Vector3.right, -1f);
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.up, -1);
        if (Input.GetKey(KeyCode.S))
            transform.Rotate(Vector3.right, 1f);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up, 1f);
    }
}
