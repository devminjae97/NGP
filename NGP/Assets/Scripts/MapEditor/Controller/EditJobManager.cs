using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditJobManager : MonoBehaviour
{
    static private EditJobManager instance;
    private Deque<EditJob> _undos;
    private Deque<EditJob> _redos;
    private Tilemap _tilemap;
    private float _tileSize;
    private float _tileHalfSize;
    private Dictionary<GameObject, Vector2Int> _objectPos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy( gameObject );
        }

        _undos = new Deque<EditJob>();
        _redos = new Deque<EditJob>();
    }

    public static EditJobManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        _tilemap = EditorManager.GetInstance().CurrentEditorScene.tilemap;
        _tileSize = _tilemap.cellSize.x;
        _tileHalfSize = _tileSize * 0.5f;
        _objectPos = new Dictionary<GameObject, Vector2Int>();
    }

    public EditJob Undo()
    {
        if (_undos.IsEmpty()) return null;
        EditJob job = _undos.GetBack();
        _undos.PopBack();
        _redos.PushBack( job );
        ExcuteUndoJob( job );
        return job;
    }

    public EditJob Redo()
    {
        if (_redos.IsEmpty()) return null;
        EditJob job = _redos.GetBack();
        _redos.PopBack();
        _undos.PushBack( job );
        ExcuteRedoJob( job );
        return job;
    }
    
    public void PushJob( EditJob job )
    {
        if (_undos.IsFull())
        {
            EditJob jobToPop = _undos.GetFront();
            // 양성인 TODO: Save 하기 전에 SetActive(false)로 되어 있는 job은 모두 삭제해야 한다.
            RemoveUselessJob( jobToPop );
            _undos.PopFront();
        }
        _undos.PushBack( job );
        _redos.Clear();
    }

    public void ExcuteUndoJob( EditJob job )
    {
        switch (job.JobType)
        {
            case EJobType.eSetTile:
                foreach (KeyValuePair<Vector2Int, (TileBase, TileBase)> pair in job.TileByPos)
                {
                    _tilemap.SetTile( (Vector3Int)pair.Key, pair.Value.Item1 );
                }
                break;
            case EJobType.eSetObject:
                foreach (KeyValuePair<Vector2Int, GameObject> pair in job.TargetObjects)
                {
                    pair.Value.SetActive( false );
                    pair.Value.transform.position = new Vector3( 100, 100 );
                }
                break;
            case EJobType.eErase:
                foreach (KeyValuePair<Vector2Int, (TileBase, TileBase)> pair in job.TileByPos)
                {
                    _tilemap.SetTile( (Vector3Int)pair.Key, pair.Value.Item1 );
                }
                foreach (KeyValuePair<GameObject, Vector2Int> pair in job.EraseObjects)
                {
                    pair.Key.SetActive( true );
                    pair.Key.transform.position = _tilemap.CellToWorld( (Vector3Int)pair.Value ) + new Vector3( _tileHalfSize, _tileHalfSize );
                }
                break;
            case EJobType.eSetValue:

                break;
            default:
                break;
        }
    }

    public void RemoveUselessJob( EditJob job )
    {
        switch (job.JobType)
        {
            case EJobType.eSetObject:
                foreach (KeyValuePair<Vector2Int, GameObject> pair in job.TargetObjects)
                {
                    if (!pair.Value.activeSelf)
                        Destroy( pair.Value );
                }
                break;
            case EJobType.eErase:
                foreach (KeyValuePair<GameObject, Vector2Int> pair in job.EraseObjects)
                {
                    if (!pair.Key.activeSelf)
                        Destroy( pair.Key );
                }
                break;
            default:
                break;
        }
        Destroy( job );
    }

    public void ExcuteRedoJob( EditJob job )
    {
        switch (job.JobType)
        {
            case EJobType.eSetTile:
                foreach (KeyValuePair<Vector2Int, (TileBase, TileBase)> pair in job.TileByPos)
                {
                    _tilemap.SetTile( (Vector3Int)pair.Key, pair.Value.Item2 );
                }
                break;
            case EJobType.eSetObject:
                foreach (KeyValuePair<Vector2Int, GameObject> pair in job.TargetObjects)
                {
                    pair.Value.SetActive( true );
                    pair.Value.transform.position = _tilemap.CellToWorld( (Vector3Int)pair.Key ) + new Vector3( _tileHalfSize, _tileHalfSize );
                }
                break;
            case EJobType.eErase:
                foreach (KeyValuePair<Vector2Int, (TileBase, TileBase)> pair in job.TileByPos)
                {
                    _tilemap.SetTile( (Vector3Int)pair.Key, pair.Value.Item2 );
                }

                foreach (KeyValuePair<GameObject, Vector2Int> pair in job.EraseObjects)
                {
                    pair.Key.SetActive( false );
                    pair.Key.transform.position = new Vector3( 100, 100, 0 );
                }
                break;
            case EJobType.eSetValue:
                break;
            default:
                break;
        }
    }

    public Dictionary<GameObject, Vector2Int> ObjectPos
    {
        get { return _objectPos; }
    }
}
