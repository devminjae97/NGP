using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IReverse : ItemBase
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
            if(collision.transform.TryGetComponent<CharacterMovement>(out var character))
            {
                character.Reverse();
                gameObject.SetActive(false);
            }
        }
    }
}
