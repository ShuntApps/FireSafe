using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class findHazardController : MonoBehaviour {


	public TextMeshProUGUI pallets;
	public TextMeshProUGUI blockedExit;
	public TextMeshProUGUI water;
	public TextMeshProUGUI boxes;
	public TextMeshProUGUI plugs;
	public TextMeshProUGUI heating;
	public TextMeshProUGUI flammable;
	public TextMeshProUGUI wrongCount;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI timeTxt;

    public bool palletsbool;
    public bool blockedExitbool;
    public bool waterbool;
    public bool boxesbool;
    public bool plugsbool;
    public bool heatingbool;
    public bool flammablebool;

    /**
    public TextMeshProUGUI extinguisherSpectator;
    public TextMeshProUGUI pinSpectator;
    public TextMeshProUGUI alarmSpectator;
    public TextMeshProUGUI fuelLeftSpectator;
    public TextMeshProUGUI scoreTxtSpectator;
    public TextMeshProUGUI nameTxtSpectator;*/

      
    public GameObject buttonCert;

    public string name;
    public playerDataScriptableObject playerData;

	int score;

	int numFound;

	int numright;

	// Use this for initialization
	void Start () {
		wrongCount.text=""+(numFound-numright);
        //nameTxtSpectator.text = "User: " + name;
        buttonCert.SetActive(false);

    }

    void Update()
    {
        scoreTxt.text = "Score: " + score;
        timeTxt.text = "Time: " + Time.timeSinceLevelLoad.ToString("N0");
    }

   [ContextMenu ("Test Cert")]
    public void testCert()
    {
        name="Al Parker";
	palletsbool=true;
   	blockedExitbool=true;
    waterbool=true;
    boxesbool=false;
    plugsbool=true;
    heatingbool=false;
    flammablebool=true;
        score=10266;
        generateCert();
    }

    public void generateCert()
    {
       // public void WriteThirdRoom(string name, bool pallets, bool blocked, bool water, bool boxes, bool plugs, bool heating, bool flammable, string scoreTxt)
		StopAllCoroutines();
        GetComponent<SimplePDF>().
            WriteThirdRoom(name, palletsbool, blockedExitbool, waterbool, boxesbool, plugsbool, heatingbool, flammablebool, scoreTxt.text);
       
    }

	private void OnEnable() {
		NewEventManager.StartListening("plugsunsafe",plugsMarked);
		
		NewEventManager.StartListening("palletsunsafe",palletsMarked);
		
		NewEventManager.StartListening("blockedExit",exitBlocked);
		
		NewEventManager.StartListening("waterunsafe",waterSpill);
		
		NewEventManager.StartListening("boxesunsafe",boxesUnsafe);

		NewEventManager.StartListening("flammableunsafe",flammables);

		NewEventManager.StartListening("heatingunsafe",plugsMarked);

		NewEventManager.StartListening("wrongGuess",wrongGuess);
	}

	void exitBlocked()
	{
		blockedExit.color=Color.green;
		numFound++;
		numright++;
		if(numFound>=6)
		{
			float timeScore = Time.timeSinceLevelLoad;
			score=(int)timeScore + (numright*1000);
		}
	}

	void waterSpill()
	{
		water.color=Color.green;
				numFound++;
				numright++;
		if(numFound>=6)
		{
			float timeScore = Time.timeSinceLevelLoad;
			score=(int)timeScore + (numright*1000);
		}
	}

	void flammables()
	{
		flammable.color=Color.green;
				numFound++;
				numright++;
		if(numFound>=6)
		{
			float timeScore = Time.timeSinceLevelLoad;
			score=(int)timeScore + (numright*1000);
		}
	}

	void boxesUnsafe()
	{
		boxes.color=Color.green;
				numFound++;
				numright++;
		if(numFound>=6)
		{
			float timeScore = Time.timeSinceLevelLoad;
			score=(int)timeScore + (numright*1000);
		}
	}
	
	private void OnDisable() {
		NewEventManager.StopListening("plugsunsafe",plugsMarked);
		
		NewEventManager.StopListening("palletsunsafe",palletsMarked);
		
		NewEventManager.StopListening("blockedExit",exitBlocked);
		
		NewEventManager.StopListening("waterunsafe",waterSpill);
		
		NewEventManager.StopListening("boxesunsafe",boxesUnsafe);

		NewEventManager.StopListening("flammableunsafe",flammables);

		NewEventManager.StopListening("heatingunsafe",plugsMarked);

		NewEventManager.StopListening("wrongGuess",wrongGuess);
	}

	void plugsMarked()
	{
		Debug.Log("plugs marked");
		plugs.color=Color.green;
				numFound++;
				numright++;
		if(numFound>=6)
		{
			float timeScore = Time.timeSinceLevelLoad;
			score=(int)timeScore + (numright*1000);
		}
	}

	void palletsMarked()
	{
		pallets.color=Color.green;
				numFound++;
				numright++;
		if(numFound>=6)
		{
			float timeScore = Time.timeSinceLevelLoad;
			score=(int)timeScore + (numright*1000);
		}
	}

	void wrongGuess()
	{
		score-=2500;
		numFound++;
		wrongCount.text = ""+(numFound-numright);
		if(numFound>=6)
		{
			float timeScore = Time.timeSinceLevelLoad;
			score=(int)timeScore + (numright*1000);
		}
	}
}
