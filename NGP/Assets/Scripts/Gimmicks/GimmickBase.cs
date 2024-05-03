using UnityEngine;

[DisallowMultipleComponent]
public abstract class GimmickBase : MonoBehaviour
{
    /**
     * 다른 요소와 헷깔리지 않기 위해 GimmickBase를 상속받는 클래스는 G를 붙입니다.
     * (e.g., Button)
     */
    
    // DO NOT use this variable directly.
    private bool _isActivated = false;

    public ETileType tileType;

    public bool IsActivated 
    {
        get => _isActivated;
        set
        {
            if (_isActivated != value)
            {
                _isActivated = value;
                enabled = value;
                OnStateChanged?.Invoke(null, value);
            }
        }
    }

    public virtual void Activate()
    { 
        IsActivated = true;
    }

    public virtual void Deactivate()
    {
        IsActivated = false;
    }
    
    public virtual void Reset()
    {
        Deactivate();
    }

    protected virtual void Initialize()
    {
        Reset();
    }
    
    protected virtual void Deinitialize()
    {
        Reset();
    }
    
    #region ID >>>>

    protected int _uid = -1;

    public int UID => _uid;
    
    private static int _generatedGimmicksCount = 0;

    private static int GetNewGimmickUID()
    {
        return ++_generatedGimmicksCount; 
    }

    #endregion ID >>>>
    

    #region Delegates >>>>
    
    public delegate void OnStateChangedDelegate(GimmickBase gimmick, bool inIsActivated);

    public OnStateChangedDelegate OnStateChanged;
    //protected OnStateChangedDelegate _onStateChanged;
    //public OnStateChangedDelegate OnStateChanged => _onStateChanged;

    #endregion Delegates >>>>
    
    
    #region Unity ####

    private void Awake()
    {
        _uid = GetNewGimmickUID();
        Initialize();
        
        // Test
        Debug.Log($"[{this.name}] uid: {_uid}");
    }
    
    protected void OnDestroy()
    {
        Deinitialize();
    }

    #endregion Unity ####
}
