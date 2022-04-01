using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
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

    void Start()
    {
        setting.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
