using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FpsController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float multiplierRunSpeed = 1.5f;
    public float lookSpeed = 15f;
    public float maxRunningTime = 10f;
    public float maxFloatingTime = 13f;
    public Image staminaBar;

    public Transform playerCamera;
    private Vector3 cameraOriginPos;
    
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isRunning = false;
    private float currentStamina;
    private float xRotation;
    private float sinWaving;
    
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
        
        cameraOriginPos = playerCamera.localPosition;
        currentStamina = 100f;
    }
    
    void OnEnable() => playerInput.Enable();
    void OnDisable() => playerInput.Disable();
    
    void Update()
    {
        // 플레이어 이동
        moveInput = playerInput.Player.Move.ReadValue<Vector2>();
        isRunning = playerInput.Player.Run.IsPressed();
        
        Vector3 moveDir = playerCamera.forward * moveInput.y + playerCamera.right * moveInput.x;
        moveDir.y = 0f;

        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveDir.x * moveSpeed;
        velocity.z = moveDir.z * moveSpeed;

        if (isRunning && currentStamina > 0.1f)
        {
            velocity.x *= multiplierRunSpeed;
            velocity.z *= multiplierRunSpeed;
            currentStamina -= (100f / maxRunningTime) * Time.deltaTime;
        }
        
        if (currentStamina < 100f && !isRunning) currentStamina += (100f / maxFloatingTime) * Time.deltaTime;
        
        Debug.Log(currentStamina);
        Debug.Log(velocity);
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
        
        
        // 머리 흔들림
        if (moveInput != Vector2.zero)
        {
            sinWaving += Time.deltaTime * ((isRunning && currentStamina > 0.1f) ? 14f : 9f);
            float yOffset = Mathf.Sin(sinWaving) * ((isRunning && currentStamina > 0.1f) ? 0.13f : 0.07f);

            playerCamera.transform.localPosition =
                cameraOriginPos + new Vector3(0f, yOffset, 0f);
        }
        else
        {
            sinWaving = 0f;
            playerCamera.transform.localPosition =
                Vector3.Lerp(playerCamera.transform.localPosition, cameraOriginPos, Time.deltaTime * 20f);
        }
    }
}
