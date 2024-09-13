using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditJobDragDrop : EditJob
{
    private (GameObject, (Vector3, Vector3)) _movedPos;

    private void Start()
    {
        _movedPos = (null, (new Vector3(),  new Vector3()));
    }

    public override void Undo()
    {
        _movedPos.Item1.transform.position = _movedPos.Item2.Item1;
    }

    public override void Redo()
    {
        _movedPos.Item1.transform.position = _movedPos.Item2.Item2;
    }

    public (GameObject, (Vector3, Vector3)) MovedPos
    {
        get { return _movedPos; }
        set { _movedPos = value; }
    }
}
