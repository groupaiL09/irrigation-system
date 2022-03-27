using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

namespace HomePage
{
    public class FarmMQTT : M2MqttUnityClient
    {
        public List<string> topics = new List<string>();
        private List<string> eventMessages = new List<string>();
        public string farm_id = "1";
        public static int modAuto = 0;

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }

        public void UpdateModeAuto(string msg)
        {
            modAuto = Int32.Parse(msg);
        }

        public void PublishPumpOn()
        {
            client.Publish(topics[2], System.Text.Encoding.UTF8.GetBytes("1"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("Turn Pump ON!");
        }

        public void PublishPumpOff()
        {
            client.Publish(topics[2], System.Text.Encoding.UTF8.GetBytes("0"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("Turn Pump OFF!");
        }

        public void PublishModeAuto(string modAuto)
        {
            client.Publish(topics[3], System.Text.Encoding.UTF8.GetBytes(modAuto), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("Update Mode Auto!");
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            Debug.Log("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            SubscribeTopics();
        }

        protected override void SubscribeTopics()
        {

            foreach (string topic in topics)
            {
                if (topic != "")
                {
                    client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                }
            }
        }

        protected override void UnsubscribeTopics()
        {
            foreach (string topic in topics)
            {
                if (topic != "")
                {
                    client.Unsubscribe(new string[] { topic });
                }
            }

        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            base.Disconnect();
        }

        protected override void OnDisconnected()
        {
            Debug.Log("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            Debug.Log("CONNECTION LOST!");
        }
        
        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg + " from " + topic);
            //StoreMessage(msg);
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;
            if (sceneName == "Homepage")
            {
                GameObject homeManager = GameObject.FindGameObjectWithTag("Manager_homepage");
                if (topic == topics[0])
                    homeManager.GetComponent<HomePageManager>().UpdateTemp(msg);
                if (topic == topics[1])
                    homeManager.GetComponent<HomePageManager>().UpdateSoil(msg);
                if (topic == topics[2])
                {
                    homeManager.GetComponent<HomePageManager>().UpdatePumpStatus(msg);
                }
            }
            if (topic == topics[3])
            {
                UpdateModeAuto(msg);
            }
        }


        private void OnDestroy()
        {
            Disconnect();
        }

        public void Start()
        {
            topics.Add("groupaiL09/f/farm"+ farm_id +".temperature");
            topics.Add("groupaiL09/f/farm" + farm_id + ".moisture");
            topics.Add("groupaiL09/f/farm" + farm_id + ".pump-status");
            topics.Add("groupaiL09/f/farm" + farm_id + ".mode-auto");
            base.brokerAddress = "io.adafruit.com";
            base.mqttUserName = "groupaiL09";
            base.mqttPassword = "aio_njzm76fKeROefSeNRAUMsW4ilPix";
            base.Connect();
        }
    }
}

