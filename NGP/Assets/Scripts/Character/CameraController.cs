using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] protected float _cameraMoveSpeed = 5f;
    [SerializeField] protected float _followDelay = 0.5f;
    [SerializeField] protected CharacterMovement _target;

    private Vector3 _defaultCameraPosition = new Vector3(0.0f, 0.0f, -10f);
    private Vector3 _targetPosition;
    private Vector3 _velocity = Vector3.zero;

    private bool _isActive;

    private void Awake()
    {
        if (_target != null)
        {
            transform.position = _target.transform.position + _defaultCameraPosition;
            _isActive = true;
        }
    }

    private void Update()
    {
        if (_isActive)
        {
            if (_target.IsPlayerStopped() && IsUsing())
                MoveCameraWithInput();
            else
                FollowPlayer();
        }
    }
    public void IsActive(bool b)
    {
        _isActive = b;
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
            _targetPosition = new Vector3(_target.transform.position.x, _target.transform.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, _followDelay);
        }
    }

    void MoveCameraWithInput() //Move Camera Sight by WASD
    {
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