using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homepage : MonoBehaviour
{
    public void gotoDevices() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(5); 
    }
    public void gotoWater()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(6); 
    }
    public void gotoNotification() 
    {
       UnityEngine.SceneManagement.SceneManager.LoadScene(7);   
    }
    public void gotoStatistic()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(8);  
    }
    public void gotoFarminfo() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(9); 
    }
    public void backHomepage()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3); 
    }
    public void back() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(9); 
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
