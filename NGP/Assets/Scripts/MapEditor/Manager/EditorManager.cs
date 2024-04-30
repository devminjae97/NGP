using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public delegate void TilemapDelegate( EditorScene scene );

public class EditorManager : MonoBehaviour
{
    private static EditorManager instance;

    private EditorScene _currentScene;
    [SerializeField] private EditorScene[] _scene;
    [SerializeField] private Tilemap[] _tilemap;
    [SerializeField] private SpriteRenderer _selectCursor;
    [SerializeField] private List<GameObject> _deactivateUIOnButtonClick;
    [SerializeField] private DetailUI _detailUI;
    private Button _currentEditorTool;
  
    public event TilemapDelegate OnSceneChanged;

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

        _scene[0].tilemap = _tilemap[0];
        _scene[1].tilemap = _tilemap[1];
        _scene[0].cameraPos = new Vector3( 0, 0, -10 );
        _scene[1].cameraPos = new Vector3( 0, 0, -10 );
        _currentScene = _scene[0];
        _scene[1].tilemap.gameObject.SetActive( false );
        _scene[0].topRight = new Vector2( 30, 30 );
        _scene[0].bottomLeft = new Vector2( -30, -30 );
        _scene[1].topRight = new Vector2( 30, 30 );
        _scene[1].bottomLeft = new Vector2( -30, -30 );
    }

    public static EditorManager GetInstance()
    {
        return instance;
    }

    public void SetScene(int i)
    {
        CurrentEditorScene.cameraPos = Camera.main.transform.position;
        CurrentEditorScene = _scene[i];
        OnSceneChanged?.Invoke( CurrentEditorScene );
        Camera.main.transform.position = CurrentEditorScene.cameraPos;
        _scene[0].tilemap.gameObject.SetActive( _scene[0] == _scene[i] );
        _scene[1].tilemap.gameObject.SetActive( _scene[1] == _scene[i] );
    }

    public void DeactivateUI()
    {
        foreach (GameObject ui in _deactivateUIOnButtonClick)
        {
            if (ui == null) continue;
            ui.SetActive( false );
        }
    }

    public Tilemap[] EditorTilemap
    {
        get { return _tilemap; }
        set { _tilemap = value; }
    }

    public EditorScene[] EditorScene
    {
        get { return _scene; }
        set { _scene = value; }
    }

    public EditorScene CurrentEditorScene
    {
        get { return _currentScene; }
        set { _currentScene = value; }
    }

    public SpriteRenderer SelectCursor
    {
        get { return _selectCursor; }
        set { _selectCursor = value; }
    }

    public DetailUI DetailUI
    {
        get { return _detailUI; }
    }

    public Button CurrentEditorTool
    {
        get { return _currentEditorTool; }
        set { _currentEditorTool = value; }
    }
}
