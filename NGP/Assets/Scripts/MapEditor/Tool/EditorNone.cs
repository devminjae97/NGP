using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorNone : EditorToolBase
{
    private void Start()
    {
        InitComponent( 1 );
    }

    public void OnClickNoneButton()
    {
        base.OnClickButton();
        _editorController.InitButtonGroup( ETileType.eNone, this );
    }

    public override void Edit( Vector2 mousePosition, EditJob editJob )
    {
        Vector3Int pos = _tilemap.WorldToCell( mousePosition );
        (bool, RaycastHit2D) result = IsBlockedByObject( pos.x, pos.y );
        if (!result.Item1) return;
        if (_editorController.SelectedCursors.ContainsKey( pos ))
        {
            Destroy( _editorController.SelectedCursors[pos].gameObject );
            _editorController.SelectedCursors.Remove( pos );
        }
        else
        {
            SpriteRenderer selectedCursor = Instantiate( _selectCursor.gameObject, mousePosition, Quaternion.identity ).GetComponent<SpriteRenderer>();
            Color color = selectedCursor.color;
            color.a = 1;
            selectedCursor.color = color;
            _editorController.SelectedCursors.Add( pos, selectedCursor );
        }
        SelectObject( result.Item2.transform.gameObject );
        // 양성인 TODO: DetailUI 띄우기
        // DetailUI.SetInfo(GimmickBase.Info);
    }

    void SelectObject( GameObject obj )
    {
        switch (obj.tag)
        {
            case "Respawn":
                break;
            case "Finish":
                break;
            default: 
                break;
        }
    }
}
