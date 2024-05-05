using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GReverse : GimmickBase
{
    private Vector2 _initPos;

    public override void Activate()
    {
        base.Activate();
    }

    public override void Reset()
    {
        base.Reset();
        transform.position = _initPos;
    }

    private void Awake()
    {
        _initPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.GetComponent<CharacterMovement>().Reverse();
            gameObject.SetActive(false);
        }
    }


}
