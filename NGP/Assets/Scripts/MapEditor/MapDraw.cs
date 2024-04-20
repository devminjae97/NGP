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

    private void Start()
    {
        _tilemap = MapLoadManager.GetInstance().TilemapToLoad;
        _tileSize = _tilemap.cellSize.x;
        _distCell = (int)(60 * 1.5f / _tileSize);
        _distFloat = 60 * 1.5f;
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

    public void DrawFlag( EditorMapData editorMapData )
    {
        Vector3 startPos;
        Vector3 goalPos;

        startPos = _tilemap.CellToWorld( (Vector3Int)editorMapData.flag[0].startFlagPos ) + new Vector3( _tileSize * 0.5f, _tileSize * 0.5f ) + new Vector3( 0, _distFloat, 0 );
        goalPos = _tilemap.CellToWorld( (Vector3Int)editorMapData.flag[0].goalFlagPos ) + new Vector3( _tileSize * 0.5f, _tileSize * 0.5f ) + new Vector3( 0, _distFloat, 0 );
        Instantiate( _startFlag, startPos, Quaternion.identity );
        Instantiate( _goalFlag, goalPos, Quaternion.identity );
        startPos = _tilemap.CellToWorld( (Vector3Int)editorMapData.flag[1].startFlagPos ) + new Vector3( _tileSize * 0.5f, _tileSize * 0.5f ) + new Vector3( 0, _distFloat, 0 );
        goalPos = _tilemap.CellToWorld( (Vector3Int)editorMapData.flag[1].goalFlagPos ) + new Vector3( _tileSize * 0.5f, _tileSize * 0.5f ) + new Vector3( 0, _distFloat, 0 );
        Instantiate( _startFlag, startPos, Quaternion.identity );
        Instantiate( _goalFlag, goalPos, Quaternion.identity );
    }

    public void DrawCrackedBlock( EditorMapData editorMapData )
    {
        Vector3 pos;
        float respawnTime;
        CrackedBlockTest crackedBlock;
        foreach (CrackedBlockInfo data in editorMapData.crackedBlock[0].infos)
        {
            pos = _tilemap.CellToWorld( (Vector3Int)data.pos );
            respawnTime = data.respawnTime;
            crackedBlock = Instantiate( _crackedBlock, pos, Quaternion.identity ).GetComponent<CrackedBlockTest>();
            crackedBlock.respawnTime = respawnTime;
        }
        foreach (CrackedBlockInfo data in editorMapData.crackedBlock[1].infos)
        {
            pos = _tilemap.CellToWorld( (Vector3Int)data.pos ) + new Vector3( _tileSize * 0.5f, _tileSize * 0.5f ) + new Vector3( 0, _distFloat, 0 );
            respawnTime = data.respawnTime;
            crackedBlock = Instantiate( _crackedBlock, pos, Quaternion.identity ).GetComponent<CrackedBlockTest>();
            crackedBlock.respawnTime = respawnTime;
        }
    }
}
