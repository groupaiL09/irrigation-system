using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homepage : MonoBehaviour
{
    public void gotoDevices() 
    {
        DBManager.pre = DBManager.cur;
        DBManager.cur = 5;
        UnityEngine.SceneManagement.SceneManager.LoadScene(DBManager.cur); 
    }
    public void gotoWater()
    {
        DBManager.pre = DBManager.cur;
        DBManager.cur = 6;
        UnityEngine.SceneManagement.SceneManager.LoadScene(DBManager.cur); 
    }
    public void gotoNotification() 
    {
        DBManager.pre = DBManager.cur;
        DBManager.cur = 7;
        UnityEngine.SceneManagement.SceneManager.LoadScene(DBManager.cur); 
    }
    public void gotoStatistic()
    {
        DBManager.pre = DBManager.cur;
        DBManager.cur = 8;
        UnityEngine.SceneManagement.SceneManager.LoadScene(DBManager.cur); 
    }
    public void gotoFarminfo() 
    {
        DBManager.pre = DBManager.cur;
        DBManager.cur = 9;
        UnityEngine.SceneManagement.SceneManager.LoadScene(DBManager.cur); 
    }
    public void backHomepage()
    {
        DBManager.pre = DBManager.cur;
        DBManager.cur = 3;
        UnityEngine.SceneManagement.SceneManager.LoadScene(DBManager.cur); 
    }
    public void back() 
    {
        if (DBManager.cur == 3) 
            DBManager.pre = 2;
        int temp = DBManager.cur;
        DBManager.cur = DBManager.pre;
        DBManager.pre = temp;
        UnityEngine.SceneManagement.SceneManager.LoadScene(DBManager.cur); 

    }
}
