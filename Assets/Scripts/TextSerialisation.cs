using UnityEngine;
using UnityEditor;
using System.IO;

public class TextSerialisation
{

    public string extinguisher;
    public string pin;
    public string alarm;
    public string fuelLeft;
    public string scoreTxt;

 //[MenuItem("Tools/Write test")]
	static void WriteTest()
	{
		WriteFirstString("James Brown",true,true,true,true,"10000");
	}

   // [MenuItem("Tools/Write file")]
    public static void WriteFirstString(string name,bool extinguisher, bool pin, bool alarm, bool fuelLeft, string scoreTxt)
    {
        string path = "Assets/Resources/"+name+".txt";
		path = Application.persistentDataPath+"\\"+name+".txt";
		System.IO.File.WriteAllText(Application.persistentDataPath+"\\name.txt", "");

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Results \n"+name+"\nYour score was "+scoreTxt+"\n This is made up of:\n"+
		"\nCorrect Extinguisher used first time: "+extinguisher+"\n Remembered to pull pin: "+!pin+"\n Remembered to press the alarm: "+ alarm+
		"\n ran out of fuel in the extinguisher: "+!fuelLeft);

        writer.Close();

        //Re-import the file to update the reference in the editor
        //AssetDatabase.ImportAsset(path); 
        //TextAsset asset = (TextAsset)Resources.Load("test");

        //Print the text from the file
        //Debug.Log(asset.text);
    }

    public static void WriteSecondString(string name,bool extinguisher, bool pin, bool alarm, bool powerOff, bool fuelLeft, string scoreTxt)
    {
        string path = "Assets/Resources/"+name+"_"+UnityEngine.SceneManagement.SceneManager.GetActiveScene().name+".txt";
		path = Application.persistentDataPath+"\\"+name+".txt";
		System.IO.File.WriteAllText(Application.persistentDataPath+"\\name.txt", "");

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Results \n"+name+"\nYour score was "+scoreTxt+"\n This is made up of:\n"+
		"\nCorrect Extinguisher used first time: "+extinguisher+"\n Remembered to pull pin: "+!pin+"\n Turned off the power: "+powerOff
        +"\n Remembered to press the alarm: "+ alarm+"\n ran out of fuel in the extinguisher: "+!fuelLeft);

        writer.Close();

        //Re-import the file to update the reference in the editor
        //AssetDatabase.ImportAsset(path); 
        //TextAsset asset = (TextAsset)Resources.Load("test");

        //Print the text from the file
        //Debug.Log(asset.text);
    }

    //[MenuItem("Tools/Read file")]
    static void ReadString()
    {
        string path = Application.persistentDataPath+"\\James Brown.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path); 
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }

}