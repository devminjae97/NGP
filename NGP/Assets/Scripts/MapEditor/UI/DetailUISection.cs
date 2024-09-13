using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailUISection : MonoBehaviour
{
    private bool _isOpened;
    private RectTransform _rectTransform;

    private void Start()
    {
        _isOpened = false;
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = new Vector2( -470, 0 );
    }

    public void OnCollapseButtonClick()
    {
        if (_isOpened)
        {
            _isOpened = false;
            _rectTransform.anchoredPosition = new Vector2( -470, 0 );
        }
        else
        {
            _isOpened = true;
            _rectTransform.anchoredPosition = new Vector2( 0, 0 );
        }
    }
}
