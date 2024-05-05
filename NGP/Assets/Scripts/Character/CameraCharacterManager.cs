using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCharacterManager : Singleton<CameraCharacterManager>
{ 
    [SerializeField] protected CharacterMovement _charcter1;
    [SerializeField] protected CharacterMovement _charcter2;

    [SerializeField] protected CameraController _camera1;
    [SerializeField] protected CameraController _camera2;

    private void Awake()
    {
       if(_camera1 != null && _camera2 != null && _charcter1 != null && _charcter2 != null)
        {
            _camera1.enabled = true;
            _camera2.enabled = true;

            _charcter1.enabled = true;
            _charcter2.enabled = true;
        }
    }

    private void Update()
    {
        if (IsStageCompleted())
        {
            _charcter1.SetCharacterSpeed(0f);
            _charcter2.SetCharacterSpeed(0f);
            _charcter1.SetCharacterControllability(false);
            _charcter2.SetCharacterControllability(false);
            //_camera1.IsActive(false);
            //_camera2.IsActive(false);
        }
    }

    public bool IsStageCompleted()
    {
        return _charcter1.IsOnGoal() && _charcter2.IsOnGoal();
    }
}
