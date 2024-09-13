using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GCrackedBlock : GimmickBase
{
    [SerializeField] protected int _breakTime = 500; //Time until Break
    [SerializeField] protected int _respawnTime = 2000;

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
    }
    private void Awake()
    {
        _initPos = transform.position;
    }
    private async UniTask SetBlockActiveFalse()
    {
        await UniTask.Delay(_breakTime);
        gameObject.SetActive(false);
    }
    private async UniTask SetBlockActiveTrue()
    {
        await UniTask.Delay(_respawnTime);
        gameObject.SetActive(true);
    }

    private async void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y == -1f)
            {
                await UniTask.WhenAll(SetBlockActiveFalse(),SetBlockActiveTrue());
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

    public float BreakTime
    {
        get { return breakTime; }
        set { breakTime = value; }
    }

    public float RespawnTime
    {
        get { return respawnTime; }
        set { respawnTime = value; }
    }
}
