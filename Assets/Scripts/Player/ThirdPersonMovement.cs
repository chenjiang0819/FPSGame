using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] float runSpeed = 3.8f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float aimSpeed = 2.4f;
    [SerializeField] float sprintSpeed = 10f;

    [Header("Gravity")]
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float groundDistance = .4f;
    [SerializeField] float aboutToLandDistance = 1f;

    [Header("Camera")]
    [SerializeField] float turnSmooth = .1f;
    [SerializeField] float aimTurnSmooth = .1f;

    CharacterController characterController;
    PlayerController playerController;
    Transform mainCam;

    private float speed;
    Vector3 velocity = Vector3.zero;
    bool isGrounded = true;
    LayerMask groundMask;
    float turnSmoothVelocity;
    Vector3 dashingDirection = Vector3.zero;
    bool jumpStarted = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
        mainCam = SceneManager.Instance.mainCam.transform;

        speed = runSpeed;
        groundMask = LayerMask.GetMask("Environment");
    }

    void Update()
    {
        if (playerController.IsAiming)
        {
            speed = aimSpeed;
        }
        else if (playerController.IsSprinting)
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = runSpeed;
        }

        HandleMovement();
        HandleGravity();
    }

    private void HandleGravity()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        if (isGrounded)
        {
            playerController.IsGrounded = true;

            if (velocity.y < 0) velocity.y = -2f;
        }

        if (!isGrounded)
        {
            RaycastHit hit;
            bool aboutToLand = Physics.Raycast(transform.position, Vector3.down, out hit, 10f, groundMask);
            if (aboutToLand && (hit.point - transform.position).magnitude <= aboutToLandDistance)
                playerController.IsGrounded = true;
            else
                playerController.IsGrounded = false;
        }

        if (jumpStarted)
        {
            velocity.y += jumpSpeed;
            jumpStarted = false;
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime * jumpHeight);
    }

    private void HandleMovement()
    {
        if (!playerController.IsAiming)
        {
            if (playerController.MovementInputRaw.magnitude >= 0.1f)
                AlignRotationToMovementAndMove(new Vector3(playerController.MovementInputRaw.x, 0f, playerController.MovementInputRaw.y));
        }
        else
            AlignRotationToCameraAndMove();
    }

    private void AlignRotationToCameraAndMove()
    {
        Vector3 direction = Vector3.zero;
        direction += playerController.MovementInputRaw.x * transform.right;
        direction += playerController.MovementInputRaw.y * transform.forward;
        direction = direction.normalized;

        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, mainCam.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aimTurnSmooth);

        characterController.Move(direction * speed * Time.deltaTime);
    }

    private void AlignRotationToMovementAndMove(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        characterController.Move(moveDir.normalized * speed * Time.deltaTime);
    }

    public void StartJumping()
    {
        playerController.CanJump = false;
        jumpStarted = true;
    }

    public void StopJumping()
    {
        playerController.CanJump = true;
        playerController.IsJumping = false;
    }
}
