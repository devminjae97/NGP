using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed = 20f;
    [SerializeField] protected float _jumpForce = 10f;
    [SerializeField] protected float _swampJumpForce = 25f;
    [SerializeField] protected float _additionalForce = 500f;
    
    [SerializeField] protected Vector2 _groundCheckBoxSize;
    [SerializeField] protected float _groundCheckCastDistance;
    //[SerializeField] protected Vector2 _initPos;

    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected LayerMask _iceLayer;
    [SerializeField] protected LayerMask _goalLayer;
    [SerializeField] protected LayerMask _swampLayer;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private Vector2 _moveInput;

    private float _iceVelocity = 12.0f;
    private float _groundVelocity = 5.0f;
    private float _swampVelocity = 2.0f;

    private bool _isSinking = false;
    private bool _isControllable = false;
    private bool _isReversed = false;
    private bool _isJumping = false;

    void Awake()
    {
        InitializeValues();
    }

    private void InitializeValues()
    {
        //_initPos = transform.position;
        if(TryGetComponent<Rigidbody2D>(out var rigidbody))
        {
            _rigidbody = rigidbody;
        }

        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            _spriteRenderer = spriteRenderer;
        }
        if (TryGetComponent<Animator>(out var animator))
        {
            _animator = animator;
        }
        _groundCheckBoxSize = new Vector2(0.45f, 0.1f);
        _groundCheckCastDistance = 0.95f;

        _groundLayer = LayerMask.GetMask("Ground");
        _iceLayer = LayerMask.GetMask("Ice");
        _goalLayer = LayerMask.GetMask("Goal");
        _swampLayer = LayerMask.GetMask("Swamp");

        _isControllable = true;
        _isSinking = false;
        _isReversed = false;
    }

    void FixedUpdate()
    {
        if (_isControllable)
        {
            Move();
        }
    }

    void Move()
    {
        Vector2 movement = _moveInput * _moveSpeed * Time.fixedDeltaTime;
        _animator.SetFloat("moveInput", _moveInput.x);
        if (_moveInput.x == 1.0f)
        {
            _spriteRenderer.flipX = false;
        }
        else if(_moveInput.x == -1.0f)
        {
            _spriteRenderer.flipX = true;
        }

        if (_isReversed)
        {
            movement.y *= -1; // Reverse vertical movement
        }

        if (IsOnGround() && !IsOnIce() && !IsOnSwamp()) // On Ground
        {
            if (_rigidbody.velocity.x < _groundVelocity && _rigidbody.velocity.x > -_groundVelocity)
            {
                _rigidbody.AddForce(movement * _additionalForce, ForceMode2D.Force);
            }

            _isSinking = false;
        }
        else if (!IsOnGround() && IsOnIce() && !IsOnSwamp()) // On Ice
        {
            Vector2 iceMovement = new Vector2(_rigidbody.velocity.x * _iceVelocity, 0);
            if (_rigidbody.velocity.x < _iceVelocity && _rigidbody.velocity.x > -_iceVelocity)
            {
                _rigidbody.AddForce(iceMovement, ForceMode2D.Force);
            }

            _isSinking = false;
        }
        else if (!IsOnGround() && !IsOnIce() && IsOnSwamp()) // On Swamp
        {
            if (_rigidbody.velocity.x < _swampVelocity && -_rigidbody.velocity.x < _swampVelocity)
            {
                _rigidbody.AddForce(movement * _additionalForce, ForceMode2D.Force);
            }

            _isSinking = true;
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
        else if (IsOnSwamp() && value.isPressed && _isControllable)
        {
            _rigidbody.AddForce(Vector2.up * _swampJumpForce, ForceMode2D.Impulse);
        }
    }

    public void Reverse()
    {
        _isReversed = !_isReversed;
        _spriteRenderer.flipY = true;
        _rigidbody.gravityScale *= -1;

        _groundCheckCastDistance *= -1;
    }

    #region Set() >>>>

    public void SetCharacterControllability(bool b)
    {
        _isControllable = b;
    }

    public void SetCharacterSpeed(float f)
    {
        _rigidbody.velocity = new Vector2(f,0);
    }

    #endregion Set() >>>>

    #region bool() >>>>

    public bool IsPlayerStopped()
    {
        return _moveInput == Vector2.zero;
    }

    public bool IsSinking()
    {
        return _isSinking;
    }

    #endregion bool() >>>>

    #region IsOnLayer() >>>>

    private bool IsOnSurface(LayerMask layer)
    {
        return Physics2D.BoxCast(transform.position, _groundCheckBoxSize, 0, -transform.up, _groundCheckCastDistance, layer);
    }

    private bool IsOnGround()
    {
        return IsOnSurface(_groundLayer);
    }

    private bool IsOnIce()
    {
        return IsOnSurface(_iceLayer);
    }
    private bool IsOnSwamp()
    {
        return IsOnSurface(_swampLayer);
    }

    public bool IsOnGoal()
    {
        return IsOnSurface(_goalLayer);
    }

    #endregion IsOnLayer() >>>>

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 castPosition = transform.position - Vector3.up * _groundCheckCastDistance;
        Vector3 boxSize = new Vector3(_groundCheckBoxSize.x, _groundCheckBoxSize.y, 0);

        Gizmos.DrawWireCube(castPosition, boxSize);
    }
}