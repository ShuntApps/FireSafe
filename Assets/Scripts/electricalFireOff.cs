using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electricalFireOff : MonoBehaviour {

	

   private void OnEnable()
    {
        NewEventManager.StartListening("powerOff", manageFire);
    }

    private void OnDisable()
    {
        NewEventManager.StopListening("powerOff", manageFire);
    }

	public void manageFire()
	{
		GetComponent<FireCheck>().enabled=true;
		GetComponent<FireCheck>().allowWater=true;
	}

	// Use this for initialization
	void Start () {
		GetComponent<FireCheck>().enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
