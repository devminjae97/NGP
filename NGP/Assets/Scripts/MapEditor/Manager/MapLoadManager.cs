using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLoadManager : Singleton<MapLoadManager>
{
    const string dataPath = "SaveData/EditorMapData.json";

    private EditorMapData _editorMapData;
    [SerializeField] private MapDraw _mapDraw;
    [SerializeField] private Tilemap _tilemapToLoad;

    private void Awake()
    {
        _editorMapData = new EditorMapData();
    }

    [ContextMenu( "From Json Data" )]
    public void LoadDataToContainer()
    {
        LoadFromJson();

        _mapDraw.DrawNormalGround( _editorMapData );
        _mapDraw.DrawFlag( _editorMapData );
        _mapDraw.DrawCrackedBlock( _editorMapData );
    }

    [ContextMenu( "From Json Data" )]
    public void LoadDataToContainerAtEditor()
    {
        LoadFromJson();

        _mapDraw.DrawNormalGroundAtEditor( _editorMapData );
        _mapDraw.DrawFlagAtEditor( _editorMapData );
        _mapDraw.DrawCrackedBlockAtEditor( _editorMapData );
        EditorManager.Instance.EditorTilemap[1].gameObject.SetActive( false );
    }

    void LoadFromJson()
    {
        // 양성인 TODO: 앞으로 임의 코드에 대응하는 Json파일을 가져와야함
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
