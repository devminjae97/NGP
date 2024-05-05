using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCrackedBlock : GimmickBase
{
    [SerializeField] protected float breakTime = 0.5f; //Time until Break
    [SerializeField] protected float respawnTime = 2f;

    private Renderer _blockRenderer;
    private Collider2D[] _colliders;
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

    protected override void Initialize()
    {
        base.Initialize();

        _initPos = transform.position;
        if (TryGetComponent<Renderer>(out Renderer blockRenderer))
        {
            _blockRenderer = blockRenderer;
        }
        if (TryGetComponent<Collider2D[]>(out Collider2D[] collider))
        {
            _colliders = collider;
        }
    }
   
    IEnumerator RespawnBlock()
    {
        yield return new WaitForSeconds(breakTime);
        SetBlockActive(false);

        
        yield return new WaitForSeconds(respawnTime);
        SetBlockActive(true);
    }

    void SetBlockActive(bool active)
    {
        _blockRenderer.enabled = active;
        foreach(Collider2D collider in _colliders)
        {
            collider.enabled = active;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y == -1f)
            {
                StartCoroutine(RespawnBlock());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("You are dead");
        }
    }
}
