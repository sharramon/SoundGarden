using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NetworkSetting", menuName = "Net/NetworkSettingData", order = 0)]
public class NetworkSetting : ScriptableObject
{
    public string SendURL;
    public string authToken;
    public string SavePath;
}
