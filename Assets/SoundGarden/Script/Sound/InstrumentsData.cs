using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstrumentalSetting", menuName = "Inst/InstrumentalSettingData", order = 0)]
public class InstrumentsData : ScriptableObject
{
    public List<Inst> Insts;
}

[Serializable]
public class Inst
{
    public string InstName;
    public int InstID;
    public Color seedColor;
    public GameObject seedModel;
    public GameObject flowerModel;
    public AudioClip introSound;
}