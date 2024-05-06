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

    /**
     * @t9100,
     * Pause가 적용되는 async 함수를 사용하기 위해선
     * UnityLifeCycle의 영향을 받는 UniTask를 사용해야 한다.
     * 반대로 Pause와 무관하게 async 함수를 구현하고자 하면 그냥 Task를 사용하자.
     */
    
    public void Pause()
        => Time.timeScale = 0;
    
    public void Resume()
        => Time.timeScale = 1;
}
