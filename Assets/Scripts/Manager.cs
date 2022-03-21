using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public CanvasGroup settingScene;
    public CanvasGroup editDeviceInfo;
    public GameObject setting;
    public GameObject edit;
    // Start is called before the first frame update
    public void showEditPanel()
    {
        edit.SetActive(true);
        editDeviceInfo.alpha = 1;
    }

    public void closeEditPanel()
    {
        editDeviceInfo.alpha = 0;
        edit.SetActive(false);
    }

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
        edit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
