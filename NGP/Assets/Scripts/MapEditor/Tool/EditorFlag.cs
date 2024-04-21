using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum EFlagType
{
    eStart,
    eGoal,
}

public class EditorFlag : EditorToolBase
{
    private bool _isSet;
    [SerializeField] private GameObject _startFlag;
    [SerializeField] private GameObject _goalFlag;
    private EFlagType _flagType;

    private void Start()
    {
        InitComponent( 1 );
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
        DrawFlag( pos );
    }

    private void DrawFlag( Vector3Int pos )
    {
        switch (_flagType)
        {
            case EFlagType.eStart:
                DrawStartFlag( pos );
                break;
            case EFlagType.eGoal:
                DrawGoalFlag( pos );
                break;
            default:
                break;
        }
    }

    void DrawStartFlag( Vector3Int pos )
    {
        if (EditorManager.GetInstance().CurrentEditorScene.startFlag != null)
        {
            Destroy( EditorManager.GetInstance().CurrentEditorScene.startFlag );
        }
        GameObject obj = Instantiate( _startFlag, _tilemap.CellToWorld( pos ) + new Vector3( _tileSize / 2, _tileSize / 2 ), Quaternion.identity );
        EditorManager.GetInstance().CurrentEditorScene.startFlag = obj;
        obj.transform.parent = EditorManager.GetInstance().CurrentEditorScene.tilemap.transform;
    }

    void DrawGoalFlag( Vector3Int pos )
    {
        if (EditorManager.GetInstance().CurrentEditorScene.goalFlag != null)
        {
            Destroy( EditorManager.GetInstance().CurrentEditorScene.goalFlag );
        }
        GameObject obj = Instantiate( _goalFlag, _tilemap.CellToWorld( pos ) + new Vector3( _tileSize / 2, _tileSize / 2 ), Quaternion.identity );
        EditorManager.GetInstance().CurrentEditorScene.goalFlag = obj;
        obj.transform.parent = EditorManager.GetInstance().CurrentEditorScene.tilemap.transform;
    }

    public void OnClickFlagButton()
    {
        _editorController.InitButtonGroup( ETileType.eFlag, this );
        _selectCursor.transform.localScale = new Vector3( _size, _size, 0 );
    }

    public void OnClickFlagTypeButton( int type )
    {
        _flagType = (EFlagType)type;
    }
}
