using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditJobEraser : EditJob
{
    private Dictionary<GameObject, Vector2Int> _eraseObjects;
    private Dictionary<Vector2Int, (TileBase, TileBase)> _tileByPos;
    private Tilemap _tilemap;
    private float _tileHalfSize;

    public EditJobEraser()
    {
        _eraseObjects = new Dictionary<GameObject, Vector2Int>();
    }

    private void Start()
    {
        _tileByPos = new Dictionary<Vector2Int, (TileBase, TileBase)>();
        _eraseObjects = new Dictionary<GameObject, Vector2Int>();   
        _tilemap = EditorManager.Instance.CurrentEditorScene.tilemap;
        _tileHalfSize = _tileHalfSize = _tilemap.cellSize.x * 0.5f;
    }

    public override bool IsEmptyJob()
    {
        return _eraseObjects.Count == 0;
    }

    public override void Undo()
    {
        foreach (KeyValuePair<Vector2Int, (TileBase, TileBase)> pair in _tileByPos)
        {
            EditorManager.Instance.CurrentEditorScene.tilemap.SetTile( (Vector3Int)pair.Key, pair.Value.Item1 );
        }
        foreach (KeyValuePair<GameObject, Vector2Int> pair in _eraseObjects)
        {
            pair.Key.SetActive( true );
            pair.Key.transform.position = EditorManager.Instance.CurrentEditorScene.tilemap.CellToWorld( (Vector3Int)pair.Value ) + new Vector3( _tileHalfSize, _tileHalfSize );
        }
    }

    public override void Redo()
    {
        foreach (KeyValuePair<Vector2Int, (TileBase, TileBase)> pair in _tileByPos)
        {
            _tilemap.SetTile( (Vector3Int)pair.Key, pair.Value.Item2 );
        }

        foreach (KeyValuePair<GameObject, Vector2Int> pair in _eraseObjects)
        {
            pair.Key.SetActive( false );
            pair.Key.transform.position = new Vector3( 100, 100, 0 );
        }
    }

    public override void RemoveJob()
    {
        foreach (KeyValuePair<GameObject, Vector2Int> pair in _eraseObjects)
        {
            if (!pair.Key.activeSelf)
                Destroy( pair.Key );
        }
        Destroy( this );
    }

    public Dictionary<Vector2Int, (TileBase, TileBase)> TileByPos
    {
        get { return _tileByPos; }
        set { _tileByPos = value; }
    }

    public Dictionary<GameObject, Vector2Int> EraseObjects
    {
        get { return _eraseObjects; }
    }
}
