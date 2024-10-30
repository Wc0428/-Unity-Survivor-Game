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
        if (craftAxeBTN != null)
        {
            craftAxeBTN.onClick.AddListener(CraftAnyItem);
        }
    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }

    void CraftAnyItem()
    {
        // Check if inventory has required resources (pseudocode, replace with actual logic)
        bool hasResources = CheckInventoryForResources("Wood", "Stone");
        
        if (hasResources)
        {
            InventorySystem.Instance.AddToInventory("Axe");  // Replace "Axe" with item name as needed
            RemoveResourcesFromInventory("Wood", 2);  // Example: remove 2 Wood
            RemoveResourcesFromInventory("Stone", 1); // Example: remove 1 Stone
            Debug.Log("Crafted Axe and added to inventory.");
        }
        else
        {
            Debug.Log("Not enough resources to craft Axe.");
        }
    }

    bool CheckInventoryForResources(string resource1, string resource2)
    {
        // Add code to verify required resources
        return inventoryItemList.Contains(resource1) && inventoryItemList.Contains(resource2);
    }

    void RemoveResourcesFromInventory(string resource, int quantity)
    {
        // Add code to remove specified quantity from inventory
    }

    void Update()
    {
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
}
