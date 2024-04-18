using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] Vector2 boxSize;
    [SerializeField] float castDistance;
    [SerializeField] LayerMask groundLayer;

    private float currentMoveSpeed = 0f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
        IsOnGround();
    }
    private bool IsOnGround()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer)) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsPlayerStopped()
    {
        return currentMoveSpeed == 0;
    }
    void Move()
    {
        Vector2 movement = moveInput * moveSpeed *Time.deltaTime;
        transform.Translate(movement);

    }
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if (moveInput != Vector2.zero)
        {
            currentMoveSpeed = moveSpeed;
        }
        else
        {
            currentMoveSpeed = 0;
        }
    }

    public void OnJump(InputValue value)
    {
        if (IsOnGround() && value.isPressed)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}