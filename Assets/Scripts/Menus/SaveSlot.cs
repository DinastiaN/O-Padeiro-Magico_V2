using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SaveSlot : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI buttontext;

    public int slotNumber;


    public GameObject UIalerta;
    public Button BTNSim;
    public Button BTNNão;


    public void Awake()
    {
        button = GetComponent<Button>();
        buttontext = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        BTNSim = UIalerta.transform.Find("Sim").GetComponent<Button>();
        BTNNão = UIalerta.transform.Find("Não").GetComponent<Button>();
    }


    public void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (SaveManager.Instance.IsSlotEmpty(slotNumber))
            {
                SaveGameConfirmed();
            }
            else
            {
                DisplayOverriWarning();
            }
        }
        );
    }


    private void Update()
    {
        if (SaveManager.Instance.IsSlotEmpty(slotNumber))
        {
            buttontext.text = "Empty";
        }
        else
        {
            buttontext.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }
    public void DisplayOverriWarning()
        {
            UIalerta.SetActive(true);

            BTNSim.onClick.AddListener(() =>
            {
                SaveGameConfirmed();
                UIalerta.SetActive(false);
            });

            BTNNão.onClick.AddListener(() =>
            {
                UIalerta.SetActive(false);
            });
            }

    private void SaveGameConfirmed()
    {
        SaveManager.Instance.SaveGame(slotNumber);
        DateTime dt = DateTime.Now;
        string time = dt.ToString("yyyy-MM-dd     HH:mm");

        string description = "Saved Game " + slotNumber + " | " + time;

        buttontext.text = description;

        PlayerPrefs.SetString("Slot" + slotNumber + "Description", description);


        SaveManager.Instance.DeselectButton();
    }
}
