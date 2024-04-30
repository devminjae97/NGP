using System.Threading.Tasks;
using EnumDefines;
using UnityEngine;

public class GTimerButton : GButton
{
    // @TODO, base의 타입을 가리는법?
    //    [HideInInspector] public ButtonType _type;
    [SerializeField] protected float _activatingTime;
    
    protected override void Initialize()
    {
        base.Initialize();
        
        // 강제고정. type을 inspector에서 숨기는법 찾을 때까지 유지
        _type = ButtonType.Press;
    }

    public override void Activate()
    {
        if (IsActivated)
        {
            return;
        }
        
        base.Activate();
        TimerAsync();
    }
    
    protected async Task TimerAsync()
    {
        int millisecond = (int)(_activatingTime * 1000);
        await Task.Delay( millisecond);
        
        Deactivate();
    }
}
