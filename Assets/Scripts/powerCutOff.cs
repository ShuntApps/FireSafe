using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerCutOff : MonoBehaviour {

	public Material mat;

	// Use this for initialization
	void Start () {
		mat=GetComponent<Material>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[ContextMenu("turnOffPower")]
	public void turnOffPower()
	{
		mat.SetColor("_EmissionColor", Color.grey);
		NewEventManager.TriggerEvent("powerOff");
	}
}
