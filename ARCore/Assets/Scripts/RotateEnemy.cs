using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEnemy : MonoBehaviour
{
    public float rotationSpeed;

    void Update()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

    }
}
