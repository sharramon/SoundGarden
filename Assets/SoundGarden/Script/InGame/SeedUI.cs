using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedUI : MonoBehaviour
{
    [SerializeField] private GameObject swipeUI;
    [SerializeField] private GameObject sendUI;

    private void Update()
    {
        Utils.LookAtInXZPlane(this.transform, PlayerInputHandler.instance.mainCamera.transform);
    }

    public void SetSwipeUI(bool isOn)
    {
        SetAllOff();
        swipeUI.SetActive(isOn);
    }

    public void SetSendUI(bool isOn)
    {
        SetAllOff();
        sendUI.SetActive(isOn);
    }

    public void SetAllOff()
    {
        swipeUI.SetActive(false);
        sendUI.SetActive(false);
    }
}
