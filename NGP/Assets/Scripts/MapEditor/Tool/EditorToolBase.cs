using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public abstract class EditorToolBase : MonoBehaviour
{
    protected float _size;
    protected float _tileSize;
    protected SpriteRenderer _selectCursor;
    [SerializeField] protected GameObject _detailUIObject;
    protected DetailUI _detailUI;
    [SerializeField] protected EditorController _editorController;
    protected Tilemap _tilemap;

    public void InitComponent( int initBrusSize )
    {
        _selectCursor = EditorManager.GetInstance().SelectCursor;
        _detailUI = _detailUIObject.GetComponent<DetailUI>();
        _tilemap = EditorManager.GetInstance().EditorTilemap[0];
        _tileSize = _tilemap.cellSize.x;
        _size = _tileSize * initBrusSize;
        OnClickButton();

        EditorManager.GetInstance().OnSceneChanged += Instance_OnSceneChanged;
    }

    public void SetDetailUIVisibility( bool isVisible ) 
    {
        _detailUIObject.SetActive( isVisible );
    }

    public (bool, RaycastHit2D) IsBlockedByObject( int x, int y )
    {
        Vector3 origin, dir;
        float dist;
        RaycastHit2D hit;
        origin = _tilemap.CellToWorld( new Vector3Int( x, y, -10 ) ) + new Vector3( _tileSize / 2, _tileSize / 2, 0 );
        dir = new Vector3( 0, 0, 1 );
        dist = 200;
        hit = Physics2D.Raycast( origin, dir, dist );

        if (hit) return (true, hit);
        return (false, hit);
    }

    public void OnClickButton()
    {
        EditorManager.GetInstance().DeactivateUI();
    }

    public void Instance_OnSceneChanged( EditorScene sceneToSet )
    {
        _tilemap = sceneToSet.tilemap;
    }

    public void DrawObject( GameObject objectToDraw, Vector3Int pos )
    {
        GameObject obj = Instantiate( objectToDraw, _tilemap.CellToWorld( pos ) + new Vector3( _tileSize / 2, _tileSize / 2 ), Quaternion.identity );
        obj.transform.parent = EditorManager.GetInstance().CurrentEditorScene.tilemap.transform;
    }

    public virtual void SetCursorColor( Vector2 mousePosition, SpriteRenderer selectCursor ) { }
    public virtual void SetSize( float val ) { }
    public virtual void Edit( Vector2 mousePosition ) { }

    public float Size
    {
        get { return _size; }
        set { _size = value; }
    }
}
