using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMovingBlock : GimmickBase
{
    [SerializeField] protected float _speed = 5.0f;
    [SerializeField] protected float _minX = 0.0f;
    [SerializeField] protected float _maxX = 0.0f;
    [SerializeField] protected float _minY = 0.0f;
    [SerializeField] protected float _maxY = 0.0f;
    [SerializeField] protected float _movingDistanceX = 10.0f;
    [SerializeField] protected float _movingDistanceY = 10.0f;
    [SerializeField] protected bool _IsHorizontal = false;

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
        if(_IsHorizontal)
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
        if (_IsHorizontal)
        {
            _minX = _initPos.x;
            _maxX = _minX + _movingDistanceX;
        }
        else
        {
            _minY = _initPos.y;
            _maxY = _minY + _movingDistanceY;
        }
    }

    private void MovingX()
    {
        if (_movingRight)
        {
            transform.Translate(Vector2.right * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * _speed * Time.deltaTime);
        }

        if (transform.position.x >= _maxX)
        {
            _movingRight = false;
        }
        else if (transform.position.x <= _minX)
        {
            _movingRight = true;
        }
    }

    private void MovingY()
    {
        if (_movingUp)
        {
            transform.Translate(Vector2.up * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.down * _speed * Time.deltaTime);
        }

        if (transform.position.x >= _maxY)
        {
            _movingUp = false;
        }
        else if (transform.position.x <= _minY)
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
