using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float angularSpeed;

    void LateUpdate()
    {
        transform.rotation = new Quaternion();
        transform.Rotate(new(0f, 0f, 1f), angularSpeed * Time.time);
    }
}
