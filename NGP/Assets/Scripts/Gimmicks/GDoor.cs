using System.Collections.Generic;
using UnityEngine;

public class GDoor : TargetGimmick
{
    protected void Open()
    {
        Debug.Log($"[{nameof(GDoor)}.{nameof(Open)}] {gameObject.name}");
    }

    protected void Close()
    {
        Debug.Log($"[{nameof(GDoor)}.{nameof(Close)}] {gameObject.name}");
    }
    
    public override void Activate()
    {
        base.Activate();
        
        if (IsActivated)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
