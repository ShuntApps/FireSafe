using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alarmShatter : MonoBehaviour {

	public GameObject brokenModel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Break()
	{
		brokenModel.SetActive(true);

        foreach (Transform item in brokenModel.transform)
        {
            item.GetComponent<Rigidbody>().AddExplosionForce(2f, transform.position, 1f);
        }

		Destroy(brokenModel,5);
            
	}
}
