using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AppState
{
    Enter,
    SkyChoose,
    InGame,
    End
}

public class StateManager : Singleton<StateManager>
{
    private Dictionary<AppState, IState> m_appStateDict = new Dictionary<AppState, IState>();
    private IState m_appState;
    public IState _currentAppState { get { return m_appState; } }

    public AppState StartState;
    
    protected void Awake()
    {
        foreach (AppState s in Enum.GetValues(typeof(AppState)))
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GameObject obj = this.transform.GetChild(i).gameObject;

                if (s.ToString().Equals(obj.name))
                {
                    m_appStateDict.Add(s, obj.GetComponent<IState>());
                    Debug.Log($"GameManager :: Add State :: {s}");
                    break;
                }
            }
        }
    }

    private void Start()
    {
        ChangeState(StartState);
    }
    private void ChangeState(IState newState)
    {
        if (m_appState == newState)
            return;

        if (m_appState != null)
            m_appState.Exit();

        m_appState = newState;
        m_appState.Enter();
    }
    public void ChangeState(AppState newState)
    {
        if (m_appStateDict.ContainsKey(newState) == false)
        {
            Debug.LogError($"The dictionary doesn't contain {newState}");
            return;
        }

        Debug.Log($"Cahnge state :::  {newState}");
        ChangeState(m_appStateDict[newState]);
    }

    public bool CompareState(AppState State)
    {
        if (m_appStateDict[State] == _currentAppState)
        {
            return true;
        }

        return false;
    }
}
