using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLoadAtEditorManager : MonoBehaviour
{
    private static MapLoadAtEditorManager instance;

    const string dataPath = "SaveData/EditorMapData.json";

    private EditorMapData _editorMapData;
    [SerializeField] private MapDraw _mapDraw;
    [SerializeField] private Tilemap _tilemapToLoad;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy( gameObject );
        }

        _editorMapData = new EditorMapData();
    }

    public static MapLoadAtEditorManager GetInstance()
    {
        return instance;
    }

    [ContextMenu( "From Json Data" )]
    public void LoadDataToContainer()
    {
        LoadFromJson();

        _mapDraw.DrawNormalGround( _editorMapData );
        _mapDraw.DrawFlag( _editorMapData );
        _mapDraw.DrawCrackedBlock( _editorMapData );
    }

    void LoadFromJson()
    {
        // �缺�� TODO: ������ ���� �ڵ忡 �����ϴ� Json������ �����;���
        string path = Path.Combine( Application.dataPath, dataPath );
        string jsonData = File.ReadAllText( path );
        _editorMapData = JsonUtility.FromJson<EditorMapData>( jsonData );
    }

    public EditorMapData EditorMapData
    {
        get { return _editorMapData; }
        set { _editorMapData = value; }
    }

    public Tilemap TilemapToLoad
    {
        get { return _tilemapToLoad; }
        set { _tilemapToLoad = value; }
    }
}