using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSeedState : MonoBehaviour, IGameState
{
    [SerializeField] private float seedTargetSize = 1f;
    [SerializeField] private float throwForceMultiply = 5f;
    private InGameState inGameState;
    public void Enter()
    {
        GrowSeed();
        SubscribeEvents();
        inGameState.activeSeed.onGrowFlower += inGameState.AddFlower;
    }
    public void Exit()
    {
        UnsubscribeEvents();
    }
    private void SubscribeEvents()
    {
        inGameState.activeSeed.onGrab += GrabSeed;
        inGameState.activeSeed.onRelease += ReleaseSeed;
    }
    private void UnsubscribeEvents()
    {
        inGameState.activeSeed.onGrab -= GrabSeed;
        inGameState.activeSeed.onRelease -= ReleaseSeed;
    }
    public void SetInGameState(InGameState _inGameState)
    {
        inGameState = _inGameState;
    }

    private void GrowSeed()
    {
        inGameState.activeSeed.transform.DOScale(Vector3.one * seedTargetSize, 0.2f);
        inGameState.activeSeed.MakeGrabbable(true);
        inGameState.activeSeed.isGrowable = true;
        
    }

    private void GrabSeed()
    {
        Debug.Log("Seed grabbed in throw seed state");
        inGameState.activeSeed.StopFollow(0);
    }

    private void ReleaseSeed()
    {
        Debug.Log("Seed released in release seed");
        inGameState.activeSeed.rigidBody.isKinematic = false;
        inGameState.activeSeed.rigidBody.useGravity = true;

        StartCoroutine(AddForceCoroutine());

        inGameState.ChangeState(GameState.ChooseInstrument);
    }

    private IEnumerator AddForceCoroutine()
    {
        yield return null;
        inGameState.activeSeed.rigidBody.AddForce(inGameState.activeSeed.rigidBody.velocity * throwForceMultiply, ForceMode.Impulse);
        inGameState.FreeActiveSeed();
    }
}
