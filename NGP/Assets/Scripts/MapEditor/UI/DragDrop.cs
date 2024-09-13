using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static Vector2 DefaultPos;
    private Tilemap _tilemap;
    private float _tileHalfSize;

    private void Start()
    {
        EditorManager.Instance.OnSceneChanged += Instance_OnSceneChanged;
        _tilemap = EditorManager.Instance.CurrentEditorScene.tilemap;
        _tileHalfSize = _tilemap.cellSize.x * 0.5f;
    }

    public void Instance_OnSceneChanged( EditorScene sceneToSet )
    {
        _tilemap = sceneToSet.tilemap;
    }

    void IBeginDragHandler.OnBeginDrag( PointerEventData eventData )
    {
        DefaultPos = this.transform.position;
    }

    void IDragHandler.OnDrag( PointerEventData eventData )
    {
        Vector2 currentPos = eventData.position;
        this.transform.position = currentPos;


    }

    void IEndDragHandler.OnEndDrag( PointerEventData eventData )
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        gameObject.transform.position = _tilemap.WorldToCell( mousePos ) + new Vector3( _tileHalfSize, _tileHalfSize, 0 );
    }
}
