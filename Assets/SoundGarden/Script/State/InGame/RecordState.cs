using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RecordState : MonoBehaviour, IGameState
{
    [SerializeField] private float timelineScale = 0.2f;

    private InGameState inGameState;
    [SerializeField] private MusicTimeline musicTimeline;

    //need one for yes and no
    bool isRecording = false;

    public void Enter()
    {
        isRecording = false;
        SubscribeEvents();
        CreateTimelineUI();
    }
    public void Exit()
    {
        isRecording = false;
        UnsubscribeEvents();
        HideTimelineUI();
    }
    private void SubscribeEvents()
    {
        //inGameState.onMicArrived += CreateTimelineUI;
        PlayerInputHandler.instance.onOffHandTriggerPressed += OnTriggerPressed;
        PlayerInputHandler.instance.onOffHandTriggerReleased += OnTriggerReleased;
    }

    private void UnsubscribeEvents()
    {
        //inGameState.onMicArrived -= CreateTimelineUI;
        PlayerInputHandler.instance.onOffHandTriggerPressed -= OnTriggerPressed;
        PlayerInputHandler.instance.onOffHandTriggerReleased -= OnTriggerReleased;
    }

    private void CreateTimelineUI()
    {
        Debug.Log("Create timeline UI created");
        //get timeline
        if(musicTimeline == null)
            musicTimeline = Instantiate(ResourceManager.instance.resourceScriptable.musicTimeline);

        //set initial things
        musicTimeline.gameObject.SetActive(false);
        musicTimeline.transform.SetParent(this.transform);
        musicTimeline.transform.position = inGameState.flowerMic.sphereCollider.transform.position;
        musicTimeline.SetFollowStats(inGameState.flowerMic.sphereCollider.transform, Vector3.zero);
        musicTimeline.transform.localScale = Vector3.zero;

        //make it appear 
        musicTimeline.gameObject.SetActive(true);
        musicTimeline.StartFollow();
        //musicTimeline.transform.DOScale(Vector3.one * timelineScale, 0.5f);
        musicTimeline.transform.localScale = Vector3.one * timelineScale;
    }
    private void HideTimelineUI()
    {
        if (musicTimeline == null)
            return;

        musicTimeline.transform.localScale = Vector3.zero;
    }

    public void SetInGameState(InGameState _inGameState)
    {
        inGameState = _inGameState;
    }

    private void OnTriggerPressed(float triggerVal)
    {
        Debug.Log("Trigger was pressed");

        if (musicTimeline != null)
        {
            musicTimeline.OnTriggerPressed();
        }

        SoundManager.Instance.onStartLoop += RecordingStart;
    }

    private void OnTriggerReleased()
    {
        Debug.Log("Trigger was released");

        if (musicTimeline != null)
        {
            musicTimeline.OnTriggerReleased();
        }

        if (isRecording == true)
        {
            OnRecordingEnd();
            //switch to next state
        }
        SoundManager.Instance.onStartLoop -= RecordingStart;
    }

    private void RecordingStart()
    {
        if (musicTimeline != null)
        {
            musicTimeline.OnRecordStart();
        }
        isRecording = true;
        //start recording
        RecordingManager.instance.StartRecording();
    }

    private void OnRecordingEnd()
    {
        if (musicTimeline != null)
        {
            musicTimeline.OnRecordEnd();
        }
        isRecording = false;
        SoundManager.Instance.onStartLoop -= RecordingStart;
        //stop recording 
        RecordingManager.instance.StopRecording();

        //switch to next state
        inGameState.ChangeState(GameState.SendRecording);
    }
}
