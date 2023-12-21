using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableProperties : MonoBehaviour
{
    public bool isRare = false; 
    // Start is called before the first frame update
    void Start()
    {
        if (isRare)
        {
            // Make the collectable 3 times bigger if it's rare
            transform.localScale *= 3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
