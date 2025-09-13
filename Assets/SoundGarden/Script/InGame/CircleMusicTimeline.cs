using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CircleMusicTimeline : MusicTimeline
{
    [SerializeField] private List<GameObject> timelineCircleImages = new List<GameObject>();
    public float circleMaxSize = 1000f;

    protected override void Update()
    {
        transform.LookAt(PlayerInputHandler.instance.mainCamera.transform);
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        SetAllCircleOff();
        SoundManager.Instance.onBeatUpdate += UpdateTimeline;
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        SetAllCircleOff();
        SoundManager.Instance.onBeatUpdate -= UpdateTimeline;
    }

    protected override void UpdateTimeline()
    {
        SetAllCircleOff();
        ShowCircle(SoundManager.Instance.currentBeatCount - 1);
        Debug.Log($"Current circle is {SoundManager.Instance.currentBeatCount - 1}");
    }

    private void ShowCircle(int circleInd)
    {
        GameObject targetCircleImage = timelineCircleImages[circleInd];
        targetCircleImage.transform.localScale = Vector3.one * circleMaxSize;
        targetCircleImage.SetActive(true);

        targetCircleImage.transform.DOScale(Vector3.zero, 50f / SoundManager.Instance.bpm);
    }

    private void SetAllCircleOff()
    {
        foreach (GameObject circleImage in timelineCircleImages)
        {
            circleImage.gameObject.SetActive(false);
        }
    }
}
