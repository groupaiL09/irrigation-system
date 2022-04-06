using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DBManager
{
    public static string username;
    public static string password;
    public static string userId;
    public static string farmId;
    public static string farmIdList;
    public static int pre;
    public static int cur;
    public static int loadFarm = 0;

    public static int noOfFarms = 0;
    public static List<GameObject> clones = new List<GameObject>(); //List of clone farm(button) base on user_id

    public static List<string> statistic = new List<string>(); //index: 0 - temp, 1 - moisture, 2 - pump status, 3 - mode auto

    public static bool LoggedIn { get { return username != null; } }
}