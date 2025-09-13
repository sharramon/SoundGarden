using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicTimeline : MonoBehaviour
{
    [SerializeField] private List<Image> timelineImages = new List<Image>();
    [SerializeField] private Color baseColor = Color.cyan;
    [SerializeField] private Color onColor = Color.green;
    [SerializeField] private Color offColor = Color.yellow;

    private FollowOffset followOffset;

    protected virtual void Start()
    {
        GetFolowOffset();
    }

    protected virtual void OnEnable()
    {
        SetColor(baseColor);
    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void Update()
    {
        UpdateTimeline();
    }

    protected virtual void UpdateTimeline()
    {
        if(timelineImages.Count == 0)
        {
            Debug.LogError("timeline image is null");
            return;
        }

        timelineImages[0].fillAmount = (float)SoundManager.Instance.currentBeatCount / (float)SoundManager.Instance.totalBeats;
        this.transform.LookAt(PlayerInputHandler.instance.mainCamera.transform);
    }

    protected void SetColor(Color color)
    {
        foreach(Image image in timelineImages)
        {
            image.color = color;
        }
    }

    private void GetFolowOffset()
    {
        if (followOffset != null)
            return;

        this.gameObject.TryGetComponent<FollowOffset>(out followOffset);

        if (followOffset == null)
            followOffset = this.gameObject.AddComponent<FollowOffset>();
    }

    public void SetFollowStats(Transform targetTransform, Vector3 Offset = default, float maxSpeed = Mathf.Infinity)
    {
        GetFolowOffset();
        followOffset.SetFollowStats(targetTransform, Offset, maxSpeed);
    }

    public void StartFollow()
    {
        followOffset.StartFollow();
    }

    public void OnTriggerPressed()
    {
        if (timelineImages.Count == 0)
            return;

        SetColor(offColor);
    }
    public void OnTriggerReleased()
    {
        if (timelineImages.Count == 0)
            return;

        SetColor(baseColor);
    }
    public void OnRecordStart()
    {
        SetColor(onColor);
    }
    public void OnRecordEnd()
    {
        SetColor(baseColor);
    }
}
