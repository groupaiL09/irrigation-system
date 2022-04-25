using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DBManager
{
    public static string username;
    public static string password;
    public static string userId;

    public static string ip = "192.168.199.137";

    public static string farmId = "0";
    public static string farmIdList;
    public static int pre;
    public static int cur;
    public static int loadFarm = 0;

    public static int noOfFarms = 0;
    public static List<GameObject> clones = new List<GameObject>(); //List of clone farm(button) base on user_id

    public static List<string> localData = new List<string>() {"80",    // 0 - temp
                                                               "90",    // 1 - moisture
                                                               "0",     // 2 - pump status
                                                               "0",     // 3 - mode auto
                                                               "0"      // 4 - next pump
                                                                };

    public static bool LoggedIn { get { return username != null; } }


}
