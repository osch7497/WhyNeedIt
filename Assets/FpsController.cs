using UnityEngine;
using UnityEngine.InputSystem;

public class FpsController : MonoBehaviour
{
    public float MoveSpeed;
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
        Vector3 moveInput = new Vector3(rawInput.x,0,rawInput.y) * MoveSpeed * Time.deltaTime;
        Debug.Log(moveInput);
        rb.linearVelocity = moveInput;
    }
}
