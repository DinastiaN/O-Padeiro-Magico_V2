using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Movement : MonoBehaviour
{
    Animator animator;

    public float moveSpeed = 0.2f;
    public float attackDistance = 1f;
    public float followDistance = 3f;

    Transform playerTransform;
    bool isFollowing = false;
    bool isAttacking = false;
    bool isRunning = false;

    float waitTime;
    float waitCounter;

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
        if (isFollowing)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            if (distance <= attackDistance)
            {
                // Attack the player
                isAttacking = true;
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", true);
                Debug.Log("O Lobo está atrás de ti!");
            }
            else if (distance > followDistance)
            {
                // Stop following
                isFollowing = false;
                isRunning = false;
                animator.SetBool("isRunning", false);
            }
            else
            {
                // Move towards the player
                transform.LookAt(playerTransform);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
        else if (isAttacking)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", true);
            Debug.Log("O Lobo matou-te!");
        }
        else
        {
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                isRunning = true;
                ChooseAction();
                waitCounter = waitTime;
            }

            if (isRunning)
            {
                animator.SetBool("isRunning", true);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
    }

    void ChooseAction()
    {
        float randomValue = Random.value;

        if (randomValue < 0.5f)
        {
            isFollowing = false;
            isRunning = true;
            animator.SetBool("isRunning", true);
        }
        else
        {
            isFollowing = true;
            isRunning = false;
            animator.SetBool("isRunning", false);
        }
    }

    // Call this method when the player enters the AI's detection range
    public void StartFollowing()
    {
        isFollowing = true;
    }
}
