using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetailUICrackedBlock : DetailUI
{
    [SerializeField] private TMP_InputField _currentBreakTimeValue;
    [SerializeField] private TMP_InputField _currentRespawnTimeValue;

    // 양성인 TODO: 이름 바꿔야 함.
    private GCrackedBlock _crackedBlock;

    public void OnBreakTimeChanged()
    {
        float value = ConvertStringToFloat( _currentBreakTimeValue.text );
        _currentBreakTimeValue.text = value.ToString();
        _crackedBlock.BreakTime = value;
    }

    public void OnRespawnTimeChanged()
    {
        float value = ConvertStringToFloat( _currentRespawnTimeValue.text );
        _currentRespawnTimeValue.text = value.ToString();
        _crackedBlock.RespawnTime = value;
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
        _crackedBlock = obj.GetComponent<GCrackedBlock>();
        SetBreakTime( _crackedBlock.BreakTime );
        SetRespawnTime( _crackedBlock.RespawnTime );
    }
}
