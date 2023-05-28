using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObjectScript : MonoBehaviour
{

    private Rigidbody rb;
    private float jumpForce = 4f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnPointerEnter()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }



    public void OnPointerExit()
    {
    }


    public void OnPointerClick()
    {
        rb.AddForce(Vector3.up * jumpForce * 3, ForceMode.Impulse);
    }
}
