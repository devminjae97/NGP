using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using static UnityEngine.Networking.UnityWebRequest;
using static UnityEngine.UI.Image;

public enum EGroundTileType
{
    eNormal,
    eIce,
}

public class EditorGround : EditorToolBase
{
    public TileBase groundTile;
    public TileBase iceTile;
    public GameObject tileTypesButton;
    EGroundTileType currentTileType;

    TileBase currentTile;

    public delegate void OnSizeChangedDelegate( int size );
    public event OnSizeChangedDelegate OnSizeChanged;

    private void Start()
    {
        InitComponent( 1 );
        currentTile = groundTile;
        currentTileType = EGroundTileType.eNormal;
    }

    public override void SetSize(float val)
    {
        val *= _tileSize;

        _size = val;
        if (_size > _tileSize * 10) _size = _tileSize * 10;
        else if (_size < _tileSize) _size = _tileSize;
        _selectCursor.transform.localScale = new Vector3( _size, _size, 0 );
        OnSizeChanged?.Invoke( (int)_size );
    }

    public override void AddSize( float val )
    {
        val *= _tileSize;

        _size += val;
        if (_size > _tileSize * 10) _size = _tileSize * 10;
        else if (_size < _tileSize) _size = _tileSize;
        _selectCursor.transform.localScale = new Vector3( _size, _size, 0 );
        OnSizeChanged?.Invoke( (int)_size );
    }

    public override void SetCursorColor( Vector2 mousePosition, SpriteRenderer selectCursor )
    {
        bool isEven = (int)(_size / _tileSize) % 2 == 0;
        int num = Mathf.RoundToInt( (_size / _tileSize - (isEven ? 0 : 1)) / 2 );
        Vector3Int pos = _tilemap.WorldToCell( mousePosition );
        for (int i = pos.x - num; i <= pos.x + num + (isEven ? -1 : 0); i++)
        {
            for (int j = pos.y - num; j <= pos.y + num + (isEven ? -1 : 0); j++)
            {
                (bool, RaycastHit2D) result = IsBlockedByObject( i, j );
                if (result.Item1)
                {
                    selectCursor.color = Color.red;
                    return;
                }
            }
        }
        selectCursor.color = Color.white;
    }

    public override void Edit( Vector2 mousePosition, EditJob editJob )
    {
        bool isEven = (int)(_size / _tileSize) % 2 == 0;
        int num = Mathf.RoundToInt( (_size / _tileSize - (isEven ? 0 : 1)) / 2 );
        Vector3Int pos = _tilemap.WorldToCell( mousePosition );
        Vector2Int curPos;

        for (int i = pos.x - num; i <= pos.x + num + (isEven ? -1 : 0); i++)
        {
            for (int j = pos.y - num; j <= pos.y + num + (isEven ? -1 : 0); j++)
            {
                (bool, RaycastHit2D) result = IsBlockedByObject( i, j );
                if (result.Item1)
                {
                    return;
                }

                curPos = new Vector2Int( i, j );
                TileBase curTile = _tilemap.GetTile( (Vector3Int)curPos );
                if (curTile != currentTile)
                {
                    (editJob as EditJobDrawingTile).TileByPos.Add( curPos, (curTile, currentTile) );
                    _tilemap.SetTile( new Vector3Int( i, j, 0 ), currentTile );
                }
            }
        }
    }

    public void OnClickGroundButton()
    {
        base.OnClickButton();
        _editorController.InitButtonGroup( ETileType.eGround, this );
        _selectCursor.transform.localScale = new Vector3( _size, _size, 0 );
    }

    public void OnClickTileTypesButton()
    {
        tileTypesButton.SetActive( !tileTypesButton.activeSelf );
    }

    public void OnClickTileTypeButton( int type )
    {
        currentTileType = (EGroundTileType)type;
        switch ((EGroundTileType)type)
        {
            case EGroundTileType.eNormal:
                currentTile = groundTile;
                break;
            case EGroundTileType.eIce:
                currentTile = iceTile;
                break;
            default:
                currentTile = groundTile;
                break;
        }
        OnClickTileTypesButton();
    }
}
