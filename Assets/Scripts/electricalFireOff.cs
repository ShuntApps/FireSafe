using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electricalFireOff : MonoBehaviour {

	

   private void OnEnable()
    {
        NewEventManager.StartListening("fireOut", manageFire);
    }

    private void OnDisable()
    {
        NewEventManager.StopListening("fireOut", manageFire);
    }

	public void manageFire()
	{
		GetComponent<FireCheck>().enabled=true;
	}

	// Use this for initialization
	void Start () {
		GetComponent<FireCheck>().enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
