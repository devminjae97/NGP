using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] protected float _cameraMoveSpeed = 5f;
    [SerializeField] protected float _followDelay = 0.5f;
    [SerializeField] protected Transform _target;
    [SerializeField] protected CharacterMovement _characterMovement;

    private Vector3 _defaultCameraPosition = new Vector3(0.0f, 0.0f, -10f);
    private Vector3 _targetPosition;
    private Vector3 _velocity = Vector3.zero;

    private void Awake()
    {
        _characterMovement = FindObjectOfType<CharacterMovement>();
        if (_target != null)
        {
            transform.position = _target.position + _defaultCameraPosition;
        }
    }

    private void Update()
    {
        if (_characterMovement.IsPlayerStopped() && IsUsing())
            MoveCameraWithInput();
        else
            FollowPlayer();
    }

    bool IsUsing()
    {
        if (GetInput() != new Vector2(0f,0f))
            return  true;
        else
            return false;
       
    }

    void FollowPlayer()
    {
        if (_target != null)
        {
            // 플레이어를 중심으로 카메라 이동
            _targetPosition = new Vector3(_target.position.x, _target.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, _followDelay);
        }
    }

    void MoveCameraWithInput()
    {
        // WASD 입력을 받아 카메라 이동
        Vector2 moveInput = GetInput();
        Vector3 moveDirection = new Vector3(moveInput.x, moveInput.y, 0f).normalized;
        transform.position += moveDirection * _cameraMoveSpeed * Time.deltaTime;
    }

    Vector2 GetInput()
    {
        Vector2 moveInput = Vector2.zero;

        moveInput.x = Keyboard.current.dKey.isPressed ? 1f : (Keyboard.current.aKey.isPressed ? -1f : 0f);
        moveInput.x += Gamepad.current?.dpad.x.ReadValue() ?? 0f;

        moveInput.y = Keyboard.current.wKey.isPressed ? 1f : (Keyboard.current.sKey.isPressed ? -1f : 0f);
        moveInput.x += Gamepad.current?.dpad.y.ReadValue() ?? 0f;

        return moveInput.normalized;
    }
}