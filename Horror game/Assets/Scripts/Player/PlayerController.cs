using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 2.0f;
    [SerializeField] private float mouseSensitivity = 120f;

    // ChildTransform
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform FlashlightPivot;
    [SerializeField] private PlayerFootstep playerFootstep;

    [SerializeField] private float gravity = -20f;
    private float verticalVelocity;
    private Light flashlightLight;

    private Vector2 moveInput;
    private Vector2 lookInput;

    // Action bool
    private bool isCurrentRun;
    private bool isCurrentCrouch;

    private CharacterController controller;
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private PlayerDropItem playerDropItem;
    [SerializeField] private InteractionUIManager interactionUIManager;

    private float cameraVerticalAngle;

    [SerializeField] private float crouchSmoothSpeed = 8f;

    private float targetHeight;
    private Vector3 targetCenter;
    private Vector3 targetCameraPosition;

    public bool canControl;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        flashlightLight = FlashlightPivot.GetComponentInChildren<Light>();
        canControl = true;
    }

    private void Update()
    {
        if (canControl)
        {
            MovePlayer();
            RotatePlayer();

            if (playerFootstep != null)
            {
                playerFootstep.UpdateFootstep(moveInput, isCurrentRun, isCurrentCrouch);
            }
        }

        PlayerCrouch();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!canControl)
        {
            return;
        }
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!canControl)
        {
            return;
        }
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (!canControl)
        {
            return;
        }
        if (context.performed)
        {
            isCurrentRun = true;
        }
        if (context.canceled)
        {
            isCurrentRun = false;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (!canControl)
        {
            return;
        }
        if (context.performed)
        {
            isCurrentCrouch = true;
        }
        if (context.canceled)
        {
            isCurrentCrouch = false;
        }
    }

    public void OnFlashlight(InputAction.CallbackContext context)
    {
        if (!canControl)
        {
            return;
        }
        if (context.performed)
        {
            flashlightLight.enabled = !flashlightLight.enabled;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (interactionUIManager != null && interactionUIManager.IsOpen)
        {
            interactionUIManager.Hide();
            return;
        }

        if (!canControl)
        {
            return;
        }

        playerInteraction.TryInteract();
    }

    public void OnDropItem(InputAction.CallbackContext context)
    {
        if (!canControl)
        {
            return;
        }
        if (context.performed)
        {
            playerDropItem.DropItem();
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (interactionUIManager != null && interactionUIManager.IsOpen)
        {
            interactionUIManager.Hide();
        }
    }

    private void MovePlayer()
    {
        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        Vector3 move = new Vector3(horizontal, 0, vertical);
        move = transform.TransformDirection(move);

        float speed = walkSpeed;

        if (isCurrentCrouch)
        {
            speed = crouchSpeed;
        }
        else if (isCurrentRun)
        {
            speed = runSpeed;
        }

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        move = move * speed;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

    private void PlayerCrouch()
    {
        if (isCurrentCrouch)
        {
            targetHeight = 0.9f;
            targetCenter = new Vector3(0f, 0.45f, 0f);
            targetCameraPosition = new Vector3(0f, 0.9f, 0f);
        }
        else
        {
            targetHeight = 1.8f;
            targetCenter = new Vector3(0f, 0.9f, 0f);
            targetCameraPosition = new Vector3(0f, 1.6f, 0f);
        }

        controller.height = Mathf.Lerp(controller.height, targetHeight, crouchSmoothSpeed * Time.deltaTime);
        controller.center = Vector3.Lerp(controller.center, targetCenter, crouchSmoothSpeed * Time.deltaTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetCameraPosition, crouchSmoothSpeed * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        float horizontal = lookInput.x;
        float vertical = lookInput.y;
        float currentSensitivity = mouseSensitivity;

        if (SettingsManager.Instance != null)
        {
            currentSensitivity = SettingsManager.Instance.MouseSensitivity;
        }
        cameraVerticalAngle -= vertical * currentSensitivity * Time.deltaTime;
        if (cameraVerticalAngle > 80)
        {
            cameraVerticalAngle = 80;
        }
        else if (cameraVerticalAngle < -80)
        {
            cameraVerticalAngle = -80;
        }

        transform.Rotate(Vector3.up * horizontal * currentSensitivity * Time.deltaTime);
        cameraTransform.localRotation = Quaternion.Euler(cameraVerticalAngle, 0f, 0f);
        FlashlightPivot.localRotation = Quaternion.Euler(-cameraVerticalAngle, 0f, 0f);
    }

    public void SetCanControl(bool value)
    {
        canControl = value;

        if (!canControl)
        {
            moveInput = Vector2.zero;
            lookInput = Vector2.zero;
            isCurrentRun = false;
            isCurrentCrouch = false;
        }
    }
}
