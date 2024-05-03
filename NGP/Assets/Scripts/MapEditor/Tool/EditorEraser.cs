using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class EditorEraser : EditorToolBase
{
    public delegate void OnSizeChangedDelegate( int size );
    public event OnSizeChangedDelegate OnSizeChanged;

    private void Start()
    {
        InitComponent( 1 );
    }

    public override void AddSize( float val )
    {
        _size += val;
        if (_size > _maxSize) _size = _maxSize;
        else if (_size < _tileSize) _size = _tileSize;
        _selectCursor.transform.localScale = new Vector3( _size, _size, 0 );
        OnSizeChanged?.Invoke( (int)_size );
    }

    public override void SetSize( float val )
    {
        _size = val;
        if (_size > _maxSize) _size = _maxSize;
        else if (_size < _tileSize) _size = _tileSize;
        _selectCursor.transform.localScale = new Vector3( _size, _size, 0 );
        OnSizeChanged?.Invoke( (int)_size );
    }

    public override void Edit( Vector2 mousePosition, EditJob editJob )
    {
        bool isEven = (int)(_size / _tileSize) % 2 == 0;
        int num = Mathf.RoundToInt( (_size / _tileSize - (isEven ? 0 : 1)) / 2 );
        Vector3Int pos = _tilemap.WorldToCell( mousePosition );
        Vector2Int curPos;
        editJob.JobType = EJobType.eErase;

        for (int i = pos.x - num; i <= pos.x + num + (isEven ? -1 : 0); i++)
        {
            for (int j = pos.y - num; j <= pos.y + num + (isEven ? -1 : 0); j++)
            {
                curPos = new Vector2Int( i, j );
                TileBase curTile = _tilemap.GetTile( (Vector3Int)curPos );
                if (curTile != null )
                {
                    (editJob as EditJobEraser).TileByPos.Add( curPos, (curTile, null) );
                    _tilemap.SetTile( new Vector3Int( i, j, 0 ), null );
                }

                (bool, RaycastHit2D) result = IsBlockedByObject( i, j );
                if (result.Item1 && result.Item2.transform.gameObject.activeSelf)
                {
                    (editJob as EditJobEraser).EraseObjects.Add( result.Item2.transform.gameObject, curPos );

                    result.Item2.transform.position = new Vector3( 100, 100 );
                    result.Item2.transform.gameObject.SetActive( false );
                }
            }
        }
    }

    public void OnClickEraserButton()
    {
        base.OnClickButton();
        _editorController.InitButtonGroup( ETileType.eEraser, this );
        _selectCursor.transform.localScale = new Vector3( _size, _size, 0 );
    }
}
