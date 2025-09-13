using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : Singleton<PlayerInputHandler>
{
    public Camera mainCamera { get; private set; }
    //main hand
    [Header ("Main Hand")]
    [SerializeField] private OVRPlugin.Hand mainHandType = OVRPlugin.Hand.HandRight;
    [SerializeField] private float minAngle = 0.0f; // 최소 허용 각도
    [SerializeField] private float maxAngle = 80.0f; // 최대 허용 각도
    private OVRSkeleton.BoneId colliderBone = OVRSkeleton.BoneId.Hand_IndexTip;
    private OVRHand mainHand;
    private OVRSkeleton mainHandSkeleton;
    public Transform wristTransform { get; private set; }

    public HandCollider HandCollider;
    [Header("Off Hand")]
    //off hand
    [SerializeField] private OVRInput.Controller _offHand = OVRInput.Controller.LTouch;
    public OVRInput.Controller offHand { get { return _offHand; } }

    //actions
    //mainHand
    public Action onMainHandLookAt;
    public Action onMainHandLookAway;
    public Action onMainHandPinch;
    public Action onMainHandPinchRelease;
    //offHand
    public Action<float> onOffHandTriggerPressed;
    public Action onOffHandTriggerReleased;

    //state check bools
    private bool isOffHandLook = false;
    private bool offTriggerPressed = false;
    private bool isPinching = false;

    private void Awake()
    {
        mainCamera = Camera.main;
        mainHand = GetMainOVRHand(mainHandType);
        if (mainHand != null)
        {
            StartCoroutine(WaitForSkeletonInitialization());
        }
    }

    private void Update()
    {
        CheckIfLookAt();

        float leftTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, _offHand);
        HandleButtonInput(leftTrigger, ref offTriggerPressed, onOffHandTriggerPressed, onOffHandTriggerReleased);

        CheckForPinchGesture();

        if (wristTransform == null && mainHandSkeleton != null)
            GetWristTransform(mainHandSkeleton);
    }

    private void CheckIfLookAt()
    {
        if (wristTransform == null || mainCamera == null)
            return;

        bool isInAngle = CheckIfInAngleAt(wristTransform, mainCamera.transform, minAngle, maxAngle);

        if (isInAngle == true && isOffHandLook == false)
        {
            isOffHandLook = true;
            Debug.Log("off hand look away invoked");
            onMainHandLookAway?.Invoke();
        }
        else if (isInAngle == false && isOffHandLook == true)
        {
            isOffHandLook = false;
            Debug.Log("off hand look at invoked");
            onMainHandLookAt?.Invoke();
        }
    }

    private bool CheckIfInAngleAt(Transform lookTransform, Transform targetTransform, float minAngle, float maxAngle)
    {
        Vector3 wristToCameraDir = (targetTransform.position - lookTransform.position).normalized;
        Vector3 palmDirection = lookTransform.up;

        float angle = Vector3.Angle(palmDirection, wristToCameraDir);
        return angle > minAngle && angle < maxAngle;
    }

    private OVRHand GetMainOVRHand(OVRPlugin.Hand handType)
    {
        OVRHand[] hands = FindObjectsOfType<OVRHand>();
        foreach (var hand in hands)
        {
            if (hand.GetHand() == handType)
            {
                hand.gameObject.TryGetComponent(out mainHandSkeleton);
                return hand;
            }
        }
        return null;
    }

    private IEnumerator WaitForSkeletonInitialization()
    {
        while (mainHandSkeleton.Bones.Count == 0)
        {
            yield return null;
        }

        GetWristTransform(mainHandSkeleton);
        SetHandCollider(mainHandSkeleton);

        //if (StateManager.instance.CompareState(AppState.Enter))
        //{
        //    StateManager.instance.ChangeState((AppState.SkyChoose));
        //}
    }

    private Transform GetWristTransform(OVRSkeleton skeleton)
    {
        foreach (OVRBone bone in skeleton.Bones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_WristRoot)
            {
                wristTransform = bone.Transform;
                return bone.Transform;
            }
        }
        return null;
    }

    private void SetHandCollider(OVRSkeleton skeleton)
    {
        foreach (OVRBone bone in skeleton.Bones)
        {
            if (bone.Id == colliderBone)
            {
                SphereCollider col =  bone.Transform.gameObject.AddComponent<SphereCollider>();
                HandCollider = bone.Transform.gameObject.AddComponent<HandCollider>();
                //Debug.Log($"HandCollider : {HandCollider}");
                
               // col.includeLayers =LayerMask.GetMask("HandUI");
                col.radius = 0.01f;
                col.isTrigger = true;

                Rigidbody rigidbody = bone.Transform.gameObject.AddComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
               
                //bone.Transform.gameObject.layer = LayerMask.GetMask("HandUI");
                break;
            }
        }
    }
    
    private void HandleButtonInput(float buttonValue, ref bool buttonPressed, Action<float> onButtonPressed, Action onButtonReleased)
    {
        if (buttonValue > 0.1f)
        {
            if (!buttonPressed)
            {
                buttonPressed = true;
                Debug.Log($"{onButtonPressed} has been invoked");
                onButtonPressed?.Invoke(buttonValue);
            }
        }
        else
        {
            if (buttonPressed)
            {
                buttonPressed = false;
                Debug.Log($"{onButtonReleased} has been invoked");
                onButtonReleased?.Invoke();
            }
        }
    }

    private void CheckForPinchGesture()
    {
        if (mainHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            if (!isPinching)
            {
                isPinching = true;
                Debug.Log("Main hand pinch invoked");
                onMainHandPinch?.Invoke();
            }
        }
        else
        {
            if (isPinching)
            {
                isPinching = false;
                Debug.Log("Main hand pinch release invoked");
                onMainHandPinchRelease?.Invoke();
            }
        }
    }
}

