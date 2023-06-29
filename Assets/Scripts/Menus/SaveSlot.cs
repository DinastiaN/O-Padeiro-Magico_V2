using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SaveSlot : MonoBehaviour
{
    Button button;
    public TextMeshProUGUI buttontext;

    public int slotNumber;



    public void Awake()
    {
        button = GetComponent<Button>();
        buttontext = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }


    public void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (isSlotEmpty())
            {
                //Save.Manager.Instance.SaveGame(slotNumber);
                DateTime dt = DateTime.Now;
                string time = dt.ToString("yyyy-MM-dd HH:mm");

                buttontext.text = "Saved Game " + slotNumber + " | " + time;


                DeselectButton();
            }
            else
            {
                //DisplayOverrideWarning
            }
        }
        );
    }

    private void DeselectButton()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }





    private bool isSlotEmpty()
    {
        throw new NotImplementedException();
    }
}
