using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class AlarmVRTKInteractableObject : MonoBehaviour {

    public bool pressed;
    public AudioSource alarm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Test()
    {
        print("WTF");
    }

    public void Press()
    {
        
        if (!pressed)
        {
            alarm.Play();
            NewEventManager.TriggerEvent("alarmPressed");
            GetComponent<VRTK_InteractableObject>().enabled = false;
        }
        pressed = true;
            // GetComponent<VRTK_InteractableObject>().enabled = false;

    }

    protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        //print("fire");
    }

    protected virtual void InteractableObjectUnused(object sender, InteractableObjectEventArgs e)
    {

    }
}
