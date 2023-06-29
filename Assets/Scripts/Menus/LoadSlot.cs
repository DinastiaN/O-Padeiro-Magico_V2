using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadSlot : MonoBehaviour
{

    public Button button;
    public TextMeshProUGUI buttontext;

    public int slotNumber;


    private void Awake()
    {
        button = GetComponent<Button>();
        buttontext = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }



    private void Update()
    {
        if (SaveManager.Instance.IsSlotEmpty(slotNumber))
        {
            buttontext.text = "";
        }
        else
        {
            buttontext.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }


    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if(SaveManager.Instance.IsSlotEmpty(slotNumber) == false)
            {
                SaveManager.Instance.StartLoadedGame(slotNumber);
                SaveManager.Instance.DeselectButton();
            }
            else
            {

            }

        });
    }

}
