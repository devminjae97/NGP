using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAsset : MonoBehaviour
{
    public static MasterAsset instance;

    private CharacterMovement _charcterMovement1;
    private CharacterMovement _charcterMovement2;

    private CameraController _camera1;
    private CameraController _camera2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _camera1 = GameObject.Find("Camera1").GetComponent<CameraController>();
            _camera2 = GameObject.Find("Camera2").GetComponent<CameraController>();
            _camera1.enabled = true;
            _camera2.enabled = true;

            _charcterMovement1 = GameObject.Find("MC1").GetComponent<CharacterMovement>();
            _charcterMovement2 = GameObject.Find("MC2").GetComponent<CharacterMovement>();


        }
    }
    private void Update()
    {
        if (IsStageCompleted())
        {
            _charcterMovement1.SetCharacterSpeed(0f);
            _charcterMovement2.SetCharacterSpeed(0f);
            _charcterMovement1.SetCharacterControllability(false);
            _charcterMovement2.SetCharacterControllability(false);
            //_camera1.IsActive(false);
            //_camera2.IsActive(false);
        }

    }

    public bool IsStageCompleted()
    {
        return _charcterMovement1.IsOnGoal() && _charcterMovement2.IsOnGoal();
    }
}
