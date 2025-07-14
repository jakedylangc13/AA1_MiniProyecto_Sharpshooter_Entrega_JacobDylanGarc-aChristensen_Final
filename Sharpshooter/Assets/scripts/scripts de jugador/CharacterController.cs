using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Dash")]
    public float dashForce = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public UnityEvent OnDashStart;

    [Header("Jump")]
    public float jumpForce = 7f;
    public float maxJumpTime = 0.3f;
    public LayerMask groundMask;
    public float groundRayLength = 0.2f;

    [Header("References")]
    public Transform cameraTransform;
    public Transform renderizado;
    public Transform groundCheckPoint;

    private Rigidbody rb;
    private PlayerInputHandler inputHandler;

    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;
    private Vector3 dashDirection;

    public bool isGrounded = false;
    public bool isJumping = false;
    private float jumpTimeCounter = 0f;

    public UnityEvent startedJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashForce;


            if (dashDirection != Vector3.zero)
            {
                Quaternion dashRotation = Quaternion.LookRotation(dashDirection);
                renderizado.rotation = Quaternion.Slerp(renderizado.rotation, dashRotation, Time.fixedDeltaTime * 10f);
            }

            return;
        }

        Vector2 input = inputHandler.MoveInput;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * input.y + camRight * input.x).normalized;
        Vector3 velocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
        rb.linearVelocity = velocity;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            renderizado.rotation = Quaternion.Slerp(renderizado.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }

        // Apply jump force
        if (isJumping && jumpTimeCounter > 0)
        {
            Debug.Log("JUMPY");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            jumpTimeCounter -= Time.fixedDeltaTime;
        }
    }


    private void Update()
    {
        // Dash input
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame || Gamepad.current?.buttonSouth.wasPressedThisFrame == true)
        {
            TryDash();
        }

        // Jump input
        bool jumpPressed = Keyboard.current.spaceKey.isPressed || Gamepad.current?.buttonNorth.isPressed == true;

        if (IsGrounded())
        {
            if (jumpPressed && !isJumping)
            {
                startedJump?.Invoke();
                isJumping = true;
                jumpTimeCounter = maxJumpTime;
            }
        }

        if (!jumpPressed || jumpTimeCounter <= 0f)
        {
            isJumping = false;
        }
    }

    private void TryDash()
    {
        if (Time.time - lastDashTime < dashCooldown || isDashing)
            return;

        lastDashTime = Time.time;

        Vector2 input = inputHandler.MoveInput;
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 inputDirection = (camForward * input.y + camRight * input.x).normalized;
        dashDirection = inputDirection != Vector3.zero ? inputDirection : camForward;

        OnDashStart?.Invoke(); //
        StartCoroutine(DashRoutine());
    }

    private System.Collections.IEnumerator DashRoutine()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    private bool IsGrounded()
    {
        Vector3 origin = groundCheckPoint != null ? groundCheckPoint.position : transform.position;
        isGrounded = Physics.Raycast(origin, Vector3.down, groundRayLength, groundMask);
        return isGrounded;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = groundCheckPoint != null ? groundCheckPoint.position : transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + Vector3.down * groundRayLength);
    }
}