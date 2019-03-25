using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class findHazardController : MonoBehaviour {


	public TextMeshProUGUI pallets;
	public TextMeshProUGUI blockedExit;
	public TextMeshProUGUI water;
	public TextMeshProUGUI boxes;
	public TextMeshProUGUI plugs;
	public TextMeshProUGUI heating;
	public TextMeshProUGUI flammable;

	int score;
	
	 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
// /		palletsunsafe,blockedExit,waterunsafe,boxesunsafe,plugsunsafe,heatingunsafe,flammableunsafe

	private void OnEnable() {
		NewEventManager.StartListening("plugsunsafe",plugsMarked);
		
		NewEventManager.StartListening("palletsunsafe",palletsMarked);
		
		NewEventManager.StartListening("blockedExit",exitBlocked);
		
		NewEventManager.StartListening("waterunsafe",waterSpill);
		
		NewEventManager.StartListening("boxesunsafe",boxesUnsafe);

		NewEventManager.StartListening("flammableunsafe",flammables);

		NewEventManager.StartListening("heatingunsafe",plugsMarked);

		NewEventManager.StartListening("wrongGuess",wrongGuess);
	}

	void exitBlocked()
	{
		blockedExit.color=Color.green;
	}

	void waterSpill()
	{
		water.color=Color.green;
	}

	void flammables()
	{
		flammable.color=Color.green;
	}

	void boxesUnsafe()
	{
		boxes.color=Color.green;
	}
	
	private void OnDisable() {
		
	}

	void plugsMarked()
	{
		Debug.Log("plugs marked");
		plugs.color=Color.green;
	}

	void palletsMarked()
	{
		pallets.color=Color.green;
	}

	void wrongGuess()
	{
		score-=2500;
	}
}
