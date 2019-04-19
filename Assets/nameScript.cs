using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class nameScript : MonoBehaviour {

	public playerDataScriptableObject gameData;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setName(TextMeshProUGUI newNameTxt) {
		
		if(newNameTxt.text!="")
		{
			gameData.playerName=newNameTxt.text;
		}
	}


}
