using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrinkFire : MonoBehaviour {


    public float targetScale = 0.1f;
    public float shrinkSpeed = 0.1f;
    public bool shrinking=false;

    // Use this for initialization
    void Start () {
		
	}

    // Update is cal    led once per frame
    void Update()
    {
        if (shrinking)
        {   
            transform.localScale -= Vector3.one * Time.deltaTime * shrinkSpeed;
            if (transform.localScale.x < targetScale)
            {
                StartCoroutine("stopFire");
            }
        }
    }

    public void startShrink()
    {
        shrinking = true;
    }

    public void stopShrink()
    {
        shrinking = true;
    }

    IEnumerator stopFire()
    {
        GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(1);
        NewEventManager.TriggerEvent("fireOut");
        Destroy(gameObject);
    }
}
