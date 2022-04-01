using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using SimpleJSON;
// using Newtonsoft.Json.Linq;

public class farmlist : MonoBehaviour
{
    public Text userDisplay;
    public CanvasGroup addFormInfo;
    public GameObject addForm;
    public GameObject canvasRef;
    public GameObject prefabButton;
    //public Button virtualButton;
    public InputField addressField;
    public InputField areaField;
    
    public string jsonArrayString;
    public static int noOfFarms = 0;

    //Action<string> _createFarmsCallback;

    private void Start()
    {
        if (DBManager.LoggedIn)
        {
            CallgetFarmIds();
            DBManager.farmIdList = jsonArrayString;

            //userDisplay.text = "Welcome " + DBManager.userId + "!";
            /*_createFarmsCallback = (jsonArrayString) =>
            {
                StartCoroutine(CreateFarmsRoutine(jsonArrayString));
            };*/
        }
        // DBManager.cur = 2;
        // Debug.Log("Scene " + DBManager.cur);
    }

    public void GotoFarm()
    {
        Debug.Log("farm created successfully");
    }


    public void CalladdFarm()
    {
        StartCoroutine(addFarm(DBManager.userId));
    }

    public void CallgetFarmIds()
    {
        StartCoroutine(getFarmIds(DBManager.userId));
    }

    public void showForm()
    {
        
        addForm.SetActive(true);
        addFormInfo.alpha = 1;
    }

    public void closeForm()
    {
        addFormInfo.alpha = 0;
        addForm.SetActive(false);
    }
    

    public void LoadFarms(string jsonArrayString)
    {
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
        //JObject jsonArray = JObject.Parse(jsonArrayString);

        for (int i = 0; i < jsonArray.Count; i++)
        {
            string farmId = jsonArray[i].AsObject["farm_id"];
            
            GameObject farm = (GameObject)Instantiate(prefabButton);
            farm.transform.SetParent(canvasRef.transform);
            //Button btn = virtualButton;
            //btn.onClick.AddListener(delegate { GotoFarm(); });
            //virtualButton.onClick.AddListener(GotoFarm);

            if (i == 0)
            {
                
                farm.transform.localPosition = new Vector2(419f, 89f);
                //farm.transform.localScale = Vector2;
                farm.GetComponentInChildren<Text>().text = "Farm " + (i + 1);
                
            }
            if (i == 1)
            {
                
                farm.transform.localPosition = new Vector2(419f, -319f);
                //farm.transform.localScale = Vector2;
                farm.GetComponentInChildren<Text>().text = "Farm " + (i + 1);
                
            }
            if (i == 2)
            {
                
                farm.transform.localPosition = new Vector2(419f, -549f);
                //farm.transform.localScale = Vector2;
                farm.GetComponentInChildren<Text>().text = "Farm " + (i + 1);
                
            }
            if (i == 3)
            {
                
                farm.transform.localPosition = new Vector2(419f, -780f);
                //farm.transform.localScale = Vector2;
                farm.GetComponentInChildren<Text>().text = "Farm " + (i + 1);
                    
            }

            
        }
    }



    public void create()
    {
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
        noOfFarms = jsonArray.Count;
        GameObject farm = (GameObject)Instantiate(prefabButton);
        farm.transform.SetParent(canvasRef.transform);
        Button btn = farm.GetComponent<Button>();
        btn.onClick.AddListener(delegate { GotoFarm(); });
        if (noOfFarms == 0)
        {
            farm.transform.localPosition = new Vector2(419f, 89f);
            //farm.transform.localScale = Vector2;
            farm.GetComponentInChildren<Text>().text = "Farm " + (noOfFarms + 1);
        }
        if (noOfFarms == 1)
        {
            farm.transform.localPosition = new Vector2(419f, -319f);
            //farm.transform.localScale = Vector2;
            farm.GetComponentInChildren<Text>().text = "Farm " + (noOfFarms + 1);
        }
        if (noOfFarms == 2)
        {
            farm.transform.localPosition = new Vector2(419f, -549f);
            //farm.transform.localScale = Vector2;
            farm.GetComponentInChildren<Text>().text = "Farm " + (noOfFarms + 1);
        }
        if (noOfFarms == 3)
        {
            farm.transform.localPosition = new Vector2(419f, -780f);
            //farm.transform.localScale = Vector2;
            farm.GetComponentInChildren<Text>().text = "Farm " + (noOfFarms + 1);
        }
        

    }

    IEnumerator addFarm(string userId)
    {

        WWWForm form = new WWWForm();
        form.AddField("address", addressField.text);
        form.AddField("area", areaField.text);
        form.AddField("userId", userId);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/insertFarms.php", form))
        {
            yield return www.Send();

            if (www.downloadHandler.text == "0")
            {
                Debug.Log("farm created successfully");
            }
            else
            {
                Debug.Log("Creation failed. error #" + www.downloadHandler.text);
            }

        }


        create();
        // Update Farmlist on mobile screen
        WWWForm form1 = new WWWForm();
        form1.AddField("userId", userId);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/getFarms.php", form1))
        {
            yield return www.Send();

            if (www.downloadHandler.text == "0")
            {
                Debug.Log("Creation failed. error #" + www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                jsonArrayString = www.downloadHandler.text;
                DBManager.farmIdList = jsonArrayString;
                // callback(jsonArrayString);
            }

        }

        //resetField();

        closeForm();
    }

    //public IEnumerator getFarmIds(string userId, System.Action<string> callback)
    public IEnumerator getFarmIds(string userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/getFarms.php", form))
        {
            yield return www.Send();

            if (www.downloadHandler.text == "0")
            {
                // callback(jsonArrayString);
                Debug.Log("Creation failed. error #" + www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                jsonArrayString = www.downloadHandler.text;

                DBManager.farmIdList = jsonArrayString;
                Debug.Log(DBManager.farmIdList);
                LoadFarms(jsonArrayString);
            }

        }
    }
}