﻿using System.Collections;
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

    public TextMeshProUGUI extinguisherSpectator;
    public TextMeshProUGUI pinSpectator;
    public TextMeshProUGUI alarmSpectator;
    public TextMeshProUGUI fuelLeftSpectator;
    public TextMeshProUGUI scoreTxtSpectator;

    public string name;
    public playerDataScriptableObject playerData;

	// Use this for initialization
	void Start () {
        name=playerData.playerName;
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
        Debug.Log("fire out");
        score += 2500;
        endTime = Time.timeSinceLevelLoad;
        if(!alarmPressed)
        {
            
              alarm.text="Forgot Alarm";
              alarm.color=Color.green;
            score -= 1500;
        }
        score -= (int)(endTime);
        if(wrongExtinguisherUsed!=true)
        {
            extinguisher.text="Correct Extinguisher";
            extinguisher.color=Color.green;
            
            extinguisherSpectator.color=Color.black;
        }
        scoreTxt.text=score.ToString();
        scoreTxt.color=Color.white;
        //scoreTxt.color=Color.white;
        //TextSerialisation textSerial = new TextSerialisation();
        //TextSerialisation.WriteFirstString(name,wrongExtinguisherUsed,forgotPinPull,alarmPressed,fuelLeft,score+"");
        //more processing of other data
    }

    void wrongExtinguisher()
    {
        extinguisher.color=Color.red;
        extinguisher.text="Wrong Extinguisher";
        
        extinguisherSpectator.color=Color.black;
        extinguisherSpectator.text="Wrong Extinguisher";
        //set vars to ensure flags can be set in GUI
        wrongExtinguisherUsed = true;
        score -= 1000;
    }

    void forgotPin()
    {
        pin.color=Color.red;
        pinSpectator.color=Color.red;
        forgotPinPull=true;
        score -= 500;
    }

    void extinguisherRanOut()
    {
        fuelLeft.color=Color.red;
        fuelLeftSpectator.color=Color.red;
        score -= 500;
    }

    void alarmIsPressed()
    {
        alarm.color=Color.green;
        alarmSpectator.color=Color.green;
        alarmPressed = true;
    }
}
