using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManaBar : MonoBehaviour
{

    private Slider slider;
    public TextMeshProUGUI manaCounter;

    public GameObject playerStats;
    private float currentMana, maxMana;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }




    void Update()
    {
        currentMana = playerStats.GetComponent<PlayerStats>().currentMana;
        maxMana = playerStats.GetComponent<PlayerStats>().maxMana;


        float fillValue = currentMana / maxMana;
        slider.value = fillValue;

        manaCounter.text = currentMana + "/" + maxMana;
    }
}
