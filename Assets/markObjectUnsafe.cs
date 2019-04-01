using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markObjectUnsafe : MonoBehaviour {

	static int amountPlaced;
	bool marked;

	public enum hazardStates
	{
		palletsunsafe,blockedExit,waterunsafe,boxesunsafe,plugsunsafe,heatingunsafe,flammableunsafe,
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
		if(amountPlaced<6)
		{
			if(!marked)
			{
				marked=true;
				Debug.Log("unsafe "+hazardState.ToString());
				unsafeSticker.SetActive(true);
				NewEventManager.TriggerEvent(hazardState.ToString());
				amountPlaced++;
			}
		}
	}

	public void FalseMark()
	{
		if(amountPlaced<6)
		{
			if(!marked)
			{
				marked=true;
				unsafeSticker.SetActive(true);
				NewEventManager.TriggerEvent("wrongGuess");
				this.enabled=false;
				amountPlaced++;
			}
		}
	}	
}
