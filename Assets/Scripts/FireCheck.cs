using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCheck : MonoBehaviour {

    public bool allowWater;
    public bool allowFoam;
    public bool allowPowder;
    public bool allowCO2;
    public float targetScale = 0.1f;
    public float shrinkSpeed = 0.1f;
    public bool shrinking;

    public bool isElectrical = false;
    public bool isDemoFire;

    // Use this for initialization
    void Start () {
		
	}

    //Consider replacing with multiple scripts "ordinaryFire, Electrical Fire, chemical fire?"

    private void OnTriggerEnter(Collider other)
    {
        //print(other.tag);
        if(other.tag=="Water"&&allowWater)
        {
            if(isElectrical)
            {
                NewEventManager.TriggerEvent("WaterOnElectricOff");
            }
            ShrinkFire();
        }
        else if(other.tag=="Water"&&isElectrical)
        {
            waterOnElectric();
        }
        else if(other.tag=="Foam"&&allowFoam)
        {
            ShrinkFire();
        }
        else if(other.tag=="CO2")
        {
            blanketFire();
        }
    }

    [ContextMenu(" electricTest")]
    private void waterOnElectric()
    {
         for(int i=0;i<transform.childCount;i++)
            {
                transform.GetChild(i).localScale=new Vector3(3,3,3);
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (shrinking)
        {
            shrinkFire[] fireScripts = GetComponentsInChildren<shrinkFire>();
            foreach (shrinkFire fire in fireScripts)
            {
                //print(fire.gameObject);
                fire.stopShrink();
            }
        }
    }

    void blanketFire()
    {
        StartCoroutine("rapidOut");
    }

    IEnumerator rapidOut()
    {
        yield return new WaitForSeconds(3);
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in particles)
        {
            p.Stop();
        }
        if(!allowCO2)
        {
            NewEventManager.TriggerEvent("wrongExtinguisher");
            yield return new WaitForSeconds(5);
            foreach (ParticleSystem p in particles)
            {
                p.Play();
            }
        }
    }

    IEnumerator fireExpand()
    {
        yield return new WaitForSeconds(2);
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in particles)
        {
            //p.sca();
        }
    }

    void ShrinkFire()
    {
        shrinkFire[] fireScripts = GetComponentsInChildren<shrinkFire>();
        foreach(shrinkFire fire in fireScripts)
        {
            //print(fire.gameObject);
            fire.startShrink();
        }
    }

    void Extinguish()
    {
        ParticleSystem[] particles=GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem p in particles)
        {
            p.Stop();
        }

    }
}
