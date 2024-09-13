using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditJobManager : Singleton<EditJobManager>
{
    private Deque<EditJob> _undos;
    private Deque<EditJob> _redos;
    private Tilemap _tilemap;
    private float _tileSize;
    private float _tileHalfSize;
    private Dictionary<GameObject, Vector2Int> _objectPos;

    private void Awake()
    {
        _undos = new Deque<EditJob>();
        _redos = new Deque<EditJob>();
    }

    private void Start()
    {
        _tilemap = EditorManager.Instance.CurrentEditorScene.tilemap;
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
        job.Undo();
        return job;
    }

    public EditJob Redo()
    {
        if (_redos.IsEmpty()) return null;
        EditJob job = _redos.GetBack();
        _redos.PopBack();
        _undos.PushBack( job );
        job.Redo();
        return job;
    }

    public void PushJob( EditJob job )
    {
        if (_undos.IsFull())
        {
            EditJob jobToPop = _undos.GetFront();
            // 양성인 TODO: Save 하기 전에 SetActive(false)로 되어 있는 job은 모두 삭제해야 한다.
            jobToPop.RemoveJob();
            _undos.PopFront();
        }
        _undos.PushBack( job );
        _redos.Clear();
    }

    public Dictionary<GameObject, Vector2Int> ObjectPos
    {
        get { return _objectPos; }
    }
}
