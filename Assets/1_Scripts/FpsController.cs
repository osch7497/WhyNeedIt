using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.Serialization;

public class FpsController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float multiplierRunSpeed = 1.5f;
    public float lookSpeed = 15f;
    public float maxRunningTime = 10f;
    public float maxFloatingTime = 13f;
    public Image staminaBar;
    
    [Header("Camera")]
    public Camera playerCamera;
    private Vector3 cameraOriginPos;
    
    [FormerlySerializedAs("waveMultipler")] [Range(0,2.0f)][SerializeField]
    private float waveForce;
    [FormerlySerializedAs("waveMultiplier")] [Range(0, 2.0f)] [SerializeField]
    private float waveSpeed;
    [Range(30f,180.0f)] [SerializeField]
    private float DefaultFov;
    [FormerlySerializedAs("waveMultiplier")] [Range(30f,180.0f)] [SerializeField]
    private float RunFov;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isRunning = false;
    private float currentStamina;
    private float xRotation;
    private float sinWaving;

    private Rigidbody rb;
    private CustomInputs playerInput;

    private Color emptyColor;
    private Color CannotRunColor;


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

        cameraOriginPos = playerCamera.transform.localPosition;
        currentStamina = 100f;
        
        emptyColor = new Color(34 / 255f, 34 / 255f, 34 / 255f);
        CannotRunColor = new Color(171 / 255f, 41 / 255f, 14 / 255f);
        Application.targetFrameRate = 120;
    }

    void OnEnable() => playerInput.Enable();
    void OnDisable() => playerInput.Disable();

    void Update()
    {
        // 플레이어 이동
        moveInput = playerInput.Player.Move.ReadValue<Vector2>();

        // 방향 구하기
        Vector3 moveDir = playerCamera.transform.forward * moveInput.y + playerCamera.transform.right * moveInput.x;
        moveDir.y = 0f;

        // 속도 부여
        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveDir.x * moveSpeed;
        velocity.z = moveDir.z * moveSpeed;

        if (currentStamina > 30f)
        {
            staminaBar.color = Color.white;
        }
        
        // RUN키가 현재 프레임에 눌렀을때 스태미나가 30 이상이면 달리기
        if (playerInput.Player.Run.WasPressedThisFrame())
        {
            if (currentStamina >= 30f)
            {
                isRunning = true;
            }
            else
            {
                staminaBar.color = emptyColor;
            }
        }

        // RUN 키가 현재 프레임에서 떼지면 달리기 해제
        if (playerInput.Player.Run.WasReleasedThisFrame())
        {
            isRunning = false;

            if (currentStamina <= 30f)
            {
                staminaBar.color = CannotRunColor;
            }
        }

        // 달리는 도중이라면
        if (isRunning)
        {
            playerCamera.fieldOfView = RunFov;
            velocity.x *= multiplierRunSpeed;
            velocity.z *= multiplierRunSpeed;

            if (currentStamina >= 0.1f)
            {
                currentStamina -= (100f / maxRunningTime) * Time.deltaTime;
            }
            else
            {
                isRunning = false;
            }
        }
        
        // 스태미나는 없는데 달리기 키가 아직 눌러져 있는 경우
        else if (playerInput.Player.Run.IsPressed())
        {
            if (currentStamina >= 0.1f && staminaBar.color != emptyColor)
            {
                currentStamina -= (100f / maxRunningTime) * Time.deltaTime;
            } 
        }

        else
        {
            playerCamera.fieldOfView = DefaultFov;
            if (currentStamina < 100f) currentStamina += (100f / maxFloatingTime) * Time.deltaTime;
        }

        staminaBar.transform.localScale = new Vector3(currentStamina / 100f,
            staminaBar.transform.localScale.y,
            staminaBar.transform.localScale.z);

        rb.linearVelocity = velocity;

        // 화면 이동
        lookInput = playerInput.Player.Look.ReadValue<Vector2>();
        transform.Rotate(Vector3.up * lookInput.x * lookSpeed * Time.deltaTime);

        xRotation -= lookInput.y * lookSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }


        // 머리 흔들림
        if (moveInput != Vector2.zero)
        {
            sinWaving += Time.deltaTime * ((isRunning && currentStamina > 0.1f) ? 14f : 9f) * waveSpeed;
            float yOffset = Mathf.Sin(sinWaving) * ((isRunning && currentStamina > 0.1f) ? 0.10f : 0.05f) * waveForce;

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
