using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

[CreateAssetMenu(fileName = "SkySettingData", menuName = "Sky/SkySetting", order = 0)]

public class SkyData : ScriptableObject
{
    public SkyElement[] SkyElements;
}

[Serializable]
public class SkyElement
{
    public Material skyMaterial;
    public Material uiMarerial;
    public AudioClip bgmClip;
}
