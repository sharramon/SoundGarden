using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySelector : MonoBehaviour
{
     public SkyData skyData;
     public Renderer[] skyUI;
     public Renderer skyPortal;
          
     private void Start()
     {
          for (int i = 0; i < skyUI.Length; i++)
          {
               Debug.Log($"before : {skyUI[i].material.name}");
               skyUI[i].material = skyData.SkyElements[i].uiMarerial;
               Debug.Log($"after : {skyUI[i].material.name} / {skyData.SkyElements[i].uiMarerial}");
          }
     }

     public void ChangeSky(int index)
     {
          Debug.Log($"before : { skyPortal.materials[1].name}");
          //skyPortal.materials[1] = skyData.SkyElements[index].skyMaterial;
          
          // Materials 배열 복사 후 변경
          Material[] materials = skyPortal.materials;
          materials[1] = skyData.SkyElements[index].skyMaterial;
          skyPortal.materials = materials;
          
          Debug.Log($"after : { skyPortal.materials[1].name} / {skyData.SkyElements[index].skyMaterial}");
          
          SoundManager.Instance.PlayBGM(skyData.SkyElements[index].bgmClip);
     }
}
