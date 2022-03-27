using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public CanvasGroup settingScene;
    public GameObject setting;
    
    public void toggleState()
    {
        if (setting.activeSelf)
        {
            settingScene.alpha = 0;
            setting.SetActive(false);
        }
        else
        {
            settingScene.alpha = 1;
            setting.SetActive(true);
        }
    }

    public void changeSceneToWateringMode()
    {
        SceneManager.LoadScene("Watering_mode");
    }

    void Start()
    {
        setting.SetActive(false);
    }
}
