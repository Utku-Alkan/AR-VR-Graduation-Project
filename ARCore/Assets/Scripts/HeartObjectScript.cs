using UnityEngine;

public class HeartObjectScript : MonoBehaviour
{
    public float rotationSpeed = 1.0f;

    void Start()
    {
        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
