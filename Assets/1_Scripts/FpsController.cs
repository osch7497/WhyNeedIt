using UnityEngine;
using UnityEngine.InputSystem;

public class FpsController : MonoBehaviour
{
    public float MoveSpeed = 3f;
    public float gravity = -9.81f;
    
    private Vector2 rawInput;
    private Vector2 lookInput;
    private Rigidbody rb;
    private CustomInputs playerInput;
    
    void Awake()
    {
        playerInput = new CustomInputs();
        rb = GetComponent<Rigidbody>();
    }
    
    void OnEnable() => playerInput.Enable();
    void OnDisable() => playerInput.Disable();
    
    void Update()
    {
        rawInput = playerInput.Player.Move.ReadValue<Vector2>();
    }
    
    void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;

        // 좌우 이동 (X, Z)
        Vector3 move = new Vector3(rawInput.x, 0, rawInput.y) * MoveSpeed;
        velocity.x = move.x;
        velocity.z = move.z;

        // 중력 가속도 적용
        velocity.y += gravity * Time.fixedDeltaTime;

        rb.linearVelocity = velocity;
    }
}
