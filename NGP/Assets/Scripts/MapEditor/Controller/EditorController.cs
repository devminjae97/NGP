using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngineInternal;

public enum ETileType
{
    eNone,
    eGround,
    eEraser,
    eCrackedBlock,
    eMovingBlock,
    eLaser,
    eFlag,
}

public class EditorController : MonoBehaviour
{
    private bool isDragging;
    private float _brushSize;
    private float _tileSize;
    private float _tileHalfSize;
    private float _dragScreenSpeed = 4000;
    private Vector3 dragScreenLatePos;

    private SpriteRenderer _selectCursor;
    private Tilemap _tilemap;
    private ETileType _currentTileType;

    [SerializeField] private TileBase _groundTileBase;

    [SerializeField] private DetailUINormalBlock _groundButtonGroup;
    [SerializeField] private DetailUIEraser _eraserButtonGroup;
    [SerializeField] private DetailUICrackedBlock _crackedBlockButtonGroup;
    //[SerializeField] private DetailUIFlag _flagButtonGroup;
    private GameObject _draggingObject;
    private Vector3 _draggingObjectInitPos;

    private Dictionary<ETileType, DetailUI> _buttonGroupByTileType;
    [SerializeField] private Camera _mainCamera;

    private EditorToolBase _tool;
    private EditJob _editJob;

    private Dictionary<Vector3Int, SpriteRenderer> _selectedCursors;

    private void Awake()
    {
        _buttonGroupByTileType = new Dictionary<ETileType, DetailUI>()
        {
            { ETileType.eNone, null },
            { ETileType.eGround, _groundButtonGroup },
            { ETileType.eEraser, _eraserButtonGroup },
            { ETileType.eCrackedBlock, _crackedBlockButtonGroup },
            //{ ETileType.eFlag, _flagButtonGroup },
        };

        foreach (DetailUI group in _buttonGroupByTileType.Values)
        {
            if (group == null) continue;
            group.gameObject.SetActive( false );
        }
    }

    private void Start()
    {
        _selectCursor = EditorManager.Instance.SelectCursor;
        _tilemap = EditorManager.Instance.EditorTilemap[0];
        _currentTileType = ETileType.eNone;
        _brushSize = _tilemap.cellSize.x;
        _tileSize = _tilemap.cellSize.x;
        _tileHalfSize = _tileSize * 0.5f;
        _selectedCursors = new Dictionary<Vector3Int, SpriteRenderer>();
    }

    void Update()
    {
        if (IsPointerOverUI()) return;

        Edit();
        DragDrop();
        MoveCursor();
        DragView();
        UndoRedo();
    }

    private void MoveCursor()
    {
        if (_currentTileType == ETileType.eNone) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );

        if ((_tool.Size / _tileSize) % 2 == 1)
        {
            _selectCursor.transform.position = GetCursorCellPosition( mousePosition );
        }
        else
        {
            _selectCursor.transform.position = GetCursorCellPosition( mousePosition ) - new Vector3( _tileHalfSize, _tileHalfSize, 0 );
            _tool.SetCursorColor( mousePosition, _selectCursor );
        }
    }

    private void DragDrop()
    {
        if (_currentTileType != ETileType.eNone) return;
        if (Input.GetMouseButtonDown( 0 ))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            Vector3 origin, dir;
            float dist;
            RaycastHit2D hit;
            origin = _tilemap.CellToWorld( _tilemap.WorldToCell(mousePosition) ) + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
            dir = new Vector3( 0, 0, 1 );
            dist = 200;
            hit = Physics2D.Raycast( origin, dir, dist );
            if (hit)
            {
                isDragging = true;
                _draggingObject = hit.transform.gameObject;

                _draggingObjectInitPos = _draggingObject.transform.position;

                hit.transform.gameObject.layer = 2;

                _editJob = gameObject.AddComponent<EditJobDragDrop>();
            }
        }
        else if (Input.GetMouseButton( 0 ))
        {
            if (isDragging && _draggingObject)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
                _draggingObject.transform.position = new Vector3( mousePosition.x, mousePosition.y, 0 );
            }
        }
        else if (Input.GetMouseButtonUp( 0 ))
        {
            if (_draggingObject == null) return;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            Vector3 posToSet = _tilemap.CellToWorld( _tilemap.WorldToCell( mousePosition ) ) + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
            if (!CanDropObject( posToSet.x, posToSet.y ))
            {
                _draggingObject.transform.position = _draggingObjectInitPos;
                _draggingObject.layer = 0;
                return;
            }
            
            _draggingObject.transform.position = posToSet;
            _draggingObject.layer = 0;
            isDragging = false;

            if (_editJob)
            {
                if (_draggingObjectInitPos != posToSet)
                {
                    (_editJob as EditJobDragDrop).MovedPos = (_draggingObject, (_draggingObjectInitPos, posToSet));
                    EditJobManager.Instance.PushJob( _editJob );
                }
                Destroy( _editJob );
            }
           
            _draggingObject = null;
        }
    }

    private void Edit()
    {
        if (_tool == null) return;
        if (_currentTileType == ETileType.eNone)
        {
            if (Input.GetMouseButtonDown( 0 ))
            {
                if (!Input.GetKey( KeyCode.LeftControl ))
                {
                    ClearSelectedCursors();
                }
                _editJob = gameObject.AddComponent<EditJobSelect>();
                _tool.Edit( GetCursorCellPosition( Camera.main.ScreenToWorldPoint( Input.mousePosition ) ), _editJob );
                Destroy( _editJob );
            }
        }
        else
        {
            if (Input.GetMouseButtonDown( 0 ))
            {
                SetEditJobType();
            }
            else if (Input.GetMouseButton( 0 ))
            {
                if (_editJob == null) return;
                _tool.Edit( _selectCursor.transform.position, _editJob );
            }
            else if (Input.GetMouseButtonUp( 0 ))
            {
                if (_editJob == null) return;
                EditJobManager.Instance.PushJob( _editJob );
                Destroy( _editJob );
            }
        }
    }

    private void SetEditJobType()
    {
        switch(_currentTileType)
        {
            case ETileType.eNone:
                break;
            case ETileType.eGround:
                _editJob = gameObject.AddComponent<EditJobDrawingTile>();
                break;
            case ETileType.eCrackedBlock:
                _editJob = gameObject.AddComponent<EditJobObjectPos>();
                break;
            case ETileType.eEraser:
                _editJob = gameObject.AddComponent<EditJobEraser>();
                break;
            default:
                break;
        }
    }

    private void ClearSelectedCursors()
    {
        foreach (KeyValuePair<Vector3Int,SpriteRenderer> pair in _selectedCursors)
        {
            Destroy( pair.Value.gameObject );
        }
        _selectedCursors.Clear();
    }

    private void UndoRedo()
    {
        if (Input.GetKeyDown( KeyCode.Z ) && Input.GetKey( KeyCode.LeftControl ))
        {
            EditJobManager.Instance.Undo();
        }

        if (Input.GetKeyDown( KeyCode.Y ) && Input.GetKey( KeyCode.LeftControl ))
        {
            EditJobManager.Instance.Redo();
        }
    }

    private void DragView()
    {
        
        if (Input.GetMouseButtonDown( 1 ))
        {
            dragScreenLatePos = Input.mousePosition;
        }
        if (Input.GetMouseButton( 1 ))
        {        
            Vector3 deltaPosition = _mainCamera.ScreenToViewportPoint( dragScreenLatePos - Input.mousePosition );
            Vector3 move = deltaPosition * (Time.deltaTime * _dragScreenSpeed);
            _mainCamera.transform.Translate( move );

            Vector2 topRight = EditorManager.Instance.CurrentEditorScene.topRight;
            Vector2 bottomLeft = EditorManager.Instance.CurrentEditorScene.bottomLeft;
            float posx = Mathf.Clamp( _mainCamera.transform.position.x, bottomLeft.x, topRight.x );
            float posy = Mathf.Clamp( _mainCamera.transform.position.y, bottomLeft.y, topRight.y );
            _mainCamera.transform.position = new Vector3( posx, posy, _mainCamera.transform.position.z );

            dragScreenLatePos = Input.mousePosition;
        }
    }

    private bool IsPointerOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData( EventSystem.current );
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll( pointerEventData, results );

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == LayerMask.NameToLayer( "UI" ))
                return true;
        }

        return false;
    }

    Vector3 GetCursorCellPosition( Vector2 mousePosition )
    {
        Vector3 vec = new Vector3( _tilemap.CellToWorld( _tilemap.WorldToCell( mousePosition ) ).x, _tilemap.CellToWorld( _tilemap.WorldToCell( mousePosition ) ).y, 0);
        return vec + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
    }

    /*
     * Set Color, TileType, ButtonGroup
     */
    public void InitButtonGroup( ETileType tiletype, EditorToolBase toolToSet )
    {
        SetDetailUIActive( tiletype );

        Color color = _selectCursor.color;
        color.a = _currentTileType == ETileType.eNone ? 0 : 1;
        _selectCursor.color = color;

        _tool = toolToSet;

        ClearSelectedCursors();
    }

    public void SetDetailUIActive( ETileType tiletype, bool isButtonClicked = true )
    {
        ETileType tiletypeTemp = _currentTileType;

        if (isButtonClicked)
            _currentTileType = tiletype;

        if (_buttonGroupByTileType[tiletypeTemp] != null)
        {
            _buttonGroupByTileType[tiletypeTemp].gameObject.SetActive( false );
        }
        if (_buttonGroupByTileType[_currentTileType] != null)
        {
            _buttonGroupByTileType[_currentTileType].gameObject.SetActive( true );
        }

    }

    // x, y에 타일 중앙 값 넣어야함
    private bool CanDropObject( float x, float y )
    {
        Vector3 origin, dir;
        float dist;
        RaycastHit2D hit;
        origin = new Vector3( x, y, -10 );
        dir = new Vector3( 0, 0, 1 );
        dist = 200;
        hit = Physics2D.Raycast( origin, dir, dist );

        if (hit)
        {
            return false;
        }
        
        if (_tilemap.GetTile( _tilemap.WorldToCell( new Vector3( x, y ) ) ) != null)
        {
            return false;
        }

        return true;
    }

    public ETileType TileType
    { 
        get { return _currentTileType; }
    }

    public Dictionary<Vector3Int, SpriteRenderer> SelectedCursors
    {
        get { return _selectedCursors; }
    }

    public DetailUICrackedBlock CrackedBlockButtonGroup
    {
        get { return _crackedBlockButtonGroup; }
    }

    public Dictionary<ETileType, DetailUI> ButtonGroupByTileType
    {
        get { return _buttonGroupByTileType; }
    }
}
