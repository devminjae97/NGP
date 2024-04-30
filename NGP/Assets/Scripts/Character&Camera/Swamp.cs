using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swamp : GimmickBase
{
    [SerializeField] protected float _increasedFriction = 100f;
    private float _normalFriction = 1f;

    public override void Activate()
    {
        base.Activate();
    }

    protected override void Initialize()
    {
        base.Initialize();
        Activate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRB = collision.GetComponent<Rigidbody2D>();

            playerRB.drag += _increasedFriction;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRB = collision.GetComponent<Rigidbody2D>();

            playerRB.drag = _normalFriction;
        }
    }
}
