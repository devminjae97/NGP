using SaveDataContainer;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSaveManager : Singleton<MapSaveManager>
{
    // 양성인 TODO: Define 폴더의 파일에 옮겨야 함
    const string dataPath = "SaveData/EditorMapData.json";

    private EditorMapData _editorMapData;
    
    private Tilemap[] _tilemap;
    private Vector2 _bottomLeft;
    private Vector2 _topRight;

    private void Awake()
    {
        _editorMapData = new EditorMapData();
        _tilemap = new Tilemap[2];
    }

    public void SaveDataToContainer()
    {
        SaveTile();
        SaveGimmick();
        SaveToJson();
    }

    void SaveTile()
    {
        _tilemap = EditorManager.Instance.EditorTilemap;
        _bottomLeft = EditorManager.Instance.EditorScene[0].bottomLeft;
        _topRight = EditorManager.Instance.EditorScene[0].topRight;

        SaveTileAtScene( 0 );
        SaveTileAtScene( 1 );
    }

    void SaveTileAtScene( int sceneNum )
    {
        Tilemap curTilemap = _tilemap[sceneNum];
        SaveDataContainer.NormalBlockData normalBlockData = new SaveDataContainer.NormalBlockData();
        normalBlockData.poses = new List<Vector2Int>();
        Vector3Int startCell = curTilemap.WorldToCell( _bottomLeft );
        Vector3Int endCell = curTilemap.WorldToCell( _topRight );
        for (int i = startCell.x; i <= endCell.x; i++)
        {
            for (int j = startCell.y; j <= endCell.y; j++)
            {
                if (curTilemap.HasTile( new Vector3Int( i, j, 0 ) ))
                {
                    normalBlockData.poses.Add( new Vector2Int( i, j ) );
                }
            }
        }
        _editorMapData.normalBlock[sceneNum] = normalBlockData;
    }

    void SaveGimmick()
    {
        SaveGimmickAtScene( 0 );
        SaveGimmickAtScene( 1 );
    }

    void SaveGimmickAtScene( int sceneNum )
    {
        foreach (Transform child in EditorManager.Instance.EditorTilemap[sceneNum].transform)
        {
            switch (child.tag)
            {
                case "Respawn":
                    _editorMapData.flag[sceneNum].startFlagPos = (Vector2Int)_tilemap[sceneNum].WorldToCell( child.position );
                    break;
                case "Finish":
                    _editorMapData.flag[sceneNum].goalFlagPos = (Vector2Int)_tilemap[sceneNum].WorldToCell( child.position );
                    break;
                case "CrackedBlock":
                    // 양성인 TODO: class 생기면 수정
                    GCrackedBlock crackedBlock = child.GetComponent<GCrackedBlock>();
                    CrackedBlockInfo crackedBlockInfo = new CrackedBlockInfo( crackedBlock.RespawnTime, (Vector2Int)_tilemap[sceneNum].WorldToCell( child.position ) );
                    _editorMapData.crackedBlock[sceneNum].infos.Add( crackedBlockInfo );
                    break;
                default:
                    break;
            }
        }
    }

    void SaveToJson()
    {
        string jsonData = JsonUtility.ToJson( _editorMapData, true );
        string path = Path.Combine( Application.dataPath, dataPath );
        File.WriteAllText( path, jsonData );
    }

    public void OnClickSaveMapButton()
    {
        SaveDataToContainer();
    }
}
