using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playButtonAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	[ContextMenu("turnOffPower")]
	public void playAnim()
	{
		GetComponent<Animation>().Play();
	}
}
