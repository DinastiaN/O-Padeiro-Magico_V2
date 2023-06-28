using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; set;}


    public float currentHealth;
    public float maxHealth;


    public float currentMana;
    public float maxMana;


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
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }


    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void setMana(float newMana)
    {
        currentMana = newMana;
    }
}