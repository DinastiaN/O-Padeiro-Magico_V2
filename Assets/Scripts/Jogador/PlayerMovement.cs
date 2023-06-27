using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController character;
    public Transform cam;
    public Animator anim;
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
        character = GetComponent<CharacterController>();
        spellSystem = GetComponent<PlayerSpellSystem>();
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
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float sideMove = Input.GetAxis("Horizontal");
        float forwardMove = Input.GetAxis("Vertical");

        Vector3 sideMoveScreen = Quaternion.Euler(0f, cam.eulerAngles.y, 0f) * Vector3.right;
        Vector3 forwardMoveScreen = Quaternion.Euler(0f, cam.eulerAngles.y, 0f) * Vector3.forward;

        moveDirection = sideMoveScreen * sideMove + forwardMoveScreen * forwardMove;
        moveDirection = moveDirection.normalized;

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

        velocity.y += gravity * Time.deltaTime;
        character.Move(moveDirection * Time.deltaTime + velocity * Time.deltaTime);

        // Rotate character towards movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion characterLookRotation = Quaternion.LookRotation(moveDirection);
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, characterLookRotation, Time.deltaTime * 10f);
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
