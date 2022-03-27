using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class FarmInfo
{
    public string farm_id { get; set; }
    public string user_id { get; set; }
    public string name { get; set; }
    public string address { get; set; }
    public string area { get; set; }
}

public class editFarmInfo : MonoBehaviour
{
    public InputField name;
    public InputField address;
    public InputField area;

    public void Save()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name.text);
        form.AddField("address", address.text);
        form.AddField("area", area.text);
        WWW www = new WWW("http://localhost/sqlconnect/editFarmInfo.php", form);
    }

    void Start()
    {
        WebRequest request = WebRequest.Create("http://localhost/sqlconnect/farm-service.php");
        request.Method = "GET";
        StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
        string response = responseReader.ReadToEnd();
        FarmInfo farmInfo = JsonConvert.DeserializeObject<FarmInfo>(response);
        name.text = farmInfo.name;
        address.text = farmInfo.address;
        area.text = farmInfo.area;
    }
}
