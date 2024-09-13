using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * 브러쉬의 크기가 달라지면 값들을 배열에 넣는게 성능상 낫다고 판단되어 배열 사용
 */
public class EditJobDrawingTile : EditJob
{
    // Dictionary<pos at tilemap, (new tile, old tile)>
    private Dictionary<Vector2Int, (TileBase, TileBase)> _tileByPos;

    public EditJobDrawingTile()
    { 
        _tileByPos = new Dictionary<Vector2Int, (TileBase, TileBase)>();
        _jobType = EJobType.eDrawingTile;
    }

    public override bool IsEmptyJob()
    {
        return _tileByPos.Count == 0;
    }

    public override void Undo()
    {
        foreach (KeyValuePair<Vector2Int, (TileBase, TileBase)> pair in _tileByPos)
        {
            EditorManager.Instance.CurrentEditorScene.tilemap.SetTile( (Vector3Int)pair.Key, pair.Value.Item1 );
        }
    }

    public override void Redo()
    {
        foreach (KeyValuePair<Vector2Int, (TileBase, TileBase)> pair in _tileByPos)
        {
            EditorManager.Instance.CurrentEditorScene.tilemap.SetTile( (Vector3Int)pair.Key, pair.Value.Item2 );
        }
    }

    public Dictionary<Vector2Int, (TileBase, TileBase)> TileByPos
    {
        get { return _tileByPos; }
        set { _tileByPos = value; }
    }
}

