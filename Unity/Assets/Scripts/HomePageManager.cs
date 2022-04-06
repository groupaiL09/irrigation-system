using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

namespace HomePage
{
    public class HomePageManager : MonoBehaviour
    {
        public Text tempValue;
        public Text soilValue;
        public Text nextDayValue;
        public Text timeValue;

        static int pumpStatus = 0;

        public GameObject pumpOn;
        public Button turnOnPump;
        public GameObject pumpOff;
        public Button turnOffPump;

        static int saferOnClick = 0;

        public void UpdateNextTime(string msg)
        {
            DateTime theTime = DateTime.Now;
            Debug.Log("theTime:" + theTime);
            theTime = theTime.AddMinutes(Int32.Parse(msg)); //fixed
            Debug.Log("theTime:" + theTime);
            nextDayValue.text = theTime.ToString();
        }

        public void UpdateDBManager(int index, string msg)
        {
            DBManager.statistic[index] = msg;
        }

        public void UpdateTemp()
        {
            tempValue.text = DBManager.statistic[0] + "°C";
        }

        public void UpdateSoil()
        {
            soilValue.text = DBManager.statistic[1] + "%";
        }

        public void UpdatePumpStatus(string msg)
        {
            pumpStatus = Int32.Parse(msg);
            UpdatePumpUI();
        }

        public void UpdatePumpUI()
        {
            if (pumpStatus == 0)
            {
                pumpOff.SetActive(true);
                pumpOn.SetActive(false);
            }
            else
            {
                pumpOff.SetActive(false);
                pumpOn.SetActive(true);
            }
        }
       
        void Update()
        {
            int modAuto = HomePage.FarmMQTT.modAuto;
            timeValue.text = System.DateTime.Now.ToString("dd/MM/yyyy");
            if (modAuto == 1)
            {
                removeListener();
            }
            else if (saferOnClick == 0)
            {
                addListenerInit();
            }
           
        }

        void InitButtonOn()
        {
            GameObject mqtt = GameObject.FindGameObjectWithTag("Manager_mqtt");
            mqtt.GetComponent<FarmMQTT>().PublishPumpOn();
        }

        void InitButtonOff()
        {
            GameObject mqtt = GameObject.FindGameObjectWithTag("Manager_mqtt");
            mqtt.GetComponent<FarmMQTT>().PublishPumpOff();
        }

        void addListenerInit()
        {
            turnOnPump.onClick.AddListener(InitButtonOff);
            turnOffPump.onClick.AddListener(InitButtonOn);
            saferOnClick = 1;
        }

        void removeListener()
        {
            turnOnPump.onClick.RemoveAllListeners();
            turnOffPump.onClick.RemoveAllListeners();
            saferOnClick = 0;
        }

        void Start()
        {
            if(DBManager.loadFarm == 0)
            {
                addListenerInit();
                DBManager.loadFarm = 1;
            }
            
            UpdatePumpUI();
            Update();
        }
    }
}

