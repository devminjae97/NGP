using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{

    #region Serialize Field >>>>

    [SerializeField] private bool _activateOnPhaseStart = false;
    public bool ActivateOnPhaseStart => _activateOnPhaseStart;

    #endregion

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


    #region Delegates >>>>

    public delegate void OnStateChangedDelegate(ItemBase item, bool inIsActivated);

    public OnStateChangedDelegate OnStateChanged;
    //protected OnStateChangedDelegate _onStateChanged;
    //public OnStateChangedDelegate OnStateChanged => _onStateChanged;

    #endregion Delegates >>>>


    #region Unity ####

    private void Awake()
    {
        Initialize();
    }

    protected void OnDestroy()
    {
        Deinitialize();
    }

    #endregion Unity ####
}
