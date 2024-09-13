using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetailUIEraser : DetailUI
{
    [SerializeField] private TMP_InputField _sizeTextArea;
    [SerializeField] private EditorEraser _eraser;

    private void Start()
    {
        _eraser.OnSizeChanged += Instance_OnSizeChanged;
    }

    private void OnEnable()
    {
        _sizeTextArea.text = _eraser.Size.ToString();
    }

    public void OnSizeChanged()
    {
        int value = ConvertStringToInt( _sizeTextArea.text );
        if (value > _eraser.MaxSize) value = (int)_eraser.MaxSize;
        else if (value < 1) value = 1;
        _sizeTextArea.text = value.ToString();
        _eraser.SetSize( value );
    }

    public void Instance_OnSizeChanged( int size )
    {
        _sizeTextArea.text = size.ToString();
    }
}
