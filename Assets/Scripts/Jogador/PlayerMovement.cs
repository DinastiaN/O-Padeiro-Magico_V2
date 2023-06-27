using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController character;
    public Animator anim;
    public float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    public Transform cameraTransform;

    private Vector3 moveDirection;
    private Vector3 velocity;
    private PlayerSpellSystem spellSystem;

    private bool isGrounded;
    private float groundCheckDistance;
    private LayerMask groundMask;
    private float gravity;

    private float jumpHeight;
    private float jumpCooldown;
    private float jumpTimer;
    private bool canJump = true;

    private bool isAiming = false;
    private bool altPressed = false;

    private void Start()
    {
        groundCheckDistance = 0.2f;
        groundMask = LayerMask.GetMask("Ground");
        gravity = -9.81f;

        jumpHeight = 3f;
        jumpCooldown = 1f;
        jumpTimer = jumpCooldown;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        spellSystem = GetComponent<PlayerSpellSystem>();
        if (spellSystem == null)
            spellSystem = gameObject.AddComponent<PlayerSpellSystem>();
    }

    private void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }

        if (!canJump)
        {
            jumpTimer += Time.deltaTime;

            if (jumpTimer >= jumpCooldown)
            {
                canJump = true;
                jumpTimer = 0f;
            }
        }

        RotatePlayer();

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            altPressed = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            altPressed = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Move()
    {
        isGrounded = character.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        moveDirection = (moveZ * cameraTransform.forward + moveX * cameraTransform.right).normalized;

        if (isGrounded)
        {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }

            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        if (!isGrounded)
        {
            moveDirection.x = 0f;
            moveDirection.z = 0f;
        }

        character.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        character.Move(velocity * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        if (!altPressed)
        {
            if (!isAiming && moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                Vector3 eulerRotation = targetRotation.eulerAngles;
                eulerRotation.x = 0f;
                targetRotation = Quaternion.Euler(eulerRotation);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
            else if (isAiming)
            {
                Vector3 cameraForward = cameraTransform.forward;
                cameraForward.y = 0f;

                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                Vector3 eulerRotation = targetRotation.eulerAngles;
                eulerRotation.x = 0f;
                targetRotation = Quaternion.Euler(eulerRotation);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }
    }

    private void Idle()
    {
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
    }

    private void Jump()
    {
        if (canJump)
        {
            anim.SetBool("Jump", true);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            canJump = false;
        }
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
        if (spellSystem != null)
            spellSystem.CastSpell();
    }
}
