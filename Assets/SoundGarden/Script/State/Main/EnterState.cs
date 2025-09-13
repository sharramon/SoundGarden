using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnterState : MonoBehaviour, IState
{
    
    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void OnSceneLoad()
    {
        StartCoroutine(GetNextState());
    }

    private IEnumerator GetNextState()
    {
        yield return new WaitForSeconds(1f);

        StateManager.instance.ChangeState(AppState.SkyChoose);
    }
}
