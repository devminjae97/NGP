using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class EditorCrackedBlock : EditorToolBase
{
    [SerializeField] private GameObject _crackedBlockObject;

    private void Start()
    {
        InitComponent( 1 );
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
                if (_tilemap.HasTile(new Vector3Int( i, j, 0 )))
                {
                    selectCursor.color = Color.red;
                    return;
                }
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

        for (int i = pos.x - num; i <= pos.x + num + (isEven ? -1 : 0); i++)
        {
            for (int j = pos.y - num; j <= pos.y + num + (isEven ? -1 : 0); j++)
            {
                // check tile
                if (_tilemap.HasTile( new Vector3Int( i, j, 0 ) ))
                {
                    return;
                }
                // check object
                (bool, RaycastHit2D) result = IsBlockedByObject( i, j );
                if (result.Item1)
                {
                    return;
                }
            }
        }
        DrawObject( _crackedBlockObject, pos, editJob );
    }

    public void OnClickCrackedBlockButton()
    {
        base.OnClickButton();
        _editorController.InitButtonGroup( ETileType.eCrackedBlock, this );
        _selectCursor.transform.localScale = new Vector3( _size, _size, 0 );
    }

    public void OnClickRespawnTimeButton()
    {
        OnClickToolOptionButton();
        _detailUI.SetTextAreaVisibility( true );
        _detailUI.SetTitle( "Respawn Time" );
        // 양성인 TODO: 스크립트에 따라 생성
        // _detailUI.SetCurrentValueFloat( _crackedBlockObject );
    }
}
