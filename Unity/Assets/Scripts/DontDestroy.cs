using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static bool realFarmMQTT;
    private bool curFarmMQTT;

    private void Awake()
    {
        if (this.gameObject.name == "Manager_mqtt")
        {
            if (!realFarmMQTT)
            {
                realFarmMQTT = true;
                curFarmMQTT = true;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (realFarmMQTT && !curFarmMQTT)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
