using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationStateController : MonoBehaviour
{
    [SerializeField] Rig[] aimLayer;

    PlayerController controller;
    Animator animator;

    int isMovingHash;
    int isSprintingHash;
    int isGroundedHash;
    int isJumpingHash;
    int isAimingHash;
    int isFiringHash;
    int inputXHash;
    int inputYHash;

    [SerializeField] float aimDuration = .1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();

        isMovingHash = Animator.StringToHash("IsMoving");
        isSprintingHash = Animator.StringToHash("IsSprinting");
        isGroundedHash = Animator.StringToHash("IsGrounded");
        isJumpingHash = Animator.StringToHash("IsJumping");
        isAimingHash = Animator.StringToHash("IsAiming");
        isFiringHash = Animator.StringToHash("IsFiring");
        inputXHash = Animator.StringToHash("InputX");
        inputYHash = Animator.StringToHash("InputY");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(inputXHash, controller.MovementInput.x);
        animator.SetFloat(inputYHash, controller.MovementInput.y);
        animator.SetBool(isMovingHash, controller.IsMoving);
        animator.SetBool(isSprintingHash, controller.IsSprinting);
        animator.SetBool(isGroundedHash, controller.IsGrounded);
        animator.SetBool(isJumpingHash, controller.IsJumping);
        animator.SetBool(isAimingHash, controller.IsAiming);
        animator.SetBool(isFiringHash, controller.IsFiring);

        BlendAimLayer();
    }

    private void BlendAimLayer()
    {
        if (controller.IsAiming)
        {
            foreach (var layer in aimLayer)
            {
                layer.weight += Time.deltaTime / aimDuration;
            }
        }
        else
        {
            foreach (var layer in aimLayer)
            {
                layer.weight -= Time.deltaTime / aimDuration;
            }
        }
    }
}
