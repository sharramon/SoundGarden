using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandCollider : MonoBehaviour
{
    public Action<Collider, Transform> collisionAction = delegate { };
    public Action<bool> collisionExitAction = delegate { };
    public Action<bool> swipeAction = delegate { };

    private bool enterCollision = false;
    private Swipeable currentSwipeable;
    private float leaveColliderTime = 0.0f;
    private Vector3 initialWristPosition;
    private float swipeThreshold = 0.05f;
    private float extraTime = 2.0f;

    private void Start()
    {
        // Assuming PlayerInputHandler is a Singleton or accessible statically
    }

    private void OnTriggerEnter(Collider other)
    {
        enterCollision = true;
        collisionAction?.Invoke(other, transform);

        // Maybe switch this to layers
        var swipeable = other.gameObject.GetComponent<Swipeable>();
        if (swipeable != null && swipeable.isSwipeable == true)
        {
            currentSwipeable = swipeable;
            initialWristPosition = PlayerInputHandler.instance.wristTransform.position;
            swipeable.OnEnter();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CheckSwipe();
    }

    private void OnTriggerExit(Collider other)
    {
        enterCollision = false;

        if (currentSwipeable != null)
        {
            leaveColliderTime = Time.time;
            StartCoroutine(ExtraTimeCheck(other));
        }
        
        collisionExitAction?.Invoke(false);
    }

    private void CheckSwipe()
    {
        if (currentSwipeable != null)
        {
            Vector3 wristMovement = PlayerInputHandler.instance.wristTransform.position - initialWristPosition;
            float localRightMovement = Vector3.Dot(PlayerInputHandler.instance.wristTransform.forward, wristMovement);
            Debug.Log($"Local right movement: {localRightMovement}");

            if (Mathf.Abs(localRightMovement) >= swipeThreshold)
            {
                if (localRightMovement < 0)
                {
                    Debug.Log("Swipe detected to the right within the collider");
                    SwipeRight();
                }
                else
                {
                    Debug.Log("Swipe detected to the left within the collider");
                    SwipeLeft();
                }
                ResetSwipe();
            }
        }
    }
    
    private IEnumerator ExtraTimeCheck(Collider other)
    {
        float startTime = Time.time;
        while (Time.time - startTime < extraTime)
        {
            if (enterCollision && other.gameObject.GetComponent<Swipeable>() == currentSwipeable)
            {
                // Re-entered the same collider, continue tracking
                //yield break;
            }

            Vector3 wristMovement = PlayerInputHandler.instance.wristTransform.position - initialWristPosition;
            float localRightMovement = Vector3.Dot(PlayerInputHandler.instance.wristTransform.forward, wristMovement);
            Debug.Log($"Local right movement: {localRightMovement}");
            if (Mathf.Abs(localRightMovement) >= swipeThreshold)
            {
                if (localRightMovement < 0)
                {
                    Debug.Log("Swipe detected to the right outside the collider within extra time");
                    SwipeRight();
                }
                else
                {
                    Debug.Log("Swipe detected to the left outside the collider within extra time");
                    SwipeLeft();
                }
                ResetSwipe();
                yield break;
            }

            yield return null;
        }

        // Extra time is over, reset everything
        Debug.Log("Extra time over, resetting swipe detection");
        ResetSwipe();
    }

    private void ResetSwipe()
    {
        initialWristPosition = Vector3.zero;

        if (currentSwipeable == null)
            return;

        currentSwipeable.OnExit();
        currentSwipeable = null;
    }

    private void SwipeRight()
    {
        // Call your method for swiping right
        Debug.Log("Swiped right");
        // Your code here
        if(currentSwipeable != null)
            currentSwipeable.SwipeRight();
    }

    private void SwipeLeft()
    {
        // Call your method for swiping left
        Debug.Log("Swiped left");
        // Your code here
        if(currentSwipeable != null)
            currentSwipeable.SwipeLeft();
    }
}
