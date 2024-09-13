using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetailUINormalBlock : DetailUI
{
    [SerializeField] private TMP_InputField _sizeTextArea;
    [SerializeField] private EditorGround _ground;

    private void Start()
    {
        _ground.OnSizeChanged += Instance_OnSizeChanged;
    }

    private void OnEnable()
    {
        _sizeTextArea.text = _ground.Size.ToString();
    }

    public void OnSizeChanged()
    {
        int value = ConvertStringToInt( _sizeTextArea.text );
        if (value > _ground.MaxSize) value = (int)_ground.MaxSize;
        else if (value < 1) value = 1;
        _sizeTextArea.text = value.ToString();
        _ground.SetSize( value );
    }

    public void Instance_OnSizeChanged( int size )
    {
        _sizeTextArea.text = size.ToString();
    }
}
