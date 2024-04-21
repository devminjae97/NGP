using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum EJobType
{ 
    eSetTile,
    eSetObject,
    eErase,
    eSetValue,
    eClickButton,
}

/*
 * 브러쉬의 크기가 달라지면 값들을 배열에 넣는게 성능상 낫다고 판단되어 배열 사용
 */
public class EditJob : MonoBehaviour
{
    protected EJobType _jobType;
    private Dictionary<Vector2Int, GameObject> _targetObjects;
    private Dictionary<Vector2Int, (TileBase, TileBase)> _tileByPos;
    private Dictionary<Vector2Int, GameObject> _eraseObjects;

    public EditJob()
    {
        _targetObjects = new Dictionary<Vector2Int, GameObject>();
        _tileByPos = new Dictionary<Vector2Int, (TileBase, TileBase)>();
        _eraseObjects = new Dictionary<Vector2Int, GameObject>();
    }

    public bool IsEmptyJob()
    {
        Debug.Log( _targetObjects.Count + ", " + _tileByPos.Count + ", " + _eraseObjects.Count );
        return _targetObjects.Count == 0 && _tileByPos.Count == 0 && _eraseObjects.Count == 0;
    }

    public EJobType JobType
    { 
        get { return _jobType; }
        set { _jobType = value; }
    }

    public Dictionary<Vector2Int, GameObject> TargetObjects
    {
        get { return _targetObjects; }
        set { _targetObjects = value; }
    }

    public Dictionary<Vector2Int, (TileBase, TileBase)> TileByPos
    {
        get { return _tileByPos; }
        set { _tileByPos = value; }
    }

    public Dictionary<Vector2Int, GameObject> EraseObjects
    {
        get { return _eraseObjects; }
        set { _eraseObjects = value; }
    }
}

/*
 * 브러쉬의 특성을 수정하기 위한 클래스로, 무조건 한 번에 한 값만 수정하기 때문에 []사용 x
 */
public class EditValueJob<T> : EditJob
{
    private T oldValue;
    private T curValue;
    public void SetValueJob( T value )
    {
        _jobType = EJobType.eSetValue;
        oldValue = curValue;
        curValue = value;
    }
}
