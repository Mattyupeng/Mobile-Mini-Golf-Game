using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 Rotationspeed;

    void Update()
    {
        transform.Rotate(Rotationspeed * Time.deltaTime, Space.Self);
    }
}
