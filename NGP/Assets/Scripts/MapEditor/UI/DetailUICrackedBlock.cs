using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetailUICrackedBlock : DetailUI
{
    [SerializeField] private TextMeshProUGUI _currentBreakTimeValue;
    [SerializeField] private TextMeshProUGUI _currentRespawnTimeValue;
    [SerializeField] private GameObject _breakTimeTextAreaObject;
    // 양성인 TODO: 이름 바꿔야 함.
    private GBreakableBlock _breakableBlock;

    public void OnObjectClick( GameObject obj )
    {
        _breakableBlock = obj.GetComponent<GBreakableBlock>();
        //_currentBreakTimeValue.text = _breakableBlock.BreakTime;
        // SetBreakTime( _breakableBlock.BreakTime );
        // SetRespawnTime( _breakableBlock.RespawnTime );
    }

    public void OnBreakTimeChanged()
    {
        float value = ConvertStringToFloat( _currentBreakTimeValue.text.Substring( 0, _currentBreakTimeValue.text.Length - 1 ) );
        _currentBreakTimeValue.text = value.ToString();
    }

    public void SetBreakTime( float value )
    {
        _currentBreakTimeValue.text = value.ToString();
    }

    public void SetRespawnTime( float value )
    {
        _currentRespawnTimeValue.text = value.ToString();
    }

    public override void SetUIInfo( GameObject obj )
    {
        OnObjectClick( obj );
    }
}
