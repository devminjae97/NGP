using UnityEngine;

[DisallowMultipleComponent]
public class IngamePhase : PhaseBase
{
    private GimmickManager _gimmickManager;
    
    protected override void Initialize()
    {
        _gimmickManager ??= GimmickManager.Instance;
    }

    protected override void StartPhase()
    {
        // @TODO, PlayerCharacter & Controller Check & On
        
        // Activate Gimmicks
        _gimmickManager.ActivateAllGimmicks(onPhaseStart: true);
        
        // @TODO, cover ui fade out?
    }
}
