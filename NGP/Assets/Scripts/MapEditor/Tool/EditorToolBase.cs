using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GridBrushBase;
using static UnityEngine.UI.Image;

public abstract class EditorToolBase : MonoBehaviour
{
    protected float _size;
    protected float _maxSize;
    protected float _tileSize;
    protected float _tileHalfSize;
    protected SpriteRenderer _selectCursor;
    [SerializeField] protected DetailUI _detailUI;
    protected EditorController _editorController;
    protected Tilemap _tilemap;
    [SerializeField] protected GameObject _toolObject;

    public void InitComponent( int initBrusSize )
    {
        _selectCursor = EditorManager.Instance.SelectCursor;
        _tilemap = EditorManager.Instance.EditorTilemap[0];
        _tileSize = _tilemap.cellSize.x;
        _maxSize = _tileSize * 6;
        _tileHalfSize = _tileSize * 0.5f;
        _size = _tileSize * initBrusSize;
        _editorController = GetComponent<EditorController>();
        _detailUI = EditorManager.Instance.DetailUI;
        OnClickButton();

        EditorManager.Instance.OnSceneChanged += Instance_OnSceneChanged;
    }

    public (bool, RaycastHit2D) IsBlockedByObject( int cellPosX, int cellPosY )
    {
        Vector3 origin, dir;
        float dist;
        RaycastHit2D hit;
        origin = _tilemap.CellToWorld( new Vector3Int( cellPosX, cellPosY, -10 ) ) + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
        dir = new Vector3( 0, 0, 1 );
        dist = 200;
        hit = Physics2D.Raycast( origin, dir, dist );
        Debug.DrawRay( origin, dir * 200, UnityEngine.Color.red, 10 );

        if (hit) return (true, hit);
        return (false, hit);
    }

    public void OnClickButton()
    {
        EditorManager.Instance.DeactivateUI();

        if (EditorManager.Instance.DetailUI)
        {
            EditorManager.Instance.DetailUI.gameObject.SetActive( false );
        }
        if (_detailUI)
        {
            EditorManager.Instance.DetailUI = _detailUI;
            _detailUI.gameObject.SetActive( true );
        }
        else
        {
            return;
        }
        EditorManager.Instance.DetailUI.SetUIInfo( _toolObject );
    }

    public void Instance_OnSceneChanged( EditorScene sceneToSet )
    {
        _tilemap = sceneToSet.tilemap;
    }

    public void DrawObject( GameObject objectToDraw, Vector3Int pos, EditJob editJob )
    {
        GameObject obj = Instantiate( objectToDraw, _tilemap.CellToWorld( pos ) + new Vector3( _tileHalfSize, _tileHalfSize ), Quaternion.identity );
        obj.transform.parent = EditorManager.Instance.CurrentEditorScene.tilemap.transform;

        editJob.JobType = EJobType.eObjectPos;
        (editJob as EditJobObjectPos).TargetObjects.Add( (Vector2Int)pos, obj );
    }

    public virtual void SetCursorColor( Vector2 mousePosition, SpriteRenderer selectCursor ) { }
    public virtual void SetSize( float val ) { }
    public virtual void AddSize( float val ) { }
    public virtual void Edit( Vector2 mousePosition, EditJob editJob ) { }
    public void OnClickToolOptionButton() 
    {
        _detailUI.gameObject.SetActive( !_detailUI.gameObject.activeSelf );
    }

    public float Size
    {
        get { return _size; }
        set { _size = value; }
    }

    public float MaxSize
    {
        get { return _maxSize; }
        set { _maxSize = value; }
    }
}
