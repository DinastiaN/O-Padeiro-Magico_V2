using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private float damageAmount = 10f;

    Animator animator;

    public float moveSpeed = 0.2f;
    public float attackDistance = 1f;
    public float followDistance = 3f;

    Transform playerTransform;
    bool isFollowing = false;
    bool isAttacking = false;
    bool isRunning = false;
    bool isWalking = false;
    float waitTime;
    float waitCounter;

    public float walkTime = 2f;
    float walkCounter;
    public Vector3[] walkDirections = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

    void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        waitTime = Random.Range(5, 7);
        waitCounter = waitTime;

        ChooseAction();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= followDistance)
        {
            isFollowing = true;
            isRunning = false;
            isWalking = false;
        }

        if (isFollowing)
        {
            if (distance > followDistance)
            {
                // Parar de seguir
                isFollowing = false;
                isRunning = false;
                isWalking = false;
                animator.SetBool("isRunning", false);
            }
            else if (distance <= attackDistance)
            {
                // Ataque o jogador
                isAttacking = true;
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", true);
                Debug.Log("O Lobo está atrás de ti!");
            }
            else
            {
                transform.LookAt(playerTransform);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
        else if (isAttacking)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", true);
            playerStats.currentHealth -= damageAmount;
        }

        else
        {
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                if (!isWalking)
                {
                    isRunning = true;
                    isWalking = false;
                    ChooseAction();
                    waitCounter = waitTime;
                }
                else
                {
                    isRunning = false;
                    isWalking = true;
                    walkCounter = walkTime;
                    animator.SetBool("isRunning", true);
                    ChooseWalkDirection();
                }
            }

            if (isRunning)
            {
                animator.SetBool("isRunning", true);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            else if (isWalking)
            {
                walkCounter -= Time.deltaTime;

                if (walkCounter <= 0)
                {
                    isWalking = false;
                    ChooseAction();
                    waitCounter = waitTime;
                }

                if (isWalking)
                {
                    animator.SetBool("isRunning", true);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                }
            }
        }
    }

    void ChooseAction()
    {
        float randomValue = Random.value;

        if (randomValue < 0.3f)
        {
            isFollowing = false;
            isRunning = true;
            isWalking = false;
            animator.SetBool("isRunning", true);
        }
        else if (randomValue < 0.6f)
        {
            isFollowing = true;
            isRunning = false;
            isWalking = false;
            animator.SetBool("isRunning", false);
        }
        else
        {
            isFollowing = false;
            isRunning = false;
            isWalking = true;
            animator.SetBool("isRunning", true);
            ChooseWalkDirection();
        }
    }

    void ChooseWalkDirection()
    {
        transform.rotation = Quaternion.LookRotation(walkDirections[Random.Range(0, walkDirections.Length)]);
    }

    public void StartFollowing()
    {
        isFollowing = true;
    }
}