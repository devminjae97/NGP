using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

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
    private float _brushSize;
    private float _tileSize;
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

    private Dictionary<ETileType, GameObject> _buttonGroupByTileType;
    [SerializeField] private Camera _mainCamera;

    private EditorToolBase _tool;

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
    }

    void Update()
    {
        if (_currentTileType == ETileType.eNone || IsPointerOverUI()) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );

        // Set Cursor Position
        if ((_tool.Size / _tileSize) % 2 == 1)
        {
            _selectCursor.transform.position = GetCursorCellPosition( mousePosition );
        }
        else
        {
            _selectCursor.transform.position = GetCursorCellPosition( mousePosition ) - new Vector3( _tileSize / 2, _tileSize / 2 );
        }
        // Set Cursor Color
        _tool.SetCursorColor( mousePosition, _selectCursor );
        // Dragging Camera
        DragView();

        if (Input.GetMouseButton( 0 ))
        {
            _tool.Edit( _selectCursor.transform.position );
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
        return _tilemap.CellToWorld( _tilemap.WorldToCell( mousePosition ) ) + new Vector3( _tileSize * 0.5f, _tileSize * 0.5f, 0 ); ;
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

        _tool = toolToSet;
    }

    public void OnClickNoneButton()
    {
        InitButtonGroup( ETileType.eNone, null );
    }
}
