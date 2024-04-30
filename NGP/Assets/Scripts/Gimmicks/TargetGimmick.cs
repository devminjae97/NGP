
using System;
using System.Collections.Generic;
using EnumDefines;
using UnityEngine;

public abstract class TargetGimmick : GimmickBase
{
    [SerializeField] protected List<GButton> _requiredButtons;
    [SerializeField] protected ActivatePolicy _activatePolicy = ActivatePolicy.ConditionMetOnly;

    private HashSet<GButton> _boundButtonSet = new();

    protected override void Initialize()
    {
        base.Initialize();
        
        // @TODO, lifecycle에 맞춰서 init, deinit 해주는 구조 짜기

        if (_activatePolicy == ActivatePolicy.ConditionMetOnly || _activatePolicy == ActivatePolicy.Both)
        {
            _boundButtonSet = new();
            foreach (var button in _requiredButtons)
            {
                if (button == null)
                {
                    continue;
                }

                // @TODO, delegate에 =로 할당하는거 어떻게 막더라
                //button.OnStateChanged = OnRequiredButtonStateChanged;

                if (_boundButtonSet.Contains(button))
                {
                    continue;
                }

                button.OnStateChanged += OnRequiredButtonStateChanged;
                _boundButtonSet.Add(button);
            }
        }
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
        
        foreach (var button in _boundButtonSet)
        {
            if (button != null)
            {
                button.OnStateChanged -= OnRequiredButtonStateChanged;
            }
        }
    }

    protected virtual void OnRequiredButtonStateChanged(GimmickBase gimmick/*ignore*/, bool isOn/*ignore*/)
    {
        bool shouldActivate = true;
        foreach (var button in _requiredButtons)
        {
            if (button.IsActivated == false)
            {
                shouldActivate = false;
                break;
            }
        }

        if (shouldActivate != IsActivated)
        {
            if (shouldActivate)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }
    }

    public override void Activate()
    {
        if (_activatePolicy == ActivatePolicy.ManualActivateOnly || _activatePolicy == ActivatePolicy.Both)
        {
            base.Activate();
        }
    }

    public override void Deactivate()
    {
        if (_activatePolicy == ActivatePolicy.ManualActivateOnly || _activatePolicy == ActivatePolicy.Both)
        {
            base.Deactivate();
        }
    }
}
