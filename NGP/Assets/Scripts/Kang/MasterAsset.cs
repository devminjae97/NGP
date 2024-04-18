using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAsset : MonoBehaviour
{
    public static MasterAsset instance;

    private Camera camera1; // ù ��° ī�޶�
    private Camera camera2; // �� ��° ī�޶�

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            camera1 = GameObject.Find("Camera1").GetComponent<Camera>();
            camera2 = GameObject.Find("Camera2").GetComponent<Camera>();
            camera1.enabled = true;
            camera2.enabled = true;
            SetCameraRect();
        }
    }
    public void SetCameraRect()
    {
        camera1.rect = new Rect(0f, 0.5f, 1f, 0.5f); // ��� ����
        camera2.rect = new Rect(0f, 0f, 1f, 0.5f);   // �ϴ� ����
    }
}
