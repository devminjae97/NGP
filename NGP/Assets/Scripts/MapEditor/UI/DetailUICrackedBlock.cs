using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetailUICrackedBlock : DetailUI
{

    private void Awake()
    {
        _textArea = _textAreaObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnRespawnTimeChanged()
    {
        base.OnValueChange();

    }
}
