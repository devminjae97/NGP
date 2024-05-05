using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : Singleton<GameManager>
{
    private PhaseBase _lobbyPhase;
    private PhaseBase _ingamePhase;
    //private PhaseBase _settingPhase;    // need?
    
    void GameFlow()
    {
        
    }
    
    
}
