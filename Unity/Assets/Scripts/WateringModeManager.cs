using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WateringModeManager : MonoBehaviour
{
    public ToggleGroup toggleGroup;

    public void UpdateAutoMode()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        string choice = (toggle.GetComponentInChildren<Text>().text == "ON") ? "1" : "0";
        GameObject farmMQTT = GameObject.FindGameObjectWithTag("Manager_mqtt");
        farmMQTT.GetComponent<HomePage.FarmMQTT>().PublishModeAuto(choice);
    }

    public void BackToHomePage()
    {
        SceneManager.LoadScene("HomePage");
    }
}
