using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EJobType
{
    eDrawingTile,
    eObjectPos,
    eErase,
}

public abstract class EditJob : MonoBehaviour
{
    protected EJobType _jobType;

    public EJobType JobType
    {
        get { return _jobType; }
        set { _jobType = value; }
    }
    public virtual bool IsEmptyJob() { return false; }
    public virtual void RemoveJob() { } 
    public abstract void Undo();
    public abstract void Redo();
}
