using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; set;}

    public bool playerDied = false;

    public float currentHealth;
    public float maxHealth;


    public float currentMana;
    public float maxMana;
    internal object playerStats;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;


        currentMana = maxMana;
    }

    void Update()
    {
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
    }


    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void setMana(float newMana)
    {
        currentMana = newMana;
    }

    private void Die()
    {
        playerDied = true;
        Debug.Log("O jogador morreu!");
    }
}
