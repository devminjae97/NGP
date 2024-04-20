using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAsset : MonoBehaviour
{
    public static MasterAsset instance;

    private Camera _camera1; // 첫 번째 카메라
    private Camera _camera2; // 두 번째 카메라

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _camera1 = GameObject.Find("Camera1").GetComponent<Camera>();
            _camera2 = GameObject.Find("Camera2").GetComponent<Camera>();
            _camera1.enabled = true;
            _camera2.enabled = true;
            SetCameraRect();
        }
    }
    public void SetCameraRect()
    {
        _camera1.rect = new Rect(0f, 0.5f, 1f, 0.5f); // 상단 영역
        _camera2.rect = new Rect(0f, 0f, 1f, 0.5f);   // 하단 영역
    }
}
