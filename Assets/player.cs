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

    [Header("Game Stats")]
    public int score = 0;

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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

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

        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }
}