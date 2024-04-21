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
                //RemoveOnIterate( job.TargetObjects );
                foreach (KeyValuePair<Vector2Int, GameObject> pair in job.TargetObjects)
                {
                    pair.Value.SetActive( false );
                    pair.Value.transform.position = new Vector3( 100, 100 );
                }
                break;
            case EJobType.eErase:
                Debug.Log( "AA1" );
                foreach (KeyValuePair<Vector2Int, (TileBase, TileBase)> pair in job.TileByPos)
                {
                    _tilemap.SetTile( (Vector3Int)pair.Key, pair.Value.Item1 );
                }
                foreach (KeyValuePair<GameObject, Vector2Int> pair in job.EraseObjectsTest)
                {
                    Debug.Log( "AA2" );
                    pair.Key.SetActive( true );
                    pair.Key.transform.position = _tilemap.CellToWorld( (Vector3Int)pair.Value ) + new Vector3( _tileSize / 2, _tileSize / 2 );
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
                foreach (KeyValuePair<GameObject, Vector2Int> pair in job.EraseObjectsTest)
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
                    pair.Value.transform.position = _tilemap.CellToWorld( (Vector3Int)pair.Key ) + new Vector3( _tileSize / 2, _tileSize / 2 );
                }
                break;
            case EJobType.eErase:
                foreach (KeyValuePair<Vector2Int, (TileBase, TileBase)> pair in job.TileByPos)
                {
                    _tilemap.SetTile( (Vector3Int)pair.Key, pair.Value.Item2 );
                }

                //RemoveOnIterate( job.EraseObjects );
                foreach (KeyValuePair<GameObject, Vector2Int> pair in job.EraseObjectsTest)
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

    private void RemoveOnIterate( Dictionary<Vector2Int, GameObject> dic )
    {
        List<Vector2Int> toRemoveKey = new List<Vector2Int>();
        List<GameObject> toRemoveValue = new List<GameObject>();
        foreach (KeyValuePair<Vector2Int, GameObject> pair in dic)
        {
            if (pair.Value == null) continue;
            toRemoveKey.Add( pair.Key );
            toRemoveValue.Add( pair.Value );
        }
        for (int i = 0; i < toRemoveKey.Count; i++)
        {
            GameObject tmp = Instantiate( toRemoveValue[i], new Vector3( 100, 100, 0 ), Quaternion.identity );
            tmp.transform.parent = _tilemap.transform;
            Destroy( dic[toRemoveKey[i]] );
            dic.Remove( toRemoveKey[i] );
            dic.Add( toRemoveKey[i], tmp );
        }
    }

    public Dictionary<GameObject, Vector2Int> ObjectPos
    {
        get { return _objectPos; }
    }
}
