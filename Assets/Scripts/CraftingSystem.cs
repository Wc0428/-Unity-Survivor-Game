using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;

    public List<string> inventoryItemList = new List<string>();

    // Category Buttons
    private Button toolsBTN;

    // Craft Buttons
    private Button craftAxeBTN;

    // Requirement Text
    private Text AxeReq1, AxeReq2;

    public bool isOpen;

    public Blueprint AxeBLP = new Blueprint("Axe",2,"Stone",3,"Stick",3);
    public static CraftingSystem Instance { get; private set; }

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

        // Find Tools Button
        toolsBTN = craftingScreenUI.transform.Find("ToolsButton")?.GetComponent<Button>();
        if (toolsBTN != null)
        {
            toolsBTN.onClick.AddListener(OpenToolsCategory);
        }

        // Find Axe Requirements
        AxeReq1 = toolsScreenUI.transform.Find("Axe/req1")?.GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe/req2")?.GetComponent<Text>();

        // Find Craft Button for Axe
        craftAxeBTN = toolsScreenUI.transform.Find("Axe/Button")?.GetComponent<Button>();
        if(craftAxeBTN !=null)
        {
        craftAxeBTN.onClick.AddListener(delegate{CraftAnyItem(AxeBLP);});
        }
    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }

   void CraftAnyItem(Blueprint blueprintToCraft)
    {
        InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);

        if (blueprintToCraft.numOfRequirements >= 1)
        {   
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }

        if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }

        StartCoroutine(calculate());

        RefreshNeedItems();
    }

    public IEnumerator calculate()
    {
        yield return new WaitForSeconds(1f);

        InventorySystem.Instance.ReCalculateList();
    }
    void Update()
    {
        RefreshNeedItems();
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            isOpen = false;
        }

    }

   private void RefreshNeedItems()
    {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach(string itemName in inventoryItemList)
        {

            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;

                case "Stick":
                    stick_count += 1;
                    break;
            }
        }

        //---AXE---//

        AxeReq1.text = "3 Stone[" + stone_count +"]";
        AxeReq2.text = "3 Stick[" + stick_count +"]";

        if(stone_count >=3 && stick_count >=3)
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }
    }



}