using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState : IState
{
    void SetInGameState(InGameState inGameState);
}
