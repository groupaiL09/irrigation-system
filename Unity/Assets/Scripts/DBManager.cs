using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DBManager
{
    public static string username = "tuannguyen1";
    // public static string username;
    public static string password;
    public static string userId;
    public static string farmId = "1";
    // public static string farmId;
    public static string farmIdList;
    public static int pre;
    public static int cur;

    public static bool LoggedIn { get { return username != null; } }
}
