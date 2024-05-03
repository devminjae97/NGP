using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMovingBlock : GimmickBase
{

    [SerializeField] protected float speed = 5.0f;
    [SerializeField] protected float minX = 0.0f;
    [SerializeField] protected float maxX = 0.0f;
    [SerializeField] protected float minY = 0.0f;
    [SerializeField] protected float maxY = 0.0f;
    [SerializeField] protected float movingDistanceX = 10.0f;
    [SerializeField] protected float movingDistanceY = 10.0f;
    [SerializeField] protected bool IsHorizontal = false;

    private bool _movingRight = true;
    private bool _movingUp = true;
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


    void Awake()
    {
        _initPos = transform.position;

        InitializeValue();
    }


    void Update()
    {
        if(IsHorizontal)
        {
            MovingX();
        }
        else
        {
            MovingY();
        }
    }

    private void InitializeValue()
    {
        if (IsHorizontal)
        {
            minX = _initPos.x;
            maxX = minX + movingDistanceX;
        }
        else
        {
            minY = _initPos.y;
            maxY = minY + movingDistanceY;
        }
    }
    private void MovingX()
    {
        if (_movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (transform.position.x >= maxX)
        {
            _movingRight = false;
        }
        else if (transform.position.x <= minX)
        {
            _movingRight = true;
        }
    }

    private void MovingY()
    {
        if (_movingUp)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }

        if (transform.position.x >= maxY)
        {
            _movingUp = false;
        }
        else if (transform.position.x <= minY)
        {
            _movingUp = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y == -1f)
            {
                collision.transform.SetParent(transform, true);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null, true);
        }
    }
}
