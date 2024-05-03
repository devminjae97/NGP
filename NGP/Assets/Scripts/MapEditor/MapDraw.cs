using SaveDataContainer;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDraw : MonoBehaviour
{
    [SerializeField] private TileBase _normalBlock;
    [SerializeField] private GameObject _startFlag;
    [SerializeField] private GameObject _goalFlag;
    [SerializeField] private GameObject _crackedBlock;
    private Tilemap _tilemap;
    private float _tileSize;
    private float _distFloat;
    private int _distCell;
    private float _tileHalfSize;
    private Vector3 _distBetweenScene;

    private void Start()
    {
        if (MapLoadManager.Instance.TilemapToLoad != null)
        {
            _tilemap = MapLoadManager.Instance.TilemapToLoad;
        }
        else
        {
            _tilemap = EditorManager.Instance.EditorTilemap[0];
        }
        _tileSize = _tilemap.cellSize.x;
        _tileHalfSize = _tileSize * 0.5f;
        _distFloat = 60 * 1.5f;
        _distCell = (int)(60 * 1.5f / _tileSize);
        _distBetweenScene = new Vector3( 0, _distFloat, 0 );
    }

    public void DrawNormalGround( EditorMapData editorMapData )
    {
        _tilemap.ClearAllTiles();

        // 양성인 TODO: 60(30 - (-30)), 1.5f는 두 맵을 떨어 뜨려놓기 위한 값. 하드 코딩이므로 추후 어떻게 수정할지 생각.
        foreach (Vector2Int pos in editorMapData.normalBlock[0].poses)
        {
            _tilemap.SetTile( (Vector3Int)pos + new Vector3Int( 0, _distCell, 0 ), _normalBlock );
        }

        foreach (Vector2Int pos in editorMapData.normalBlock[1].poses)
        {
            _tilemap.SetTile( (Vector3Int)pos, _normalBlock );
        }
    }

    public void DrawNormalGroundAtEditor( EditorMapData editorMapData )
    {
        Tilemap tilemap1 = EditorManager.Instance.EditorTilemap[0];
        Tilemap tilemap2 = EditorManager.Instance.EditorTilemap[1];
        tilemap1.ClearAllTiles();
        tilemap2.ClearAllTiles();

        // 양성인 TODO: 60(30 - (-30)), 1.5f는 두 맵을 떨어 뜨려놓기 위한 값. 하드 코딩이므로 추후 어떻게 수정할지 생각.
        foreach (Vector2Int pos in editorMapData.normalBlock[0].poses)
        {
            tilemap1.SetTile( (Vector3Int)pos, _normalBlock );
        }

        foreach (Vector2Int pos in editorMapData.normalBlock[1].poses)
        {
            tilemap2.SetTile( (Vector3Int)pos, _normalBlock );
        }
    }

    public void DrawFlag( EditorMapData editorMapData )
    {
        Vector3 startPos;
        Vector3 goalPos;

        startPos = _tilemap.CellToWorld( (Vector3Int)editorMapData.flag[0].startFlagPos ) + _distBetweenScene + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
        goalPos = _tilemap.CellToWorld( (Vector3Int)editorMapData.flag[0].goalFlagPos ) + _distBetweenScene + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
        Instantiate( _startFlag, startPos, Quaternion.identity );
        Instantiate( _goalFlag, goalPos, Quaternion.identity );
        startPos = _tilemap.CellToWorld( (Vector3Int)editorMapData.flag[1].startFlagPos ) + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
        goalPos = _tilemap.CellToWorld( (Vector3Int)editorMapData.flag[1].goalFlagPos ) + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
        Instantiate( _startFlag, startPos, Quaternion.identity );
        Instantiate( _goalFlag, goalPos, Quaternion.identity );
    }

    public void DrawFlagAtEditor( EditorMapData editorMapData )
    {
        Vector3 startPos;
        Vector3 goalPos;
        Tilemap tilemap1 = EditorManager.Instance.EditorTilemap[0];
        Tilemap tilemap2 = EditorManager.Instance.EditorTilemap[1];

        startPos = tilemap1.CellToWorld( (Vector3Int)editorMapData.flag[0].startFlagPos ) + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
        goalPos = tilemap1.CellToWorld( (Vector3Int)editorMapData.flag[0].goalFlagPos ) + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
        GameObject startFlag1 = Instantiate( _startFlag, startPos, Quaternion.identity );
        GameObject goalFlag1 = Instantiate( _goalFlag, goalPos, Quaternion.identity );
        startFlag1.transform.parent = tilemap1.transform;
        goalFlag1.transform.parent = tilemap1.transform;

        startPos = tilemap2.CellToWorld( (Vector3Int)editorMapData.flag[1].startFlagPos ) + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
        goalPos = tilemap2.CellToWorld( (Vector3Int)editorMapData.flag[1].goalFlagPos ) + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
        GameObject startFlag2 = Instantiate( _startFlag, startPos, Quaternion.identity );
        GameObject goalFlag2 = Instantiate( _goalFlag, goalPos, Quaternion.identity );
        startFlag2.transform.parent = tilemap2.transform;
        goalFlag2.transform.parent = tilemap2.transform;
    }

    public void DrawCrackedBlock( EditorMapData editorMapData )
    {
        Vector3 pos;
        float respawnTime;
        GCrackedBlock crackedBlock;

        foreach (CrackedBlockInfo data in editorMapData.crackedBlock[0].infos)
        {
            pos = _tilemap.CellToWorld( (Vector3Int)data.pos ) + _distBetweenScene + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
            respawnTime = data.respawnTime;
            crackedBlock = Instantiate( _crackedBlock, pos, Quaternion.identity ).GetComponent<GCrackedBlock>();
            crackedBlock.RespawnTime = respawnTime;
        }
        foreach (CrackedBlockInfo data in editorMapData.crackedBlock[1].infos)
        {
            pos = _tilemap.CellToWorld( (Vector3Int)data.pos ) + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
            respawnTime = data.respawnTime;
            crackedBlock = Instantiate( _crackedBlock, pos, Quaternion.identity ).GetComponent<GCrackedBlock>();
            crackedBlock.RespawnTime = respawnTime;
        }
    }

    public void DrawCrackedBlockAtEditor( EditorMapData editorMapData )
    {
        Vector3 pos;
        float respawnTime;
        GCrackedBlock crackedBlock;
        Tilemap tilemap1 = EditorManager.Instance.EditorTilemap[0];
        Tilemap tilemap2 = EditorManager.Instance.EditorTilemap[1];

        foreach (CrackedBlockInfo data in editorMapData.crackedBlock[0].infos)
        {
            pos = tilemap1.CellToWorld( (Vector3Int)data.pos ) + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
            respawnTime = data.respawnTime;
            crackedBlock = Instantiate( _crackedBlock, pos, Quaternion.identity ).GetComponent<GCrackedBlock>();
            crackedBlock.RespawnTime = respawnTime;
            crackedBlock.transform.parent = tilemap1.transform;
        }
        foreach (CrackedBlockInfo data in editorMapData.crackedBlock[1].infos)
        {
            pos = tilemap2.CellToWorld( (Vector3Int)data.pos ) + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
            respawnTime = data.respawnTime;
            crackedBlock = Instantiate( _crackedBlock, pos, Quaternion.identity ).GetComponent<GCrackedBlock>();
            crackedBlock.RespawnTime = respawnTime;
            crackedBlock.transform.parent = tilemap2.transform;
        }
    }
}
