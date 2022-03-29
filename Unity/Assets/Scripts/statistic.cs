using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using SimpleJSON;

public class statistic : MonoBehaviour
{
    public InputField dateField;    
    public GameObject canvasRef;
    public GameObject prefabStat;
    public static int farm_id = Int32.Parse(DBManager.farmId); 
    public static JSONArray jsonArray; 
    public static string dateShow = DateTime.UtcNow.ToString("yyyy-MM-dd");

    public static int statNumber;
    public static GameObject[] goStat;

    public Button submitButton;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        // button choose date 
            // dateShow = "2022-03-27";
            dateField.text = dateShow;
            yield return run();
    }

    IEnumerator run() {
        // farm_id && dateShow
        // receive from php
            yield return getStat();
            
            int statNumber = jsonArray.Count;
            //
                // time[0] = "10:12:54";
                // time[1] = "18:01:12";

                // temperature[0] = "23.45";
                // temperature[1] = "26.13";

                // soil_moisture[0] = "13.12";
                // soil_moisture[1] = "45.12";

                // pump_status[0] = "ON";
                // pump_status[1] = "OFF";

        goStat = new GameObject[statNumber];
        // goStat = new (GameObject)Instantiate(prefabstat) [statNumber];
        for (int i = 0; i < statNumber; i++) {
            goStat[i] = (GameObject)Instantiate(prefabStat);
            goStat[i].transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = jsonArray[i].AsObject["time"];
            goStat[i].transform.GetChild(1).gameObject.GetComponentInChildren<Text>().text = jsonArray[i].AsObject["soil_moisture"];
            goStat[i].transform.GetChild(2).gameObject.GetComponentInChildren<Text>().text = jsonArray[i].AsObject["temperature"];
            goStat[i].transform.GetChild(3).gameObject.GetComponentInChildren<Text>().text = jsonArray[i].AsObject["pump_status"] == "1" ? "ON" : "OFF";

            goStat[i].transform.SetParent(canvasRef.transform);

            goStat[i].GetComponent<RectTransform>().offsetMin = new Vector2 (2,600);
            goStat[i].GetComponent<RectTransform>().offsetMax = new Vector2 (0,-100 * (i + 1));
        }
    }
    IEnumerator getStat()
    {
        WWWForm form = new WWWForm();
        form.AddField("farm_id", farm_id);
        form.AddField("dateShow", dateShow);
        WWW www = new WWW("http://localhost/sqlconnect/statistic.php", form);
        yield return www;

        string result = www.text;
        jsonArray = JSON.Parse(result) as JSONArray;
        // Debug.Log(result);
    }

    public void showStat()
    {
        dateShow = dateField.text;  
        while (GameObject.Find("prefabStat(Clone)") != null) {
            GameObject.Find("prefabStat(Clone)").SetActive(false);   
        }
        test();
    }

    public void test() 
    {
        StartCoroutine(run());
    }

    public void VerifyInputs()
    {
        string strRegex = @"(^[0-9]{4}-([0]{1}[1-9]{1}|[1]{1}[0-2]{1})-([0-2]{1}[0-9]{1}|[3]{1}[0-1]{1})$)";
                
        Regex re = new Regex(strRegex);
                 
        submitButton.interactable = re.IsMatch(dateField.text);
    }
}