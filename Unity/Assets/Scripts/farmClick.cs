    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Networking;
    using System;
    using UnityEngine.EventSystems;
    using SimpleJSON;


    public class farmClick : MonoBehaviour
    {
        public GameObject prefabButton;
        public GameObject canvasRef;
        //public string[] farmIdList = new string[10];
        public List<string> farmIdList = new List<string>();


    public void GotoFarm()
    {
        GameObject farm = EventSystem.current.currentSelectedGameObject;

        string jsonArrayString = DBManager.farmIdList;
        
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
        Debug.Log(jsonArrayString + "   " + jsonArray.Count);
        for (int i = 0; i < jsonArray.Count; i++)
        {
            //Debug.Log(jsonArray[i].AsObject["farm_id"]);
            
            farmIdList.Add(jsonArray[i].AsObject["farm_id"]);
        }

        

        for (int i = 0; i < jsonArray.Count; i++)
        {
            if(farm.GetComponentInChildren<Text>().text == String.Concat("Farm " + (i+1)))
            {
                DBManager.farmId = farmIdList[i];
                Debug.Log(i);
            }
        }

       

        Debug.Log("farm_id: " + DBManager.farmId);
        DBManager.cur = 3;
        DBManager.pre = 2;
        UnityEngine.SceneManagement.SceneManager.LoadScene(DBManager.cur);
    }
}