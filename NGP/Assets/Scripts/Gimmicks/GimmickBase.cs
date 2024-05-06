using UnityEngine;

[DisallowMultipleComponent]
public abstract class GimmickBase : MonoBehaviour
{
    /**
     * 다른 요소와 헷깔리지 않기 위해 GimmickBase를 상속받는 클래스는 G를 붙입니다.
     * (e.g., Button)
     */

    #region Serialize Field >>>>

    [SerializeField] private bool _activateOnPhaseStart = false;
    public bool ActivateOnPhaseStart => _activateOnPhaseStart;

    #endregion
    
    /** CAUTION: DO NOT use this variable directly. */
    private bool _isActivated = false;
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

    public bool IsPaused { get; private set; } = false;

    public bool IsWorking => IsActivated && IsPaused;

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
        RegisterToManager();
    }
    
    protected virtual void Deinitialize()
    {
        Reset();
    }

    private void RegisterToManager()
    {
        GimmickManager.Instance.RegisterGimmick(this);
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
    
    #endregion Delegates >>>>
    
    
    #region Unity ####

    private void Awake()
    {
        _uid = GetNewGimmickUID();
        Initialize();
    }
    
    protected void OnDestroy()
    {
        Deinitialize();
    }

    #endregion Unity ####
}
