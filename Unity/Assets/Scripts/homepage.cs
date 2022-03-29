using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homepage : MonoBehaviour
{
    public void gotoNotification() 
    {
       UnityEngine.SceneManagement.SceneManager.LoadScene(7);   
    }
    public void gotoStatistic()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(8);  
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
