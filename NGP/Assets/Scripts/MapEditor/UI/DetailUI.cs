using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class DetailUI : MonoBehaviour
{
    private bool _isPointerDownTabBar;
    private Vector3 _posGapMouseAndUI;
    private bool _isOpen;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _currentValue;
    [SerializeField] private GameObject _textAreaObject;
    private TextMeshProUGUI _textArea;

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

    public void OnClickApply()
    {
        int value = int.Parse( _textArea.text.Substring( 0, _textArea.text.Length - 1 ) );
        _currentValue.text = value.ToString();
        // 정해지면 추가
        // CrackedGround.timeToRespawn = value;
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
