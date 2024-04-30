using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    [SerializeField] private GameObject _groundButtonGroup;
    [SerializeField] private GameObject _eraserButtonGroup;
    [SerializeField] private GameObject _crackedBlockButtonGroup;
    [SerializeField] private GameObject _flagButtonGroup;
    private GameObject _draggingObject;

    private Dictionary<ETileType, GameObject> _buttonGroupByTileType;
    [SerializeField] private Camera _mainCamera;

    private EditorToolBase _tool;
    private EditJob _editJob;

    private Dictionary<Vector3Int, SpriteRenderer> _selectedCursors;

    private void Awake()
    {
        _buttonGroupByTileType = new Dictionary<ETileType, GameObject>()
        {
            { ETileType.eNone, null },
            { ETileType.eGround, _groundButtonGroup },
            { ETileType.eEraser, _eraserButtonGroup },
            { ETileType.eCrackedBlock, _crackedBlockButtonGroup },
            { ETileType.eFlag, _flagButtonGroup },
        };

        foreach (GameObject group in _buttonGroupByTileType.Values)
        {
            if (group == null) continue;
            group.SetActive( false );
        }
    }

    private void Start()
    {
        _selectCursor = EditorManager.GetInstance().SelectCursor;
        _tilemap = EditorManager.GetInstance().EditorTilemap[0];
        _currentTileType = ETileType.eNone;
        _brushSize = _tilemap.cellSize.x;
        _tileSize = _tilemap.cellSize.x;
        _tileHalfSize = _tileSize * 0.5f;
        _currentTileType = ETileType.eNone;
        _selectedCursors = new Dictionary<Vector3Int, SpriteRenderer>();
    }

    void Update()
    {
        if (IsPointerOverUI()) return;

        if (_currentTileType == ETileType.eNone)
        {
            DragDrop();
        }
        else
        {
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

        DragView();
        Edit();
        UndoRedo();
    }

    private void DragDrop()
    {
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
            Debug.DrawRay( origin, dir * 10, UnityEngine.Color.yellow, 100 );
            if (hit)
            {
                isDragging = true;
                _draggingObject = hit.transform.gameObject;
            }
        }
        else if (Input.GetMouseButton( 0 ))
        {
            if (isDragging && _draggingObject)
            {
                _draggingObject.transform.position = _selectCursor.transform.position;
            }
        }
        else if (Input.GetMouseButtonUp( 0 ))
        {
            isDragging = false;
            _draggingObject = null;
        }
    }

    private void Edit()
    {
        if (_currentTileType == ETileType.eNone)
        {
            if (Input.GetMouseButtonDown( 0 ))
            {
               /* if (!Input.GetKey( KeyCode.LeftControl ))
                {
                    ClearSelectedCursors();
                }*/
                _editJob = gameObject.AddComponent<EditJob>();
                _tool.Edit( GetCursorCellPosition( Camera.main.ScreenToWorldPoint( Input.mousePosition ) ), _editJob );
            }
            else if (Input.GetMouseButtonUp( 0 ))
            {
                /*if (!_editJob.IsEmptyJob())
                {
                    EditJobManager.GetInstance().PushJob( _editJob );
                }
                Destroy( _editJob );*/
            }
        }
        else
        {
            if (Input.GetMouseButtonDown( 0 ))
            {
                _editJob = gameObject.AddComponent<EditJob>();
            }
            else if (Input.GetMouseButton( 0 ))
            {
                 _tool.Edit( _selectCursor.transform.position, _editJob );
            }
            else if (Input.GetMouseButtonUp( 0 ))
            {
                if (!_editJob.IsEmptyJob())
                {
                    EditJobManager.GetInstance().PushJob( _editJob );
                }
                Destroy( _editJob );
            }
        }
    }

    private void ClearSelectedCursors()
    {
        foreach (KeyValuePair<Vector3Int,SpriteRenderer> pair in _selectedCursors)
        {
            Destroy( pair.Value );
        }
        _selectedCursors.Clear();
    }

    private void UndoRedo()
    {
        if (Input.GetKeyDown( KeyCode.Z ) && Input.GetKey( KeyCode.LeftControl ))
        {
            EditJobManager.GetInstance().Undo();
        }

        if (Input.GetKeyDown( KeyCode.Y ) && Input.GetKey( KeyCode.LeftControl ))
        {
            EditJobManager.GetInstance().Redo();
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

            Vector2 topRight = EditorManager.GetInstance().CurrentEditorScene.topRight;
            Vector2 bottomLeft = EditorManager.GetInstance().CurrentEditorScene.bottomLeft;
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
    public void InitButtonGroup(ETileType tiletype, EditorToolBase toolToSet)
    {
        ETileType tiletypeTemp = _currentTileType;
        _currentTileType = tiletype;

        if (_buttonGroupByTileType[tiletypeTemp] != null)
        {
            _buttonGroupByTileType[tiletypeTemp].SetActive( false );
        }
        if (_buttonGroupByTileType[_currentTileType] != null)
        {
            _buttonGroupByTileType[_currentTileType].SetActive( true );
        }

        Color color = _selectCursor.color;
        color.a = _currentTileType == ETileType.eNone ? 0 : 1;
        _selectCursor.color = color;
        //_selectCursor.gameObject.SetActive( _currentTileType != ETileType.eNone );

        _tool = toolToSet;

        ClearSelectedCursors();
    }

    public ETileType TileType
    { 
        get { return _currentTileType; }
    }

    public Dictionary<Vector3Int, SpriteRenderer> SelectedCursors
    {
        get { return _selectedCursors; }
    }
}
