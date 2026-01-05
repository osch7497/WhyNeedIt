using UnityEngine;
using UnityEngine.InputSystem;

public class FpsController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float lookSpeed = 15f;
    public float gravity = -9.81f;

    public Transform playerCamera;
    
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation;
    
    private Rigidbody rb;
    private CustomInputs playerInput;
    
    
    void Awake()
    {
        playerInput = new CustomInputs();
        rb = GetComponent<Rigidbody>();
        
        if (playerCamera == null)
        {
            Debug.LogError("No Camera in FpsController");
        }
    }
    
    void OnEnable() => playerInput.Enable();
    void OnDisable() => playerInput.Disable();
    
    void Update()
    {
        // 플레이어 이동
        moveInput = playerInput.Player.Move.ReadValue<Vector2>();
        Vector3 velocity = rb.linearVelocity;
        
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
        velocity.x = move.x;
        velocity.z = move.z;

        // 중력 가속도 적용
        velocity.y += gravity * Time.fixedDeltaTime;

        rb.linearVelocity = velocity;
        
        // 화면 이동
        lookInput = playerInput.Player.Look.ReadValue<Vector2>();
        transform.Rotate(Vector3.up * lookInput.x * lookSpeed * Time.deltaTime);
        
        xRotation -= lookInput.y * lookSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
