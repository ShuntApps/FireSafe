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
    bool fuelLeftBool;
    bool powerOffBool;


    public TextMeshProUGUI extinguisher;
    public TextMeshProUGUI pin;
    public TextMeshProUGUI alarm;
    public TextMeshProUGUI fuelLeft;
	public TextMeshProUGUI powerOffLbl;
    public TextMeshProUGUI scoreTxt;

    public TextMeshProUGUI extinguisherSpectator;
    public TextMeshProUGUI pinSpectator;
    public TextMeshProUGUI alarmSpectator;
    public TextMeshProUGUI fuelLeftSpectator;
    public TextMeshProUGUI powerOffSpectator;
    public TextMeshProUGUI scoreTxtSpectator;
    public TextMeshProUGUI nameTxtSpectator;

    public GameObject buttonCert;

    public string name;
    public playerDataScriptableObject playerData;

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
        print("wrong Extinguisher used");
		extinguisher.color=Color.red;
        extinguisherSpectator.color = Color.red;
        //set vars to ensure flags can be set in GUI
        wrongExtinguisherUsed = true;
        wrongExtinguisher();
        score -= 1000;
	}

	public void powerOff()
	{
        powerOffBool = true;
		powerOffLbl.color=Color.green;
        powerOffSpectator.color = Color.green;
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
        scoreTxt.color=Color.white;
        //more processing of other data

        buttonCert.SetActive(true);
       
    }
      void Start()
      {
          name = playerData.playerName;
          nameTxtSpectator.text = "User: " + name;
          buttonCert.SetActive(false);
          fuelLeftBool = true;
      }

    public void generateCert()
    {
        GetComponent<SimplePDF>().
        WriteSecondRoom(name, wrongExtinguisherUsed, forgotPinPull, alarmPressed, powerOffBool, fuelLeftBool, score + "");
    }

    void wrongExtinguisher()
    {
        extinguisher.color=Color.red;
        extinguisherSpectator.color = Color.red;
        //set vars to ensure flags can be set in GUI
        wrongExtinguisherUsed = true;
        score -= 1000;
    }

    void forgotPin()
    {
        pin.color=Color.red;
        pinSpectator.color = Color.red;
        forgotPinPull = true;
        score -= 500;
    }

    void extinguisherRanOut()
    {
        fuelLeftBool = false;
        fuelLeft.color=Color.red;
        fuelLeftSpectator.color = Color.red;
        score -= 500;
    }

    void alarmIsPressed()
    {
        alarm.color=Color.green;
        alarmSpectator.color = Color.green;
        alarmPressed = true;
    }

    public void pinpulled()
    {
        pinSpectator.color=Color.green;
         pin.color=Color.green;
    }
}
