using UnityEngine;
using UnityEngine.InputSystem;

public class FpsController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float lookSpeed = 15f;

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
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void OnEnable() => playerInput.Enable();
    void OnDisable() => playerInput.Disable();
    
    void Update()
    {
        // 플레이어 이동
        moveInput = playerInput.Player.Move.ReadValue<Vector2>();
        
        Vector3 moveDir = playerCamera.forward * moveInput.y + playerCamera.right * moveInput.x;
        moveDir.y = 0f;

        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveDir.x * moveSpeed;
        velocity.z = moveDir.z * moveSpeed;

        rb.linearVelocity = velocity;

        Debug.Log(rb.linearVelocity);

        
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
