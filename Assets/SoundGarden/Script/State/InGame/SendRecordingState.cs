using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendRecordingState : MonoBehaviour, IGameState
{
    private InGameState inGameState;
    public void Enter()
    {
        inGameState.activeSeed.SetSwipeable(true);
        inGameState.activeSeed.seedParticles.SetChargeParticle(true);
        SubscribeEvents();

        if (inGameState.activeSeed != null)
            inGameState.activeSeed.seedUI.SetSendUI(true);
    }
    public void Exit()
    {
        inGameState.activeSeed.SetSwipeable(false);
        inGameState.activeSeed.seedParticles.SetChargeParticle(false);
        UnsubscribeEvents();

        if (inGameState.activeSeed != null)
            inGameState.activeSeed.seedUI.SetAllOff();
    }
    private void SubscribeEvents()
    {
        inGameState.activeSeed.swipeable.onSwipeRight += SendRecording;
        inGameState.activeSeed.swipeable.onSwipeLeft += Rerecord;
    }
    private void UnsubscribeEvents()
    {
        inGameState.activeSeed.swipeable.onSwipeRight -= SendRecording;
        inGameState.activeSeed.swipeable.onSwipeLeft -= Rerecord;
    }
    public void SetInGameState(InGameState _inGameState)
    {
        inGameState = _inGameState;
    }
    public void Rerecord()
    {
        Color color = InstrumentsManager.instance._InstrumentsData.Insts[inGameState.activeSeed.currentInstrument].seedColor;
        inGameState.activeSeed.bubble.gameObject.GetComponent<Renderer>().material.color = color;
        inGameState.ChangeState(GameState.Record);
    }

    public void SendRecording()
    {
        //send the recording to the web
        inGameState.activeSeed.bubble.gameObject.SetActive(false);
        inGameState.activeSeed.bubble.gameObject.GetComponent<Renderer>().material.color = inGameState.activeSeed.baseColor;
        inGameState.activeSeed.seedParticles.TurnAllOff();
        inGameState.activeSeed.seedParticles.SetExplosionParticle(true);
        SoundUploadManager.instance.SendToAI(inGameState.activeSeed);
        inGameState.ChangeState(GameState.ThrowSeed);
    }
}
