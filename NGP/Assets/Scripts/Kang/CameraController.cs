using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; // 카메라 이동 속도
    [SerializeField] float followDelay = 0.5f; // 딜레이 시간
    [SerializeField] Transform target; // 플레이어의 Transform
    [SerializeField] CharacterMovement characterMovement;

    private Vector3 defaultCameraPosition = new Vector3(0.0f, 0.0f, -10f);
    private Vector3 targetPosition; // 목표 위치
    private Vector3 velocity = Vector3.zero; // 현재 속도
 
    private void Start()
    {
        characterMovement = FindObjectOfType<CharacterMovement>();
        transform.position = target.position + defaultCameraPosition;
    }
    private void Update()
    {
        if (characterMovement.IsPlayerStopped())
            MoveCameraWithInput();
        else
            FollowPlayer();
    }

    void FollowPlayer()
    {
        if (target != null)
        {
            // 플레이어를 중심으로 카메라 이동
            targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followDelay);
        }
    }
    void MoveCameraWithInput()
    {
        // WASD 입력을 받아 카메라 이동
        Vector2 moveInput = GetInput();
        Vector3 moveDirection = new Vector3(moveInput.x, moveInput.y, 0f).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    Vector2 GetInput()
    {
        Vector2 moveInput = Vector2.zero;

        moveInput.x = Keyboard.current.dKey.isPressed ? 1f : (Keyboard.current.aKey.isPressed ? -1f : 0f);
        moveInput.x += Gamepad.current?.leftStick.x.ReadValue() ?? 0f;

        moveInput.y = Keyboard.current.wKey.isPressed ? 1f : (Keyboard.current.sKey.isPressed ? -1f : 0f);
        moveInput.y += Gamepad.current?.leftStick.y.ReadValue() ?? 0f;

        return moveInput.normalized;
    }
}