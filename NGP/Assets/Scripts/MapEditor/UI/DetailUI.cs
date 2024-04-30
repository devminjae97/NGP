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
    [SerializeField] protected TextMeshProUGUI _title;
    [SerializeField] protected TextMeshProUGUI _currentValue;
    [SerializeField] protected GameObject _textAreaObject;
    protected TextMeshProUGUI _textArea;

    private void Awake()
    {
        _textArea = _textAreaObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (_isPointerDownTabBar)
        {
            gameObject.transform.position = Input.mousePosition + _posGapMouseAndUI;
        }
    }

    public void OnValueChange()
    {
        int value = int.Parse( _textArea.text.Substring( 0, _textArea.text.Length - 1 ) );
        _currentValue.text = value.ToString();
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

    public void SetTitle( string titleToSet)
    {
        _title.text = titleToSet;
    }

    public void SetCurrentValueFloat( float value )
    {
        _currentValue.text = value.ToString();
    }

    public void SetTextAreaVisibility( bool isVisible )
    {
        _textAreaObject.SetActive( isVisible );
    }

    public bool IsOpen
    {
        get { return _isOpen; }
        set { _isOpen = value; }
    }
}
