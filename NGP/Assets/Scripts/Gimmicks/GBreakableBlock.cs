using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GBreakableBlock : GimmickBase
{
    public float breakDelay = 3f; //Time until Break

    private bool playerOnBlock = false;
    private float timeElapsed = 0f; // When Player on Block
    private Vector2 _initPos;

    public override void Activate()
    {
        base.Activate();
    }

    public override void Reset()
    {
        base.Reset();
        transform.position = _initPos;
        InitializeValues();
    }

    private void Awake()
    {
        _initPos = transform.position;
    }

    private void InitializeValues()
    {
        playerOnBlock = false;
        timeElapsed = 0f;
    }

    void Update()
    {
        if (playerOnBlock)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= breakDelay)
            {    
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnBlock = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnBlock = false;
            timeElapsed = 0f;
        }
    }
}
