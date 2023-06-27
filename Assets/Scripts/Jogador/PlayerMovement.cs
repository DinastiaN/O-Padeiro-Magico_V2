using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController character;
    public Transform cam;
    public Animator anim;
    public float characterSpeed = 2f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpCooldown;
    private float jumpTimer;
    private bool canJump = true;
    private PlayerSpellSystem spellSystem;

    private void Start()
    {

    }


    private void Update()
    {
        float sideMove = 0;
        float forwardMove = 0;

        sideMove += Input.GetAxis("Horizontal");
        forwardMove += Input.GetAxis("Vertical");

        Vector3 sideMoveScreen = Quaternion.Euler(0f, cam.eulerAngles.y, 0f) * Vector3.right;
        Vector3 forwardMoveScreen = Quaternion.Euler(0f, cam.eulerAngles.y, 0f) * Vector3.forward;

        moveDirection = sideMoveScreen * sideMove + forwardMoveScreen * forwardMove;
        character.Move(moveDirection * characterSpeed * Time.deltaTime);

        Quaternion characterLookDirection = Quaternion.LookRotation(moveDirection);
        character.transform.rotation = characterLookDirection;

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
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(moveX, moveDirection.y, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

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

        character.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        character.Move(velocity * Time.deltaTime);
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
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            canJump = false;
        }
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
        spellSystem.CastSpell();
    }
}