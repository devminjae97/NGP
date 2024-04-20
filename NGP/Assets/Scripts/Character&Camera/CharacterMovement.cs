using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed = 20f;
    [SerializeField] protected float _jumpForce = 10f;
    [SerializeField] protected float _additionalForce = 200f;
    [SerializeField] protected Vector2 _boxSize;
    [SerializeField] protected float _castDistance;
    [SerializeField] protected LayerMask _groundLayer;

    private Rigidbody2D _rigidbody;
    private Vector2 moveInput;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
        IsOnGround();
    }

    private bool IsOnGround()
    {
        return Physics2D.BoxCast(transform.position, _boxSize, 0, -transform.up, _castDistance, _groundLayer); 
    }
    public void OnDrawGizmos()
    {
        // 박스 캐스트의 시작점과 크기 그리기
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position - new Vector2(0, _castDistance), _boxSize * 2);
    }
    public bool IsPlayerStopped()
    {
        return moveInput == Vector2.zero;
    }
    void Move()
    {
        Vector2 movement = moveInput * _moveSpeed * Time.fixedDeltaTime;
        if (_rigidbody.velocity.x <5.0f && -_rigidbody.velocity.x <5.0f)
            _rigidbody.AddForce(movement * _additionalForce, ForceMode2D.Force);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (IsOnGround() && value.isPressed)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }
}