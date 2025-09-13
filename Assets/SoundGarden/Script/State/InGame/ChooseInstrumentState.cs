using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseInstrumentState : MonoBehaviour, IGameState
{
    [SerializeField] private Vector3 seedOffSet = new Vector3(0f, 0.1f, 0f);
    [SerializeField] private float seedFollowSpeed = 0.1f;
    [SerializeField] private Seed currentTempSeed;

    [SerializeField] private float distanceToActiveSeed = 0.3f;

    [SerializeField] private AudioClip bubbleAppear;

    private int currentInstrumentInd = 0;
    private int instrumentCount = 0;

    private InGameState inGameState;

    public void Enter()
    {
        currentInstrumentInd = 0;
        GetInstrumentCount();
        SubscribeEvents();
        inGameState.GetFlowerMic().SetColliderOn(true);
    }
    public void Exit()
    {
        UnsubscribeEvents();
        inGameState.GetFlowerMic().SetColliderOn(false);
        SubscribeToSeedSwipe(false, inGameState.activeSeed);

        if (inGameState.activeSeed != null)
            inGameState.activeSeed.seedUI.SetAllOff();
    }
    private void SubscribeEvents()
    {
        PlayerInputHandler.instance.onMainHandLookAt += CreatePreviewSeed;
        PlayerInputHandler.instance.onMainHandLookAway += DestroyPreviewSeed;
        PlayerInputHandler.instance.onMainHandPinch += CreateActiveSeed;
    }
    private void UnsubscribeEvents()
    {
        PlayerInputHandler.instance.onMainHandLookAt -= CreatePreviewSeed;
        PlayerInputHandler.instance.onMainHandLookAway -= DestroyPreviewSeed;
        PlayerInputHandler.instance.onMainHandPinch -= CreateActiveSeed;
    }
    public void SetInGameState(InGameState _inGameState)
    {
        inGameState = _inGameState;
    }
    private void CreatePreviewSeed()
    {
        //check to see if hand is too close to active seed
        if(inGameState.activeSeed != null)
        {
            Debug.Log($"distance to active seed is {Vector3.Distance(PlayerInputHandler.instance.wristTransform.position, inGameState.activeSeed.transform.position)}");

            if (Vector3.Distance(PlayerInputHandler.instance.wristTransform.position, inGameState.activeSeed.transform.position) < distanceToActiveSeed)
            {
                Debug.Log("distance is too small, returning");
                return;
            }
        }

        Debug.Log("Create preview seed");
        currentTempSeed = GameManager.instance.seedObjectPool.Spawn(PlayerInputHandler.instance.wristTransform, seedOffSet);
        Debug.Log($"PlayerInputHandler is null is {PlayerInputHandler.instance}");
        Debug.Log($"Player wrist is null is {PlayerInputHandler.instance.wristTransform}");
        Debug.Log($"Current temp seed is null is {currentTempSeed}");
        Debug.Log($"SeedOffSet {seedOffSet}");
        Debug.Log($"SeedFollowSpeed {seedFollowSpeed}");
        currentTempSeed.StartFollowOffset(PlayerInputHandler.instance.wristTransform, seedOffSet, seedFollowSpeed);
    }

    private void DestroyPreviewSeed()
    {
        Debug.Log("Destroy preview seed");

        if (currentTempSeed == null)
            return;

        currentTempSeed.DestroySeed();
        currentTempSeed = null;
    }
    private void CreateActiveSeed()
    {
        if (currentTempSeed == null)
            return;

        if(inGameState.activeSeed != null)
            SubscribeToSeedSwipe(false, inGameState.activeSeed);

        currentTempSeed.StopFollow();
        currentTempSeed.GrowBubble();
        inGameState.SetActiveSeed(currentTempSeed);
        SoundManager.Instance.PlayOneShot(currentTempSeed.gameObject, bubbleAppear);

        SubscribeToSeedSwipe(true, inGameState.activeSeed);

        currentTempSeed = null;
    }

    //Instrument select
    private void GetInstrumentCount()
    {
        instrumentCount = InstrumentsManager.instance._InstrumentsData.Insts.Count;
    }

    private void SubscribeToSeedSwipe(bool isSubscribe, Seed seed)
    {
        if (seed == null)
            return;

        if(isSubscribe)
        {
            seed.swipeable.isSwipeable = true;
            seed.swipeable.onSwipeLeft += OnSwipeLeft;
            seed.swipeable.onSwipeRight += OnSwipeRight;
            seed.swipeable.onEnter += OnSwipeEnter;
            seed.swipeable.onExit += OnSwipeExit;
            seed.onGrab += OnGrab;
        }
        else
        {
            seed.swipeable.isSwipeable = false;
            seed.swipeable.onSwipeLeft -= OnSwipeLeft;
            seed.swipeable.onSwipeRight -= OnSwipeRight;
            seed.swipeable.onEnter -= OnSwipeEnter;
            seed.swipeable.onExit -= OnSwipeExit;
            seed.onGrab -= OnGrab;
        }
    }

    private void OnSwipeEnter()
    {
        if (inGameState.activeSeed == null || inGameState.activeSeed.isGrabbed)
            return;

        inGameState.activeSeed.seedUI.SetSwipeUI(true);
    }

    private void OnSwipeExit()
    {
        if (inGameState.activeSeed == null || inGameState.activeSeed.isGrabbed)
            return;

        inGameState.activeSeed.seedUI.SetSwipeUI(false);
    }

    private void OnSwipeLeft()
    {
        UpdateInstrument(false);
    }

    private void OnSwipeRight()
    {
        UpdateInstrument(true);
    }

    private void OnGrab()
    {
        if(inGameState.activeSeed == null)
            return;

        inGameState.activeSeed.seedUI.SetSwipeUI(false);
    }

    private void UpdateInstrument(bool isRight)
    {
        if(isRight)
        {
            currentInstrumentInd += 1;
            if (currentInstrumentInd == instrumentCount)
                currentInstrumentInd = 0;
        }
        else
        {
            currentInstrumentInd -= 1;
            if (currentInstrumentInd < 0)
                currentInstrumentInd = instrumentCount - 1;
        }

        inGameState.activeSeed.SetSeedInstrument(currentInstrumentInd);
    }
}
