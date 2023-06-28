using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{

    private Slider slider;
    public TextMeshProUGUI healthCounter;

    public GameObject playerStats;
    private float currentHealth, maxHealth;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

 


    void Update()
    {
        currentHealth = playerStats.GetComponent<PlayerStats>().currentHealth;
        maxHealth = playerStats.GetComponent<PlayerStats>().maxHealth;

        healthCounter.text = currentHealth + "/" + maxHealth;
    }
}
