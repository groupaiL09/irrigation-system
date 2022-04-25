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

        public GameObject pumpOn;
        public Button turnOnPump;
        public GameObject pumpOff;
        public Button turnOffPump;

        static int saferOnClick = 0;

        public void UpdateNextTime()
        {
            int min = Int32.Parse(DBManager.localData[4]);
            if (min < 10)
            {
                nextDayValue.text = "UNDEFINED";
                return;
            }
            DateTime theTime = DateTime.Now;
            theTime = theTime.AddMinutes(min);
            nextDayValue.text = theTime.ToString();
        }

        public void UpdateTemp()
        {
            tempValue.text = DBManager.localData[0] + "�C";
        }

        public void UpdateSoil()
        {
            soilValue.text = DBManager.localData[1] + "%";
        }

        public void UpdatePumpUI()
        {
            if (DBManager.localData[2] == "0")
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

        void Update()                                                           // UPDATE FRAME BY FRAME time + automod + data
        {
            UpdateTemp();
            UpdateSoil();
            UpdatePumpUI();
            UpdateNextTime();
            timeValue.text = System.DateTime.Now.ToString("dd/MM/yyyy");
            string modAuto = DBManager.localData[3];
            if (modAuto == "1")
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
            addListenerInit();
            Update();
        }
    }
}
