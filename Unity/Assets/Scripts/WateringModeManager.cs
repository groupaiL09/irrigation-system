using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WateringModeManager : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public UnityEngine.UI.Toggle isOnRadio;
    public UnityEngine.UI.Toggle isOffRadio;

    public void UpdateAutoMode()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        string choice = (toggle.GetComponentInChildren<Text>().text == "ON") ? "1" : "0";
        GameObject farmMQTT = GameObject.FindGameObjectWithTag("Manager_mqtt");
        farmMQTT.GetComponent<HomePage.FarmMQTT>().PublishModeAuto(choice);
    }

    private void Start()
    {
        GameObject farmMQTT = GameObject.FindGameObjectWithTag("Manager_mqtt");
        int tmp = farmMQTT.GetComponent<HomePage.FarmMQTT>().getValueOfAutoMod();
        if (tmp == 1)
        {
            isOnRadio.isOn = true;
            isOffRadio.isOn = false;
        }
        else
        {
            isOnRadio.isOn = false;
            isOffRadio.isOn = true;
        }
    }
}
