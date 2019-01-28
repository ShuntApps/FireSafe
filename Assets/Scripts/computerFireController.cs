using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class computerFireController : MonoBehaviour {


    float endTime;
    bool wrongExtinguisherUsed;
    bool forgotPinPull;
    int score=9000;
    bool alarmPressed;
    public TextMeshProUGUI extinguisher;
    public TextMeshProUGUI pin;
    public TextMeshProUGUI alarm;
    public TextMeshProUGUI fuelLeft;

	public TextMeshProUGUI powerOffLbl;
    public TextMeshProUGUI scoreTxt;

	/**
	* Set up event manager to list for Power off, Fire out, Alarm pressed, pin pulled, right extinguisher used	
	 */

	// Use this for initialization

	 private void OnEnable()
    {
        NewEventManager.StartListening("powerOff", powerOff);
		NewEventManager.StartListening("WaterOnElectricOff",waterOnElectric);
    
        NewEventManager.StartListening("fireOut", fireOut);
        NewEventManager.StartListening("forgotPin", forgotPin);
        NewEventManager.StartListening("wrongExtinguisher", wrongExtinguisher);
        NewEventManager.StartListening("alarmPressed", alarmIsPressed);
        NewEventManager.StartListening("extinguisherRanOut", extinguisherRanOut);
    }
    private void OnDisable()
    {
       NewEventManager.StopListening("powerOff", powerOff);
	   NewEventManager.StopListening("WaterOnElectricOff",waterOnElectric);
        NewEventManager.StopListening("fireOut", fireOut);
        NewEventManager.StopListening("forgotPin", forgotPin);
        NewEventManager.StopListening("wrongExtinguisher", wrongExtinguisher);
        NewEventManager.StopListening("alarmPressed", alarmIsPressed);
        NewEventManager.StopListening("extinguisherRanOut", extinguisherRanOut);
    }

	public void waterOnElectric()
	{
		extinguisher.color=Color.red;
        //set vars to ensure flags can be set in GUI
        wrongExtinguisherUsed = true;
        score -= 1000;
	}

	public void powerOff()
	{
		powerOffLbl.color=Color.green;
		score+=1000;
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
        alarm.color=Color.green;
        alarmPressed = true;
    }
}
