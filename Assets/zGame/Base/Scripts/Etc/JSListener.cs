using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Defective.JSON;

namespace ntDev
{
    public class JSListener : MonoBehaviour
    {
        void Start()
        {
#if UNITY_EDITOR
            // CityConnection._instance.Connect(CityConnection.SERVER_WEB_SOCKET);
#endif
        }

        // public void LoginDude(string data = "")
        // {
        //     JSONObject jSONObject = new JSONObject(data);
        //     string url = jSONObject["url"].stringValue;
        //     string token = jSONObject["token"].stringValue;
        //     noone.Ez.Log("Connect with token: " + token);
        //     CityConnection._instance.Connect(url, token);
        // }
    }

    [Serializable]
    public class EventDataForJS
    {
        public string EventName;
        public string Data;
    }
}