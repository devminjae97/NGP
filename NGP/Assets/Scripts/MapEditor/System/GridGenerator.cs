using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridGenerator : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Vector2 _topRight;
    private Vector2 _bottomLeft;

    public float gridSize;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _topRight = EditorManager.Instance.EditorScene[0].topRight;
        _bottomLeft = EditorManager.Instance.EditorScene[0].bottomLeft;

        initLineRenderer();

        makeGrid( _bottomLeft.x, _bottomLeft.y, (int)_topRight.x * 2, (int)_topRight.y * 2 );
        _lineRenderer.sortingOrder = -3;
    }

    void initLineRenderer()
    {
        _lineRenderer.startWidth = _lineRenderer.endWidth = 0.1f;
    }

    void makeGrid( float sr, float sc, int rowCount, int colCount )
    {
        List<Vector3> gridPos = new List<Vector3>();

        float ec = sc + colCount * gridSize;

        gridPos.Add( new Vector3( sr, sc ) );
        gridPos.Add( new Vector3( sr, ec ) );

        int toggle = -1;
        Vector3 currentPos = new Vector3( sr, ec );
        for (int i = 0; i < rowCount; i++)
        {
            Vector3 nextPos = currentPos;

            nextPos.x += gridSize;
            gridPos.Add( nextPos );

            nextPos.y += (colCount * toggle * gridSize);
            gridPos.Add( nextPos );

            currentPos = nextPos;
            toggle *= -1;
        }

        currentPos.x = sr;
        gridPos.Add( currentPos );

        int colToggle = toggle = 1;
        if (currentPos.y == ec) colToggle = -1;

        for (int i = 0; i < colCount; i++)
        {
            Vector3 nextPos = currentPos;

            nextPos.y += (colToggle * gridSize);
            gridPos.Add( nextPos );

            nextPos.x += (rowCount * toggle * gridSize);
            gridPos.Add( nextPos );

            currentPos = nextPos;
            toggle *= -1;
        }

        _lineRenderer.positionCount = gridPos.Count;
        _lineRenderer.SetPositions( gridPos.ToArray() );
    }

    public void SetGridVisibility()
    {
        _lineRenderer.enabled = !_lineRenderer.enabled;
    }
}
