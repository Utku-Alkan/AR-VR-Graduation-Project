using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{

    [SerializeField] private Text mission;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public void changeTextToSuccess()
    {
        mission.text = "You have successfully found it!";
    }

    public void changeTextToFail()
    {
        mission.text = "You lost it. Find the JavaScript code.";
    }

    public void changeTextToSpecific(string message)
    {
        mission.text = message;
    }
}
