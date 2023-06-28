using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    private GameObject itemInfoUI;

    private TextMeshProUGUI itemInfoUI_itemName;
    private TextMeshProUGUI itemInfoUI_itemDescription;
    private TextMeshProUGUI itemInfoUI_itemFunctionality;

    public string thisName, thisDescription, thisFunctionality;

    private GameObject itemPendingConsumption;
    public bool isConsumable;

    public float healthEffect;
    public float manaEffect;

    private void Start()
    {
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("ItemFunctionality").GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable)
            {
                itemPendingConsumption = gameObject;
                consumingFunction(healthEffect, manaEffect);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.ReCalculateList();
                CraftingSystem.Instance.RefreshNeededItems();
            }
        }
    }
    private void consumingFunction(float healthEffect, float manaEffect)
    {
        itemInfoUI.SetActive(false);

        healthEffectCalculation(healthEffect);

        manaEffectCalculation(manaEffect);

    }

    private static void healthEffectCalculation(float healthEffect)
    {
        float healthBeforeConsumption = PlayerStats.Instance.currentHealth;
        float maxHealth = PlayerStats.Instance.maxHealth;

        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerStats.Instance.setHealth(maxHealth);
            }
            else
            {
                PlayerStats.Instance.setHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }

    private static void manaEffectCalculation(float manaEffect)
    {
        float manaBeforeConsumption = PlayerStats.Instance.currentMana;
        float maxMana = PlayerStats.Instance.maxMana;

        if (manaEffect != 0)
        {
            if ((manaBeforeConsumption + manaEffect) > maxMana)
            {
                PlayerStats.Instance.setMana(maxMana);
            }
            else
            {
                PlayerStats.Instance.setMana(manaBeforeConsumption + manaEffect);
            }
        }
    }
}