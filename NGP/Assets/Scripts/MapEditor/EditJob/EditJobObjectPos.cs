using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditJobObjectPos : EditJob
{
    private Dictionary<Vector2Int, GameObject> _targetObjects;
    private Tilemap _tilemap;
    private float _tileHalfSize;

    public EditJobObjectPos()
    {
        _targetObjects = new Dictionary<Vector2Int, GameObject>();
        _jobType = EJobType.eObjectPos;
    }

    private void Start()
    {
        _tilemap = EditorManager.Instance.CurrentEditorScene.tilemap;
        _tileHalfSize = _tileHalfSize = _tilemap.cellSize.x * 0.5f;
    }

    public override bool IsEmptyJob()
    {
        return _targetObjects.Count == 0;
    }

    public Dictionary<Vector2Int, GameObject> TargetObjects
    {
        get { return _targetObjects; }
        set { _targetObjects = value; }
    }

    public override void Undo()
    {
        foreach (KeyValuePair<Vector2Int, GameObject> pair in _targetObjects)
        {
            pair.Value.SetActive( false );
            pair.Value.transform.position = new Vector3( 100, 100 );
        }
    }

    public override void Redo()
    {
        foreach (KeyValuePair<Vector2Int, GameObject> pair in _targetObjects)
        {
            pair.Value.SetActive( true );
            pair.Value.transform.position = _tilemap.CellToWorld( (Vector3Int)pair.Key ) + new Vector3( _tileHalfSize, _tileHalfSize );
        }
    }

    public override void RemoveJob()
    {
        foreach (KeyValuePair<Vector2Int, GameObject> pair in _targetObjects)
        {
            if (!pair.Value.activeSelf)
                Destroy( pair.Value );
        }
        Destroy( this );
    }
}
