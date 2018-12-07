using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class binFireController : MonoBehaviour {

    float endTime;
    bool wrongExtinguisherUsed;
    bool forgotPinPull;
    int score=9000;
    bool alarmPressed;
    public TextMeshProUGUI extinguisher;
    public TextMeshProUGUI pin;
    public TextMeshProUGUI alarm;
    public TextMeshProUGUI fuelLeft;
    public TextMeshProUGUI scoreTxt;

	// Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void Update () {		
	}

    private void OnEnable()
    {
        NewEventManager.StartListening("fireOut", fireOut);
        NewEventManager.StartListening("forgotPin", forgotPin);
        NewEventManager.StartListening("wrongExtinguisher", wrongExtinguisher);
        NewEventManager.StartListening("alarmPressed", alarmIsPressed);
        NewEventManager.StartListening("extinguisherRanOut", extinguisherRanOut);
    }

    private void OnDisable()
    {
        NewEventManager.StopListening("fireOut", fireOut);
        NewEventManager.StopListening("forgotPin", forgotPin);
        NewEventManager.StopListening("wrongExtinguisher", wrongExtinguisher);
        NewEventManager.StopListening("alarmPressed", alarmIsPressed);
        NewEventManager.StopListening("extinguisherRanOut", extinguisherRanOut);
    }

    void fireOut()
    {
        score += 2500;
        endTime = Time.timeSinceLevelLoad;
        if(!alarmPressed)
        {
            score -= 1500;
        }
        score -= (int)(endTime);
        if(wrongExtinguisherUsed!=true)
        {
            extinguisher.color=Color.white;
        }
        scoreTxt.text=score.ToString();
        //more processing of other data
    }

    void wrongExtinguisher()
    {
        extinguisher.color=Color.red;
        //set vars to ensure flags can be set in GUI
        wrongExtinguisherUsed = true;
        score -= 1000;
    }

    void forgotPin()
    {
        pin.color=Color.red;
        forgotPinPull = true;
        score -= 500;
    }

    void extinguisherRanOut()
    {
        fuelLeft.color=Color.red;
        score -= 500;
    }

    void alarmIsPressed()
    {
        alarm.color=Color.white;
        alarmPressed = true;
    }
}
