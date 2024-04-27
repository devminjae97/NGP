using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed = 20f;
    [SerializeField] protected float _jumpForce = 10f;
    [SerializeField] protected float _swampJumpForce = 15f;
    [SerializeField] protected float _additionalForce = 500f;
    
    [SerializeField] protected Vector2 _groundCheckBoxSize;
    [SerializeField] protected float _groundCheckCastDistance;

    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected LayerMask _iceLayer;
    [SerializeField] protected LayerMask _goalLayer;
    [SerializeField] protected LayerMask _swampLayer;

    private Rigidbody2D _rigidbody;
    private Vector2 _moveInput;

    private float _iceVelocity = 12.0f;
    private float _groundVelocity = 5.0f;
    private float _swampVelocity = 2.0f;

    private bool _isControllable = false;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundCheckBoxSize = new Vector2(0.45f, 0.1f);
        _groundCheckCastDistance = 0.95f;

        _groundLayer = LayerMask.GetMask("Ground");
        _iceLayer = LayerMask.GetMask("Ice");
        _goalLayer = LayerMask.GetMask("Goal");
        _swampLayer = LayerMask.GetMask("Swamp");

        _isControllable = true;
    }

    void FixedUpdate()
    {
        if (_isControllable)
        {
            Move();
            InitLayer();
        }
    }

    void Move()
    {
        
        Vector2 movement = _moveInput * _moveSpeed * Time.fixedDeltaTime;
        if (IsOnGround() && !IsOnIce() && !IsOnSwamp()) //땅위에서
        {
            if (_rigidbody.velocity.x < _groundVelocity && -_rigidbody.velocity.x < _groundVelocity)
                _rigidbody.AddForce(movement * _additionalForce, ForceMode2D.Force);
        }
        else if (!IsOnGround() && IsOnIce() && !IsOnSwamp()) //얼음위에서
        {
            Vector2 iceMovement = new Vector2(_rigidbody.velocity.x * _iceVelocity, 0);
            if (_rigidbody.velocity.x < _iceVelocity && -_rigidbody.velocity.x < _iceVelocity)
                _rigidbody.AddForce(iceMovement, ForceMode2D.Force);
        }
        else if(!IsOnGround() && !IsOnIce() && IsOnSwamp()) //늪위에서
        {
            if (_rigidbody.velocity.x < _swampVelocity && -_rigidbody.velocity.x < _swampVelocity)
                _rigidbody.AddForce(movement*_additionalForce, ForceMode2D.Force);
        }
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if ((IsOnGround() || IsOnIce()) && value.isPressed && _isControllable)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
        else if(IsOnSwamp() && value.isPressed && _isControllable)
        {
            _rigidbody.AddForce(Vector2.up * _swampJumpForce, ForceMode2D.Impulse);
        }
    }

    public void SetCharacterControllability(bool b)
    {
        _isControllable = b;
    }

    public void SetCharacterSpeed(float f)
    {
        _rigidbody.velocity = new Vector2(f,0);
    }

    public bool IsPlayerStopped()
    {
        return _moveInput == Vector2.zero;
    }

    public bool IsOnGround()
    {
        return Physics2D.BoxCast(transform.position, _groundCheckBoxSize, 0, -transform.up, _groundCheckCastDistance, _groundLayer);
    }

    public bool IsOnIce()
    {
        return Physics2D.BoxCast(transform.position, _groundCheckBoxSize, 0, -transform.up, _groundCheckCastDistance, _iceLayer);
    }

    public bool IsOnGoal()
    {
        return Physics2D.BoxCast(transform.position, _groundCheckBoxSize, 0, -transform.up, _groundCheckCastDistance, _goalLayer);
    }

    public bool IsOnSwamp()
    {
        return Physics2D.BoxCast(transform.position, _groundCheckBoxSize, 0, -transform.up, _groundCheckCastDistance, _swampLayer);
    }

    private void InitLayer()
    {
        IsOnGround();
        IsOnIce();
        IsOnGoal();
        IsOnSwamp();
    }

    public void OnDrawGizmos()
    {
        // 박스 캐스트의 시작점과 크기 그리기
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position - new Vector2(0, _groundCheckCastDistance), _groundCheckBoxSize * 2);
    }
}