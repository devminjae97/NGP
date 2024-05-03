using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public enum EUIMode
{
    eSelect,    // 개별 선택 (선택한 오브젝트 값 수정)
    eSet,       // 앞으로 설치할 것 (프리팹 값 수정)
}

public class DetailUI : MonoBehaviour
{
    protected bool _isPointerDownTabBar;
    protected Vector3 _posGapMouseAndUI;
    protected bool _isOpen;

    private void Update()
    {
        if (_isPointerDownTabBar)
        {
            gameObject.transform.position = Input.mousePosition + _posGapMouseAndUI;
        }
    }

    public void OnPointerDownTabBar()
    {
        _posGapMouseAndUI = gameObject.transform.position - Input.mousePosition;
        _isPointerDownTabBar = true;
    }

    public void OnPointerUpTabBar()
    {
        _isPointerDownTabBar = false;
    }

    protected float ConvertStringToFloat( string str )
    {
        return float.Parse( str );
    }

    protected int ConvertStringToInt( string str )
    {
        return int.Parse( str );
    }

    public virtual void SetUIInfo( GameObject obj ) { }

    public bool IsOpen
    {
        get { return _isOpen; }
        set { _isOpen = value; }
    }
}
