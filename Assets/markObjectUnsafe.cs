using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markObjectUnsafe : MonoBehaviour {

	static int amountPlaced;

	public enum hazardStates
	{
		palletsunsafe,blockedExit,waterunsafe,boxesunsafe,plugsunsafe,heatingunsafe,flammableunsafe
	};

	public hazardStates hazardState;
	public GameObject unsafeSticker;
	
	// Use this for initialization

	void Start () {
		unsafeSticker.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Mark()
	{
		Debug.Log("unsafe "+hazardState.ToString());
		unsafeSticker.SetActive(true);
		NewEventManager.TriggerEvent(hazardState.ToString());
	}

	public void FalseMark()
	{
		NewEventManager.TriggerEvent("wrongGuess");
	}
}
