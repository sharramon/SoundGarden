using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Start,
    ChooseInstrument,
    Record,
    SendRecording,
    ThrowSeed,
    End
}
public class InGameState : MonoBehaviour, IState
{
    private Dictionary<GameState, IGameState> m_gameStateDict = new Dictionary<GameState, IGameState>();
    private IGameState m_currentState;
    public IGameState _currentState { get { return m_currentState; } }
    public Seed activeSeed { get; private set; }
    private FlowerMic _flowerMic;
    public FlowerMic flowerMic {get { return _flowerMic; }}

    public Action onMicArrived;

    private List<GameObject> flowers = new List<GameObject>();
    [SerializeField] int butterflyFlowerCount = 3;
    [SerializeField] private GameObject butterflyPrefab;
    private bool isButterflyCreated = false;

    private void Awake()
    {
        foreach (GameState s in Enum.GetValues(typeof(GameState)))
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GameObject obj = this.transform.GetChild(i).gameObject;

                if (s.ToString().Equals(obj.name))
                {
                    IGameState gameState = obj.GetComponent<IGameState>();
                    m_gameStateDict.Add(s, gameState);
                    gameState.SetInGameState(this);
                    Debug.Log($"GameManager :: Add State :: {s}");
                    break;
                }
            }
        }
    }

    private void Start()
    {
        GetFlowerMic();
    }

    public Seed SetActiveSeed(Seed _activeSeed)
    {
        if (activeSeed != null)
            activeSeed.DestroySeed();

        activeSeed = _activeSeed;
        activeSeed.seedParticles.SetIdleParticle(true);
        activeSeed.onSetToMic += OnMicPlaced;
        activeSeed.onMicArrived += OnMicArrived;
        return activeSeed;
    }

    public Seed FreeActiveSeed()
    {
        Seed freedSeed = activeSeed;
        activeSeed = null;
        return freedSeed;
    }

    public void Enter()
    {
        ChangeState(GameState.ChooseInstrument);
    }
    public void Exit()
    {

    }

    private void ChangeState(IGameState newState)
    {
        if (m_currentState == newState)
            return;

        if (m_currentState != null)
            m_currentState.Exit();

        m_currentState = newState;
        m_currentState.Enter();
    }
    public void ChangeState(GameState newState)
    {
        if (m_gameStateDict.ContainsKey(newState) == false)
        {
            Debug.LogError($"The dictionary doesn't contain {newState}");
            return;
        }

        Debug.Log($"GameManager :: Change State :: {newState}");
        ChangeState(m_gameStateDict[newState]);
    }

    public FlowerMic GetFlowerMic()
    {
        this.gameObject.TryGetComponent<FlowerMic>(out _flowerMic);

        if (_flowerMic == null)
            _flowerMic = this.gameObject.AddComponent<FlowerMic>();

        return _flowerMic;
    }

    private void OnMicPlaced()
    {
        ChangeState(GameState.Record);
    }

    private void OnMicArrived()
    {
        Debug.Log("on mic arrived called from in game state");
        onMicArrived?.Invoke();
    }

    public void AddFlower(GameObject flower)
    {
        if(flowers.Contains(flower))
            return;

        flowers.Add(flower);

        Seed seed;
        if(flower.TryGetComponent<Seed>(out seed))
        {
            seed.onGrowFlower -= AddFlower;
        }

        if (flowers.Count > butterflyFlowerCount && isButterflyCreated == false)
        {
            CreateButterflies();
        }
    }

    private void CreateButterflies()
    {
        isButterflyCreated = true;
        GameObject butterfly = Instantiate(butterflyPrefab);
        butterfly.transform.position = new Vector3(0, 0, 0);
    }
}
