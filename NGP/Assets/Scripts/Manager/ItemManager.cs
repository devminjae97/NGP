using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ItemManager : Singleton<ItemManager>
{
    //@TODO, GameInstanceSubsystem 같은 느낌의 무언가가 필요한가?

    private List<ItemBase> _items;

    public void RegisterItem(ItemBase item)
    {
        if (_items.Contains(item))
        {
            return;
        }

        _items.Add(item);
    }

    public void ResetAllItem()
    {
        foreach (var item in _items)
        {
            item.Reset();
        }
    }

    public void ActivateAllItems(bool onPhaseStart = false)
    {
        foreach (var item in _items)
        {
            if (onPhaseStart && item.ActivateOnPhaseStart == false)
            {
                continue;
            }

            item.Activate();
        }
    }

    public void DeactivateAllItems()
    {
        foreach (var item in _items)
        {
            item.Deactivate();
        }
    }

    private void Awake()
    {
        dontDestroy = false;
        _items = new();
    }

    protected override void OnDestroy()
    {
        _items.Clear();
        base.OnDestroy();
    }
}
