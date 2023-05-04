using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSensor : MonoBehaviour
{
    
    public Light myLight;
    bool isPlayerInside;

    void Start()
    {
       
        myLight = GetComponent<Light>();
        myLight.enabled = false;
    }

    private void OnTriggerEnter(Collider character)
    {
        if (character.gameObject.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }
    private void OnTriggerExit(Collider character)
    {
        if (character.gameObject.CompareTag("Player"))
        {
            isPlayerInside = false;   
        }
    }
    private void Update()
    {
        if (isPlayerInside == true) 
        {
            myLight.enabled = true;
        } else
        {
            myLight.enabled = false;
        }
    }

}
