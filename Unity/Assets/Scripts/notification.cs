using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using SimpleJSON;

public class notification : MonoBehaviour
{
    public InputField dateField;
    public GameObject canvasRef;
    public GameObject prefabNotif;
    public static string username = DBManager.username;  
    public static JSONArray jsonArray; 
    public static string dateShow = DateTime.UtcNow.ToString("yyyy-MM-dd");  

    public static int notifNumber;
    public static GameObject[] goNotif; 

    public Button submitButton;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        // button choose date
            // dateShow = "2022-03-19";
            dateField.text = dateShow;
            yield return run();
    }

    IEnumerator run() {
        // username && dateShow
        // receive from php
            yield return getNotif();

            notifNumber = jsonArray.Count;
            //
                // date[0] = "2022-03-27";
                // date[1] = "2022-03-27";
                
                // time[0] = "10:12:54";
                // time[1] = "18:01:12";

                // content[0] = "chao ban lan 1 trong ngay";
                // content[1] = "chao ban lan 2 trong ngay";

        goNotif = new GameObject[notifNumber];
        // goNotif = new (GameObject)Instantiate(prefabNotif) [notifNumber];
        for (int i = 0; i < notifNumber; i++) {
            // goNotif[i] = new (GameObject)Instantiate(prefabNotif);
            goNotif[i] = (GameObject)Instantiate(prefabNotif);
            goNotif[i].GetComponentsInChildren<Text>()[0].text = jsonArray[i].AsObject["content"];
            goNotif[i].GetComponentsInChildren<Text>()[1].text = jsonArray[i].AsObject["time"];
            goNotif[i].transform.SetParent(canvasRef.transform);

            switch (i){
                case 0: 
                    goNotif[i].transform.localPosition = new Vector2(-2, 80);
                    break;
                case 1: 
                    goNotif[i].transform.localPosition = new Vector2(-2, -230);
                    break;
                case 2: 
                    goNotif[i].transform.localPosition = new Vector2(-2, -540);
                    break;
            }
        }
    }    

    IEnumerator getNotif()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("dateShow", dateShow);
        WWW www = new WWW("http://" + DBManager.ip + "/sqlconnect/notification.php", form);
        yield return www;

        string result = www.text;
        jsonArray = JSON.Parse(result) as JSONArray;
        // Debug.Log(result);
    }

    public void showNotif()
    {
        dateShow = dateField.text;  
        while (GameObject.Find("prefabNotif(Clone)") != null) {
            GameObject.Find("prefabNotif(Clone)").SetActive(false);   
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