using UnityEngine;
[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveThreshold = 0.1f;

    private Animator animator;
    private bool wasGrounded = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (characterController == null)
            characterController = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        if (characterController == null)
            return;

        Vector3 horizontalVelocity = characterController.GetComponent<Rigidbody>().linearVelocity;
        horizontalVelocity.y = 0f;

        bool isMoving = horizontalVelocity.magnitude > moveThreshold;
        bool isJumping = characterController.isJumping;
        bool isGrounded = characterController.isGrounded;

        // Set Run/Idle
        animator.SetBool("isRunning", isMoving);

        Debug.Log("MOVING" + isMoving);

        // Jump trigger (only when we go from grounded to jumping)
        if (wasGrounded && isJumping)
        {
            animator.SetBool("grounded", false);
            animator.SetTrigger("jump");
        }

        animator.SetBool("grounded", characterController.isGrounded);

        wasGrounded = isGrounded;
    }

    public void DashAnimation()
    {
        animator.SetTrigger("dash");

    }


}
