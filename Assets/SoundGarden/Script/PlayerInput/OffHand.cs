using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffHand : MonoBehaviour
{
    [SerializeField] private Transform wristTransform;
    public Transform _wristTransform { get { return wristTransform; } }
    [SerializeField] private Collider indexFinger;
    public Collider _indexFinger { get { return indexFinger; } }
    [SerializeField] private OVRHand ovrHand;
    public OVRHand _ovrHand { get { return ovrHand; } }

    
}
