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
 * �귯���� ũ�Ⱑ �޶����� ������ �迭�� �ִ°� ���ɻ� ���ٰ� �ǴܵǾ� �迭 ���
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
 * �귯���� Ư���� �����ϱ� ���� Ŭ������, ������ �� ���� �� ���� �����ϱ� ������ []��� x
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
