using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTest : MonoBehaviour
{
    [SerializeField] private Color onColor = Color.green;
    [SerializeField] private Color offColor = Color.red;
    [SerializeField] private Color enterColor = Color.blue;

    Swipeable swipeable;
    Material material;
    private void Start()
    {
        swipeable = GetComponent<Swipeable>();
        swipeable.onSwipeRight += SwipeRight;
        swipeable.onSwipeLeft += SwipeLeft;
        swipeable.onEnter += OnEnter;

        material = GetComponent<Renderer>().material;
    }

    private void SwipeRight()
    {
        Debug.Log("Swipe test swiped right");
        material.color = onColor;
    }
    private void SwipeLeft()
    {
        Debug.Log("Swipe test swiped left");
        material.color = offColor;
    }
    private void OnEnter()
    {
        material.color = enterColor;
    }
}
