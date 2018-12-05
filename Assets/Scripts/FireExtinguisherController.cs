using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisherController : MonoBehaviour
{
    public bool extinguisherActive = false;
    public bool extinguisherDead = false;

    [SerializeField] ParticleSystem outputParticles;
    [SerializeField] GameObject capsule;
    Animator anim;
    [SerializeField] bool isUsable;
    [SerializeField] bool CO2;
    public float timeLeft=2;

    void Start()
    {
        capsule.SetActive(false);
        anim = GetComponent<Animator>();
        outputParticles.Stop();
        GetComponent<VRTK.VRTK_InteractHaptics>().enabled = false;
    }

    private void Update()
    {
        if(extinguisherActive)
        {
            timeLeft -= Time.deltaTime;
        }
        if(timeLeft<=0)
        {
            //extinguisherDead = true;
            NewEventManager.TriggerEvent("extinguisherRanOut");
            ToggleExtinguisher(false);
            isUsable = false;
        }
    }

    [ContextMenu("activateExtinguisher")]

    public void activateExtinguisher()
    {
        isUsable = true;
        GetComponent<VRTK.VRTK_InteractHaptics>().enabled = true;
    }

    [ContextMenu ("ToggleExtinguisher")]

    void ToggleExtinguisherTrue()
    {
        ToggleExtinguisher(true);
    }

    public void ToggleExtinguisher(bool newStatus)
    {
        if (extinguisherActive != newStatus&&isUsable)
        {
            extinguisherActive = newStatus;

            if (extinguisherActive)
            {
                capsule.SetActive(true);
                outputParticles.Play();
                anim.SetBool("ExtinguisherActive", true);
                
            }
            else
            {
                capsule.SetActive(false);
                outputParticles.Stop();
                anim.SetBool("ExtinguisherActive", false);
            }
        }
        else if(!isUsable)
        {
            NewEventManager.TriggerEvent("forgotPin");
        }
    }
}
