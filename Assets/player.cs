using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input Setup")]
    public InputActionAsset inputActionsAsset;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 15f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private InputAction moveAction;
    private Vector2 movementInput;
    private float verticalVelocity;
    private Transform cameraTransform;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (inputActionsAsset != null)
        {
            moveAction = inputActionsAsset.FindAction("moves");
        }
    }

    private void OnEnable()
    {
        if (inputActionsAsset != null) inputActionsAsset.Enable();

        if (moveAction != null)
        {
            moveAction.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            moveAction.canceled += ctx => movementInput = Vector2.zero;
        }
    }

    private void OnDisable()
    {
        if (inputActionsAsset != null) inputActionsAsset.Disable();

        if (moveAction != null)
        {
            moveAction.performed -= ctx => movementInput = ctx.ReadValue<Vector2>();
            moveAction.canceled -= ctx => movementInput = Vector2.zero;
        }
    }

    private void Update()
    {
        // Fallback if Camera.main isn't tagged correctly
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        // 1. Calculate camera-relative movement direction
        Vector3 moveDir = Vector3.zero;
        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            moveDir = (camForward * movementInput.y + camRight * movementInput.x).normalized;
        }
        else
        {
            moveDir = new Vector3(movementInput.x, 0f, movementInput.y);
        }

        // 2. Handle Gravity
        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f; // Keeps the character grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = verticalVelocity;

        // 3. Move the CharacterController
        controller.Move(velocity * Time.deltaTime);

        // 4. Rotate smoothly toward movement direction
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}