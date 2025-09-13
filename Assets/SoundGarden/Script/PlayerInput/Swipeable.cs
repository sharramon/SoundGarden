using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipeable : MonoBehaviour
{
    public bool isSwipeable = false;

    public Action onSwipeRight;
    public Action onSwipeLeft;
    public Action onEnter;
    public Action onExit;

    public void SwipeRight()
    {
        Debug.Log("Swipeable swiped Right");
        onSwipeRight?.Invoke();
    }

    public void SwipeLeft()
    {
        Debug.Log("Swipeable swiped left");
        onSwipeLeft?.Invoke();
    }
    public void OnEnter()
    {
        onEnter?.Invoke();
    }
    public void OnExit()
    {
        onExit?.Invoke();
    }
}
