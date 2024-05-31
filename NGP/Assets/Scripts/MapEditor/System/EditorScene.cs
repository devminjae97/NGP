using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditorScene : MonoBehaviour
{
    public Vector3 cameraPos;
    public Tilemap tilemap;
    // 양성인 TODO: Flag에 관한 스크립트로 바꿔야 함
    public GameObject startFlag;
    public GameObject goalFlag;
    public Vector2 topRight;
    public Vector2 bottomLeft;
}
