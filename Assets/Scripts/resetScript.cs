using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class resetScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void reset()
	{
		if(Time.timeSinceLevelLoad>10)
		{
			Scene scene = SceneManager.GetActiveScene(); 
			SceneManager.LoadScene(scene.name);
		}
	}

	public void Max()
	{
		if(Time.timeSinceLevelLoad>10)
		{
			Debug.Log("max");
		}
	}

		public void Min()
	{
		if(Time.timeSinceLevelLoad>10)
		{
		Debug.Log("min");
	}
	}
}
