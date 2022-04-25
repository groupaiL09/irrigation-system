using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using SimpleJSON;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

public class farmlist : MonoBehaviour
{
    public Text userDisplay;
    public CanvasGroup addFormInfo;
    public GameObject addForm;
    public GameObject canvasRef;
    public GameObject prefabButton;
    public Button saveButton;
    //public Button virtualButton;
    public InputField addressField;
    public InputField areaField;
    
    public string jsonArrayString;
    //public static int noOfFarms = 0;

    public List<string> farmIdList = new List<string>();


    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void ToggleDeleteButton()
    {
        foreach (GameObject clone in DBManager.clones)
        {
            Debug.Log(DBManager.clones.Count);
            GameObject child = clone.transform.GetChild(1).gameObject;
            //Debug.Log(child);
            Button delete = child.GetComponentInChildren<Button>();
            //Debug.Log(delete);
            delete.onClick.AddListener(CallDeleteFarm);
            if (child.activeSelf)
            {
                child.SetActive(false);
            }
            else
            {
                child.SetActive(true);
            }
        }
    }

    public void CallDeleteFarm()
    {
        StartCoroutine(DeleteFarm());
    }

    IEnumerator DeleteFarm()
    {
        farmIdList = new List<string>(); //reset list of farmId because jsonArrayString has already been changed

        GameObject child = EventSystem.current.currentSelectedGameObject;

        GameObject current = child.transform.parent.gameObject;

        GameObject farm = current.transform.parent.gameObject;

        //string jsonArrayString = DBManager.farmIdList;
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
        string deleteId = "failed";  //FarmId of deleted farm

        for (int i = 0; i < jsonArray.Count; i++)
        {
            farmIdList.Add(jsonArray[i].AsObject["farm_id"]);
        }

        Debug.Log(farm);

        //Find farmId of deleted farm
        for (int i = 0; i < jsonArray.Count; i++)
        {
            if (farm.GetComponentInChildren<Text>().text == String.Concat("Farm " + (i + 1)))
            {
                deleteId = farmIdList[i];
            }
        }

        //Delete farm database 
        WWWForm form = new WWWForm();
        form.AddField("farmId", deleteId);
        using (UnityWebRequest www = UnityWebRequest.Post("http://" + DBManager.ip + "/sqlconnect/deleteFarms.php", form))
        {
            yield return www.Send();

            if (www.downloadHandler.text == "0")
            {
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Delete failed. error #" + www.downloadHandler.text);
            }

        }

        //DBManager.noOfFarms--;

        Destroy(farm);

        DBManager.clones.Remove(farm);
        DBManager.noOfFarms = DBManager.clones.Count;

        //Destroy(farm);

        Debug.Log(deleteId);
        Debug.Log(DBManager.clones.Count);
        Debug.Log(DBManager.noOfFarms);

        // Update Farmlist on mobile screen
        WWWForm form1 = new WWWForm();
        form1.AddField("userId", DBManager.userId);
        using (UnityWebRequest www = UnityWebRequest.Post("http://" + DBManager.ip + "/sqlconnect/getFarms.php", form1))
        {
            yield return www.Send();

            if ((www.downloadHandler.text).Contains("["))
            {
                Debug.Log(www.downloadHandler.text);
                jsonArrayString = www.downloadHandler.text;
                DBManager.farmIdList = jsonArrayString;
                //LoadFarms(jsonArrayString);
            }
            else
            {
                Debug.Log("Update failed. error #" + www.downloadHandler.text);
            }

        }

        ToggleDeleteButton();
        LoadScene();
    }

    public void VerifyInputs()
    {
        saveButton.interactable = (addressField.text.Length >= 4 && areaField.text.Length >= 3);
    }

    private void Start()
    {
        if (DBManager.LoggedIn)
        {
            
            DBManager.loadFarm = 0;
            DBManager.clones = new List<GameObject>();
            DBManager.farmId = "";
            CallgetFarmIds();
            //DBManager.farmIdList = jsonArrayString;
        }
    }

    public void LogOut()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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


            DBManager.clones.Add(farm);
        }
        DBManager.noOfFarms = jsonArray.Count;
    }



    public void create()
    {
        /*if (DBManager.noOfFarms > 0)
        {
            JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
            DBManager.noOfFarms = jsonArray.Count;
        }*/
        GameObject farm = (GameObject)Instantiate(prefabButton);
        farm.transform.SetParent(canvasRef.transform);
        
        //Button btn = farm.GetComponent<Button>();
        //btn.onClick.AddListener(delegate { GotoFarm(); });
        
        if (DBManager.noOfFarms == 0)
        {
            farm.transform.localPosition = new Vector2(419f, 89f);
            //farm.transform.localScale = Vector2;
            farm.GetComponentInChildren<Text>().text = "Farm " + (DBManager.noOfFarms + 1);
        }
        if (DBManager.noOfFarms == 1)
        {
            farm.transform.localPosition = new Vector2(419f, -319f);
            //farm.transform.localScale = Vector2;
            farm.GetComponentInChildren<Text>().text = "Farm " + (DBManager.noOfFarms + 1);
        }
        if (DBManager.noOfFarms == 2)
        {
            farm.transform.localPosition = new Vector2(419f, -549f);
            //farm.transform.localScale = Vector2;
            farm.GetComponentInChildren<Text>().text = "Farm " + (DBManager.noOfFarms + 1);
        }
        if (DBManager.noOfFarms == 3)
        {
            farm.transform.localPosition = new Vector2(419f, -780f);
            //farm.transform.localScale = Vector2;
            farm.GetComponentInChildren<Text>().text = "Farm " + (DBManager.noOfFarms + 1);
        }

        DBManager.clones.Add(farm);
        DBManager.noOfFarms = DBManager.clones.Count;
    }

    IEnumerator addFarm(string userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("address", addressField.text);
        form.AddField("area", areaField.text);
        form.AddField("userId", userId);
        using (UnityWebRequest www = UnityWebRequest.Post("http://" + DBManager.ip + "/sqlconnect/insertFarms.php", form))
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
        using (UnityWebRequest www = UnityWebRequest.Post("http://" + DBManager.ip + "/sqlconnect/getFarms.php", form1))
        {
            yield return www.Send();

            if ((www.downloadHandler.text).Contains("["))
            {
                Debug.Log(www.downloadHandler.text);
                jsonArrayString = www.downloadHandler.text;
                DBManager.farmIdList = jsonArrayString;
                // callback(jsonArrayString);
            }
            else
            {
                Debug.Log("Update failed. error #" + www.downloadHandler.text);
            }

        }

        closeForm();
        //LoadScene();
    }

    //public IEnumerator getFarmIds(string userId, System.Action<string> callback)
    public IEnumerator getFarmIds(string userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        using (UnityWebRequest www = UnityWebRequest.Post("http://" + DBManager.ip + "/sqlconnect/getFarms.php", form))
        {
            yield return www.Send();

            if ((www.downloadHandler.text).Contains("["))
            {
                Debug.Log(www.downloadHandler.text);
                jsonArrayString = www.downloadHandler.text;
                DBManager.farmIdList = jsonArrayString;
                
                LoadFarms(jsonArrayString);
                // callback(jsonArrayString);

            }
            else if(www.downloadHandler.text == "0")
            {
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Creation failed. error #" + www.downloadHandler.text);
            }

        }
    }

}