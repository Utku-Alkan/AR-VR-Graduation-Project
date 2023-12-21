using UnityEngine;

public class HeartObjectScript : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationSpeed = 30.0f;

    void Start()
    {
        // Assuming 'cameraR' is the main camera you use for AR
        cameraTransform = GameObject.Find("AR Session Origin/cameraR").transform;
    }


    private float rotationDirection = 1.0f; // Direction of rotation
    private float maxRotationAngle = 45.0f; // Maximum rotation angle from the center

    void Update()
    {
        // Rotate the heart object to always face the user
        if(cameraTransform != null)
        {
            transform.LookAt(cameraTransform);
        }

        // Adjust the X-axis rotation to be 90 degrees
        Vector3 rotation = transform.eulerAngles;
        rotation.x = -90; // Set the X-axis rotation to 90 degrees

        // Apply the adjusted rotation back to the transform
        transform.eulerAngles = rotation;

        // Apply an additional rotation from left to right
        float rotationZ = maxRotationAngle * Mathf.Sin(Time.time * rotationSpeed);
        transform.Rotate(0, 0, rotationZ * rotationDirection * Time.deltaTime);
    }
}
