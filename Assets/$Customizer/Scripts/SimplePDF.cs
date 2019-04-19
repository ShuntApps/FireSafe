using 	UnityEngine;
using 	System.Collections;
using 	System.IO;
using	sharpPDF;
using	sharpPDF.Enumerators;
using System;



public class SimplePDF : MonoBehaviour {
	
	internal	string		attacName	= "SimplePDFTest.pdf";
	public string extinguisher;
    public string pin;
    public string alarm;
    public string fuelLeft;
    public string scoreTxt;

    public string powerOff;

	public string nameTxt;

    public string pallets;
    public string blocked;
    public string water;
    public string boxes;
    public string plugs;
    public string heating;
    public string flammables;

	public void WriteSecondRoom(string name,bool extinguisher, bool pin, bool alarm, bool powerOff, bool fuelLeft, string scoreTxt)
	{
	//forgot Pin will be true if forgot
		//extinguisher ran out would be true if ran out 
		//alam pressed would be true if alarm had been pressed 
		//extinguisher would be true if wrong extinguisher - invert
		//power off would be true if power was off - 

		nameTxt=name;
		this.extinguisher=extinguisher.ToString();
		this.pin=pin.ToString();
		this.alarm=alarm.ToString();
		this.powerOff=powerOff.ToString();
		this.fuelLeft=fuelLeft.ToString();
		this.scoreTxt=scoreTxt;
		StartCoroutine ( CreateSecondRoomPDF() );
	}

    public void WriteThirdRoom(string name, bool pallets, bool blocked, bool water, bool boxes, bool plugs, bool heating, bool flammable, string scoreTxt)
    {
        nameTxt = name;
        this.pallets = pallets.ToString();
        this.blocked = blocked.ToString();
        this.water = water.ToString();
        this.boxes = boxes.ToString();
        this.plugs = plugs.ToString();
        this.heating = heating.ToString();
        this.flammables = flammable.ToString();
        this.scoreTxt = scoreTxt;
        StartCoroutine(CreateSecondRoomPDF());
    }

	public void WriteFirstRoom(string name,bool extinguisher, bool pin, bool alarm, bool fuelLeft, string scoreTxt)
	{
	//forgot Pin will be true if forgot
		//extinguisher ran out would be true if ran out 
		//alam pressed would be true if alarm had been pressed 
		//extinguisher would be true if wrong extinguisher - invert
		//power off would be true if power was off - 

		nameTxt=name;
		this.extinguisher=extinguisher.ToString();
		this.pin=pin.ToString();
		this.alarm=alarm.ToString();
		this.fuelLeft=fuelLeft.ToString();
		this.scoreTxt=scoreTxt;
		StartCoroutine ( CreateFirstRoomPDF() );
	}
 

	// Use this for initialization
	void Start () {
		//nameTxt="test";
		//yield return StartCoroutine ( CreateSecondRoomPDF() );
		//yield return StartCoroutine ( CreatePDF() );
	}


	public Texture2D medal;

    public IEnumerator CreateThirdRoomPDF()
    {
        pdfDocument myDoc = new pdfDocument(name + " Spotting FireHazards ", "FireSafeVR", false);
        pdfPage myFirstPage = myDoc.addPage();
        Debug.Log('\u263A');
        myFirstPage.drawRectangle(20, 20, 822, 575, predefinedColor.csBlack, predefinedColor.csWhite);
        myFirstPage.addParagraph("FireSafeVR", 30, 530, predefinedFont.csTimesBold, 55, 800, 20, predefinedAlignment.csCenter, new pdfColor(predefinedColor.csDarkRed));
        myFirstPage.addParagraph("Certificate of Completion", 30, 480, predefinedFont.csTimesBold, 55, 800, 20, predefinedAlignment.csCenter, new pdfColor(predefinedColor.csDarkRed));
        myFirstPage.addParagraph("Spotting Fire Hazards", 30, 410, predefinedFont.csTimes, 30, 800, 20, predefinedAlignment.csCenter, new pdfColor(predefinedColor.csDarkBlue));

        myFirstPage.addParagraph("User: " + nameTxt, 30, 360, predefinedFont.csTimes, 22, 800, 20, predefinedAlignment.csCenter);
        myFirstPage.addParagraph("Your score was: " + scoreTxt, 30, 310, predefinedFont.csTimes, 22, 800, 20, predefinedAlignment.csCenter);
        myFirstPage.addParagraph("Hazards Found and missed: ", 30, 280, predefinedFont.csTimes, 22, 800, 20, predefinedAlignment.csCenter);
        myFirstPage.addParagraph("Blocked fire exit: " + blocked, 30, 240, predefinedFont.csTimes, 22, 800, 20, predefinedAlignment.csCenter);
        myFirstPage.addParagraph("Chained and Overloaded plugs: " + plugs, 30, 210, predefinedFont.csTimes, 22, 800, 20, predefinedAlignment.csCenter);
        myFirstPage.addParagraph("Flammables unsecured: " + flammables, 30, 180, predefinedFont.csTimes, 22, 800, 20, predefinedAlignment.csCenter);
        myFirstPage.addParagraph("Water near electricals: " + water, 30, 150, predefinedFont.csTimes, 22, 800, 20, predefinedAlignment.csCenter);
        myFirstPage.addParagraph("Boxes discarded : " + boxes, 30, 120, predefinedFont.csTimes, 22, 800, 20, predefinedAlignment.csCenter);
        myFirstPage.addParagraph("Blocked heating: " + heating , 30, 90, predefinedFont.csTimes, 22, 800, 20, predefinedAlignment.csCenter);
        myFirstPage.addParagraph("Pallets discarded: " + pallets, 30, 90, predefinedFont.csTimes, 22, 800, 20, predefinedAlignment.csCenter);

        string str = "FireSafe_Hazards_" + nameTxt + ".PDF";
        str = str.Replace(" ", String.Empty);

        string
            //path = "file://"+Path.Combine(Application.streamingAssetsPath, "FireSafe_Electrical_"+str);
        path = Path.Combine(Application.persistentDataPath, "\\" + str + "");

        Debug.Log(path);
        //myFirstPage.addText("Ran out of fuel in the extinguisher",250,400,predefinedFont.csTimes,18,new pdfColor(predefinedColor.csBlack));

        //yield return StartCoroutine ( myFirstPage.newAddImageByTexture (medal,250,400 ) );
        //yield return StartCoroutine ( );
        //myFirstPage.addImage(ImageConversion.EncodeToPNG(medal),250,400,512,512);
        //myFirstPage.addImage(medal.GetRawTextureData(),250,400,512,512);

        yield return new WaitForSeconds(1);
        myDoc.createPDF(str);
    }

	public IEnumerator CreateSecondRoomPDF () {
		pdfDocument myDoc = new pdfDocument(name+" Electrical Fires ","FireSafeVR", false);
		pdfPage myFirstPage = myDoc.addPage();
		Debug.Log('\u263A');
		myFirstPage.drawRectangle(20,20,822,575,predefinedColor.csBlack,predefinedColor.csWhite);
		myFirstPage.addParagraph("FireSafeVR",30,530,predefinedFont.csTimesBold,55,800,20,predefinedAlignment.csCenter,new pdfColor(predefinedColor.csDarkRed));
		myFirstPage.addParagraph("Certificate of Completion",30,480,predefinedFont.csTimesBold,55,800,20,predefinedAlignment.csCenter,new pdfColor(predefinedColor.csDarkRed));
		myFirstPage.addParagraph("Electrical Fire Safety",30,410,predefinedFont.csTimes,30,800,20,predefinedAlignment.csCenter,new pdfColor(predefinedColor.csDarkBlue));

		myFirstPage.addParagraph("User: "+nameTxt,30,360,predefinedFont.csTimes,22,800,20,predefinedAlignment.csCenter);
		myFirstPage.addParagraph("Your score was: "+scoreTxt,30,310,predefinedFont.csTimes,22,800,20,predefinedAlignment.csCenter);
		myFirstPage.addParagraph("Correct Extinguisher used first time: "+extinguisher,30,280,predefinedFont.csTimes,22,800,20,predefinedAlignment.csCenter);
		myFirstPage.addParagraph("Forgot to pull the pin: "+pin,30,240,predefinedFont.csTimes,22,800,20,predefinedAlignment.csCenter);
		myFirstPage.addParagraph("Sounded the alarm first: "+alarm,30,200,predefinedFont.csTimes,22,800,20,predefinedAlignment.csCenter);
		myFirstPage.addParagraph("Ran out of fuel in the extinguisher: "+fuelLeft,30,160,predefinedFont.csTimes,22,800,20,predefinedAlignment.csCenter);
		myFirstPage.addParagraph("Turned the power off: "+powerOff,30,120,predefinedFont.csTimes,22,800,20,predefinedAlignment.csCenter);
		


		string str = "FireSafe_Electrical_"+nameTxt+".PDF";
		str = str.Replace(" ", String.Empty);

     	string 
		 //path = "file://"+Path.Combine(Application.streamingAssetsPath, "FireSafe_Electrical_"+str);
		path = Path.Combine(Application.persistentDataPath,"\\"+str+"");

		Debug.Log(path);
		//myFirstPage.addText("Ran out of fuel in the extinguisher",250,400,predefinedFont.csTimes,18,new pdfColor(predefinedColor.csBlack));

		//yield return StartCoroutine ( myFirstPage.newAddImageByTexture (medal,250,400 ) );
		//yield return StartCoroutine ( );
		//myFirstPage.addImage(ImageConversion.EncodeToPNG(medal),250,400,512,512);
		//myFirstPage.addImage(medal.GetRawTextureData(),250,400,512,512);
	
		yield return new WaitForSeconds(1);
		myDoc.createPDF(str);
	}

	public IEnumerator CreateFirstRoomPDF () {
		pdfDocument myDoc = new pdfDocument(name+" Office Fires ","FireSafeVR", false);
		pdfPage myFirstPage = myDoc.addPage();
		Debug.Log('\u263A');
		myFirstPage.drawRectangle(20,20,822,575,predefinedColor.csBlack,predefinedColor.csWhite);
		myFirstPage.addParagraph("FireSafeVR",30,530,predefinedFont.csTimesBold,55,800,20,predefinedAlignment.csCenter,new pdfColor(predefinedColor.csDarkRed));
		myFirstPage.addParagraph("Certificate of Completion",30,480,predefinedFont.csTimesBold,55,800,20,predefinedAlignment.csCenter,new pdfColor(predefinedColor.csDarkRed));
		myFirstPage.addParagraph("Office Fire Safety",30,410,predefinedFont.csTimes,30,800,20,predefinedAlignment.csCenter,new pdfColor(predefinedColor.csDarkBlue));

		myFirstPage.addParagraph("User: "+nameTxt,30,360,predefinedFont.csTimes,22,800,20,predefinedAlignment.csCenter);
		myFirstPage.addParagraph("Your score was: "+scoreTxt,30,310,predefinedFont.csTimes,22,800,20,predefinedAlignment.csCenter);
		myFirstPage.addParagraph("Correct Extinguisher used first time: "+extinguisher,30,280,predefinedFont.csTimes,22,800,20,predefinedAlignment.csCenter);
		myFirstPage.addParagraph("Forgot to pull the pin: "+pin,30,240,predefinedFont.csTimes,22,800,20,predefinedAlignment.csCenter);
		myFirstPage.addParagraph("Sounded the alarm first: "+alarm,30,200,predefinedFont.csTimes,22,800,20,predefinedAlignment.csCenter);
		myFirstPage.addParagraph("Ran out of fuel in the extinguisher: "+fuelLeft,30,160,predefinedFont.csTimes,22,800,20,predefinedAlignment.csCenter);
	


		string str = "FireSafe_Office_"+nameTxt+".PDF";
		str = str.Replace(" ", String.Empty);

     	string 
		 //path = "file://"+Path.Combine(Application.streamingAssetsPath, "FireSafe_Electrical_"+str);
		path = Path.Combine(Application.persistentDataPath,"\\"+str+"");

		Debug.Log(path);
		//myFirstPage.addText("Ran out of fuel in the extinguisher",250,400,predefinedFont.csTimes,18,new pdfColor(predefinedColor.csBlack));

		//yield return StartCoroutine ( myFirstPage.newAddImageByTexture (medal,250,400 ) );
		//yield return StartCoroutine ( );
		//myFirstPage.addImage(ImageConversion.EncodeToPNG(medal),250,400,512,512);
		//myFirstPage.addImage(medal.GetRawTextureData(),250,400,512,512);
	
		yield return new WaitForSeconds(1);
		myDoc.createPDF(str);
	}

	
	
	// Update is called once per frame
	public IEnumerator CreatePDF () {
		pdfDocument myDoc = new pdfDocument("Sample Application","Me", false);
		pdfPage myFirstPage = myDoc.addPage();
		
//		Debug.Log ( "Continue to create PDF");
		myFirstPage.addText("Test Driving",10,730,predefinedFont.csHelveticaOblique,30,new pdfColor(predefinedColor.csOrange));	
		
		/*Table's creation*/
		pdfTable myTable = new pdfTable();
		//Set table's border
		myTable.borderSize = 1;
		myTable.borderColor = new pdfColor(predefinedColor.csDarkBlue);
		
		/*Add Columns to a grid*/
		myTable.tableHeader.addColumn(new pdfTableColumn("Model",predefinedAlignment.csRight,120));
		myTable.tableHeader.addColumn(new pdfTableColumn("Speed",predefinedAlignment.csCenter,120));
		myTable.tableHeader.addColumn(new pdfTableColumn("Weight",predefinedAlignment.csLeft,150));
		myTable.tableHeader.addColumn(new pdfTableColumn("Color",predefinedAlignment.csLeft,150));
		
		
		pdfTableRow myRow = myTable.createRow();
		myRow[0].columnValue = "A";
		myRow[1].columnValue = "100 km/h";
		myRow[2].columnValue = "180Kg";
		myRow[3].columnValue = "Orange";
		
		myTable.addRow(myRow);
		
		pdfTableRow myRow1 = myTable.createRow();
		myRow1[0].columnValue = "B";
		myRow1[1].columnValue = "130 km/h";
		myRow1[2].columnValue = "150Kg";
		myRow1[3].columnValue = "Yellow";
		
		myTable.addRow(myRow1);
		
		

		/*Set Header's Style*/
		myTable.tableHeaderStyle = new pdfTableRowStyle(predefinedFont.csCourierBoldOblique,12,new pdfColor(predefinedColor.csBlack),new pdfColor(predefinedColor.csLightOrange));
		/*Set Row's Style*/
		myTable.rowStyle = new pdfTableRowStyle(predefinedFont.csCourier,8,new pdfColor(predefinedColor.csBlack),new pdfColor(predefinedColor.csWhite));
		/*Set Alternate Row's Style*/
		myTable.alternateRowStyle = new pdfTableRowStyle(predefinedFont.csCourier,8,new pdfColor(predefinedColor.csBlack),new pdfColor(predefinedColor.csLightYellow));
		/*Set Cellpadding*/
		myTable.cellpadding = 10;
		/*Put the table on the page object*/
		myFirstPage.addTable(myTable, 5, 700);
		
		
		yield return StartCoroutine ( myFirstPage.newAddImage (  "FILE://picture1.jpg",2,100 ) );
		//yield return new WaitForSeconds(1);
		myDoc.createPDF(attacName);
		myTable = null;
	}
}
