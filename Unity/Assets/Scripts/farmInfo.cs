using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using SimpleJSON;

public class farmInfo : MonoBehaviour
{
    public InputField addressField;  
    public InputField areaField;    
    public static int farm_id = Int32.Parse(DBManager.farmId); 
    public static JSONArray jsonArray; 

    public Button submitButton;

    // Start is called before the first frame update
    IEnumerator Start()
    {
            addressField.text = "";
            areaField.text = "";   
            yield return getInfo();
            addressField.text = jsonArray[0].AsObject["address"];
            areaField.text = jsonArray[0].AsObject["area"];
    }
    
    IEnumerator getInfo()
    {
        farm_id = 1;
        WWWForm form = new WWWForm();
        form.AddField("farm_id", farm_id);
        
        WWW www = new WWW("http://localhost/sqlconnect/getFarmInfo.php", form);
        yield return www;

        string result = www.text;
        jsonArray = JSON.Parse(result) as JSONArray;
    }

    public void callSave()
    {
        StartCoroutine(Save());
    }

    IEnumerator Save()
    {
        farm_id = 1;
        WWWForm form = new WWWForm();
        form.AddField("farm_id", farm_id);
        form.AddField("location", addressField.text);
        form.AddField("description", areaField.text);

        WWW www = new WWW("http://localhost/sqlconnect/saveFarmInfo.php", form);
        yield return www;
        
        Debug.Log("Successfully updated!");
    }
}
