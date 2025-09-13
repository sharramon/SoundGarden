using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerMic : MonoBehaviour
{
    [SerializeField] private SphereCollider _sphereCollider;
    public SphereCollider sphereCollider { get { return _sphereCollider; } }
    public Vector3 micOffset = new Vector3(0, 0.1f, 0.05f);
    public float triggerRadius = 0.05f;

    private void Awake()
    {
        CreateMicTrigger();
    }

    private void CreateMicTrigger()
    {
        if (_sphereCollider != null)
        {
            Destroy(_sphereCollider.gameObject);
        }

        GameObject sphereTriggerObject = new GameObject("micTrigger");
        _sphereCollider = sphereTriggerObject.AddComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;
        _sphereCollider.radius = triggerRadius;
        sphereTriggerObject.AddComponent<FlowerMicTrigger>();

        Transform offControllerTransform = GameObject.Find("OVRLeftControllerPrefab").transform;
        sphereTriggerObject.transform.SetParent(offControllerTransform);

        sphereTriggerObject.transform.localPosition = micOffset;

        SetColliderOn(false);
    }

    public void SetColliderOn(bool isOn)
    {
        if (sphereCollider == null)
            return;

        sphereCollider.enabled = isOn;
    }
}
