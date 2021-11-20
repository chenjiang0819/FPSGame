using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThirdPersonMovement))]
public class PlayerController : MonoBehaviour
{
    public float MouseX { get; set; }
    public float MouseY { get; set; }
    public bool Dead { get; set; }
    public bool Disabled { get; set; }
    public bool IsMoving { get; set; }
    public bool IsSprinting { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsJumping { get; set; }
    public bool CanJump { get; set; }
    public bool IsAiming { get; set; }
    public bool IsFiring { get; set; }
    public bool MovingKeyPressed { get; set; }
    public Vector2 MovementInput { get; set; }
    public Vector2 MovementInputRaw { get; set; }

    ThirdPersonMovement thirdPersonMovement;

    void Start()
    {
        MouseX = MouseY = 0f;
        IsMoving = IsSprinting = IsAiming = IsFiring = IsJumping = MovingKeyPressed = Disabled = Dead = false;
        IsGrounded = CanJump = true;
        MovementInput = MovementInputRaw = Vector2.zero;

        thirdPersonMovement = gameObject.GetComponent<ThirdPersonMovement>();
    }

    void Update()
    {
        // receive no input if the player is dead
        if (Dead) return;
        if (Disabled)
        {
            MouseX = MouseY = 0f;
            IsMoving = IsSprinting = IsAiming = IsFiring = IsJumping = MovingKeyPressed = false;
            MovementInput = MovementInputRaw = Vector2.zero;
            return;
        }

        MouseX = Input.GetAxisRaw("Mouse X");
        MouseY = Input.GetAxisRaw("Mouse Y");

        if (IsSprinting && Input.GetKeyUp(KeyCode.LeftShift))
        {
            IsSprinting = false;
        }

        // Skip movement input if is jumping
        if (IsJumping && !IsMoving) return;

        MovingKeyPressed = Input.GetKey(KeyCode.W) ||
                           Input.GetKey(KeyCode.A) ||
                           Input.GetKey(KeyCode.S) ||
                           Input.GetKey(KeyCode.D);
        MovementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        MovementInputRaw = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (!IsMoving && MovingKeyPressed)
        {
            IsMoving = true;
        }

        if (IsMoving && !MovingKeyPressed)
        {
            IsMoving = false;
            IsSprinting = false;
        }

        if (IsMoving && !IsSprinting && IsGrounded && Input.GetKeyDown(KeyCode.LeftShift))
        {
            IsAiming = IsFiring = false;
            IsSprinting = true;
        }

        if (!IsJumping && !IsAiming && Input.GetMouseButtonDown(1))
        {
            if (IsSprinting)
            {
                IsSprinting = false;
            }
            IsAiming = true;
        }

        if (IsAiming && Input.GetMouseButtonUp(1))
        {
            IsAiming = false;
            if (IsFiring) IsFiring = false;
        }

        if (IsAiming && !IsFiring && Input.GetMouseButtonDown(0))
        {
            IsFiring = true;
        }

        if (IsFiring && Input.GetMouseButtonUp(0))
        {
            IsFiring = false;
        }

        if (IsGrounded && CanJump && Input.GetKeyDown(KeyCode.Space))
        {
            IsJumping = true;
            if (IsAiming || IsFiring)
            {
                IsAiming = IsFiring = false;
            }
        }
    }

    public void IsDead()
    {
        IsMoving = IsSprinting = IsFiring = MovingKeyPressed = false;
        // MovementInput = MovementInputRaw = Vector2.zero;
    }
}
