using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSystem : MonoBehaviour
{

    public GameObject craftingScreenUI;
    public GameObject cozinharScreenUI;
    public GameObject farinhaScreenUI;

    public List<string> inventoryItemList = new List<string>();


    Button cozinharBTN;
    Button farinharBTN;

    Button cozinharPaoBTN;

    Button cozinharFarinhaBTN;

    TextMeshProUGUI PaoReq1, PaoReq2, FarinhaReq1, FarinhaReq2;

    public bool isOpen;

    public Blueprint PaoReceita = new Blueprint("Pão", 2, "Farinha", 3, "Água", 1);

    public Blueprint FarinhaReceita = new Blueprint("Farinha", 2, "Trigo", 5, "Água", 1);


    public static CraftingSystem Instance { get; set; }

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



    void Start()
    {
        isOpen = false;

        cozinharBTN = craftingScreenUI.transform.Find("PadariaButton").GetComponent<Button>();
        cozinharBTN.onClick.AddListener(delegate { OpenCookingCategory(); });

        farinharBTN = craftingScreenUI.transform.Find("FarinhaButton").GetComponent<Button>();
        farinharBTN.onClick.AddListener(delegate { OpenFarinhaCategory(); });


        //PÃEEES

        PaoReq1 = cozinharScreenUI.transform.Find("Tabua").transform.Find("Req1").GetComponent<TextMeshProUGUI>();
        PaoReq2 = cozinharScreenUI.transform.Find("Tabua").transform.Find("Req2").GetComponent<TextMeshProUGUI>();
        FarinhaReq1 = farinhaScreenUI.transform.Find("Tabua").transform.Find("Req1").GetComponent<TextMeshProUGUI>();
        FarinhaReq2 = farinhaScreenUI.transform.Find("Tabua").transform.Find("Req2").GetComponent<TextMeshProUGUI>();

        cozinharPaoBTN = cozinharScreenUI.transform.Find("Tabua").transform.Find("Cozinhar").GetComponent<Button>();
        cozinharPaoBTN.onClick.AddListener(delegate { Cozinhar(PaoReceita); });

        cozinharFarinhaBTN = farinhaScreenUI.transform.Find("Tabua").transform.Find("Farinhar").GetComponent<Button>();
        cozinharFarinhaBTN.onClick.AddListener(delegate { Cozinhar(FarinhaReceita); });

    }

    void OpenCookingCategory()
    {
        craftingScreenUI.SetActive(false);
        cozinharScreenUI.SetActive(true);
    }

    void OpenFarinhaCategory()
    {
        craftingScreenUI.SetActive(false);
        farinhaScreenUI.SetActive(true);
    }

    void Cozinhar(Blueprint blueprintToCraft)
    {
        InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);

        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }
        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }

        StartCoroutine(calculate());
    }


    public IEnumerator calculate()
    {
        yield return 0;
        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();
    }



    void Update()
    {

        if (Input.GetKeyDown(KeyCode.O) && !isOpen)
        {
            Debug.Log("O está a ser pressionado!");
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.O) && isOpen)
        {
            cozinharScreenUI.SetActive(false);
            craftingScreenUI.SetActive(false);
            farinhaScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            isOpen = false;
        }
    }

    public void RefreshNeededItems()
    {
        int cogumelo_count = 0;
        int farinha_count = 0;
        int trigo_count = 0;
        int água_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Cogumelo":
                    cogumelo_count += 1;
                    break;
                case "Farinha":
                    farinha_count += 1;
                    break;
                case "Trigo":
                    trigo_count += 1;
                    break;
                case "Água":
                    água_count += 1;
                    break;

            }
        }

        PaoReq1.text = "3 Farinha[" + farinha_count + "]";
        PaoReq2.text = "1 Água [" + água_count + "]";

        if (farinha_count >=3 && água_count >=1)
        {
            cozinharPaoBTN.gameObject.SetActive(true);
        }
        else
        {
            cozinharPaoBTN.gameObject.SetActive(false);
        }

        FarinhaReq1.text = "5 Trigo[" + trigo_count + "]";
        FarinhaReq2.text = "1 Água[" + água_count + "]";

        if (trigo_count >=5 && água_count >=1)
        {
            cozinharFarinhaBTN.gameObject.SetActive(true);
        }
        else
        {
            cozinharFarinhaBTN.gameObject.SetActive(false);
        }

    }




}
