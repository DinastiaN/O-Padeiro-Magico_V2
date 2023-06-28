using System;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public float damageAmount;
    public float attackDelay = 1.0f; // Tempo de espera entre os ataques
    public bool playerInRange = false;

    private DateTime nextAttackTime;
    public GameObject playerObj;

    private void Awake()
    {
        nextAttackTime = DateTime.Now;
    }

    private void FixedUpdate()
    {
        if (playerInRange && CanAttack())
        {
            DamagePlayer();
            nextAttackTime = DateTime.Now.AddSeconds(attackDelay); // Define o próximo tempo de ataque
        }
    }

    private bool CanAttack()
    {
        return DateTime.Now >= nextAttackTime; // Verifica se o tempo atual é maior ou igual ao próximo tempo de ataque
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerObj = other.gameObject;
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void DamagePlayer()
    {
        if (PlayerStats.Instance != null)
        {
            float healthBeforeDamage = PlayerStats.Instance.currentHealth;
            float maxHealth = PlayerStats.Instance.maxHealth;

            if (healthBeforeDamage != 0)
            {
                PlayerStats.Instance.setHealth(healthBeforeDamage - damageAmount);
            }
        }
    }
}
