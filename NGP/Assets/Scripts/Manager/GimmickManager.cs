using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GimmickManager : Singleton<GimmickManager>
{
    //@TODO, GameInstanceSubsystem 같은 느낌의 무언가가 필요한가?

    private List<GimmickBase> _gimmicks;

    public void RegisterGimmick(GimmickBase gimmick)
    {
        if (_gimmicks.Contains(gimmick))
        {
            return;
        }
        
        _gimmicks.Add(gimmick);
    }

    public void ResetAllGimmicks()
    {
        foreach (var gimmick in _gimmicks)
        {
            gimmick.Reset();
        }
    }
    
    public void ActivateAllGimmicks(bool onPhaseStart = false)
    {
        foreach (var gimmick in _gimmicks)
        {
            if (onPhaseStart && gimmick.ActivateOnPhaseStart == false)
            {
                continue;
            }
            
            gimmick.Activate();
        }
    }
    
    public void DeactivateAllGimmicks()
    {
        foreach (var gimmick in _gimmicks)
        {
            gimmick.Deactivate();
        }
    }
    
    private void Awake()
    {
        dontDestroy = false;
        _gimmicks = new();
    }

    protected override void OnDestroy()
    { 
        _gimmicks.Clear();
        base.OnDestroy();
    }
}
