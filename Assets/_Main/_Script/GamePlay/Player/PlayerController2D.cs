using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    
    //Input System when using Player Input Component without Dynamic Joystick

    // Vector2 moveVector;
    // public void InputPlayer(InputAction.CallbackContext context)
    // {
    //     moveVector = context.ReadValue<Vector2>();
    // }

    // private void Update()
    // {
    //     transform.Translate(moveVector * speed * Time.deltaTime);
    // }
    
    [SerializeField] private float speed = 5f;
    [SerializeField] private InputActionAsset inputAsset;

    private InputAction moveAction;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // moveAction = inputAsset.FindAction("Move");
    }

    private void Start()
    {
        moveAction = inputAsset.FindAction("Player/Move");
        if (moveAction != null)
        {
            moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            moveAction.canceled += ctx => moveInput = Vector2.zero;
            moveAction.Enable();
        }
    }

    private void Update()
    {
        
        rb.linearVelocity = moveInput * speed;
        // Debug.Log("Player Velocity: " + rb.linearVelocity.x + ", " + rb.linearVelocity.y);
    }


}