using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class EditorEraser : EditorToolBase
{
    private void Start()
    {
        InitComponent( 1 );
    }

    public override void SetSize( float val )
    {
        _size += val;
        if (_size > _tileSize * 10) _size = _tileSize * 10;
        else if (_size < _tileSize) _size = _tileSize;
        _selectCursor.transform.localScale = new Vector3( _size, _size, 0 );
    }

    public override void Edit( Vector2 mousePosition )
    {
        bool isEven = (int)(_size / _tileSize) % 2 == 0;
        int num = Mathf.RoundToInt( (_size / _tileSize - (isEven ? 0 : 1)) / 2 );
        Vector3Int pos = _tilemap.WorldToCell( mousePosition );

        for (int i = pos.x - num; i <= pos.x + num + (isEven ? -1 : 0); i++)
        {
            for (int j = pos.y - num; j <= pos.y + num + (isEven ? -1 : 0); j++)
            {
                _tilemap.SetTile( new Vector3Int( i, j, 0 ), null );

                (bool, RaycastHit2D) result = IsBlockedByObject( i, j );
                if (result.Item1)
                {
                    Destroy( result.Item2.transform.gameObject );
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
