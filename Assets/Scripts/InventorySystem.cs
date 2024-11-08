using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventorySystem : MonoBehaviour
{
    
  
    public static InventorySystem Instance { get; set; }
 
    public GameObject inventoryScreenUI;

    public List <GameObject> slotList = new List<GameObject>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;
    public bool isOpen;
 
    //public bool isFull;
    
    //pickup Popup
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

    public GameObject ItemInfoUi;
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
        PopulateSlotList();

        Cursor.visible = false;
    }
 
    private void PopulateSlotList() 
    {
        foreach(Transform child in inventoryScreenUI.transform)
        {
            if(child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);

            }

        }

    }
    void Update()
    {
 
        if (Input.GetKeyDown(KeyCode.B) && !isOpen)
        {
 
		    Debug.Log("B is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
 
        }
        else if (Input.GetKeyDown(KeyCode.B) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            isOpen = false;
        }
    }
 

public void AddToInventory(string itemName)
{

        whatSlotToEquip = FindNextEmptySlot();

        itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);

        Sprite sprite = itemToAdd.GetComponent<Image>().sprite;
        TriggerPickupPopUp(itemName, sprite);

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeedItems();

}

    void TriggerPickupPopUp(string itemName, Sprite itemSprite)
        {
            pickupAlert.SetActive(true);

            pickupName.text = itemName;

            pickupImage.sprite = itemSprite;

            StartCoroutine(HidePickupAlertAfterDelay(2f));
        }

    private IEnumerator HidePickupAlertAfterDelay(float delay)
        {
        
            yield return new WaitForSeconds(delay);

            
            pickupAlert.SetActive(false);
        }
    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount == 0)
            {
                return slot;
            }
        }

        return new GameObject();

    }

    public bool CheckIfFull()
    {
        int counter = 0;
        foreach (GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                counter +=1;
            }
        }

        if (counter == 21)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for(var i=slotList.Count-1; i>=0; i--)
        {
            if(slotList[i].transform.childCount >0)
            {
                if(slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter !=0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);

                    counter -= 1;

                }


            }


        }

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeedItems();

    }

    public void ReCalculateList()
    {
        itemList.Clear();

        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount >0)
            {
                String name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                itemList.Add(result);
            }
        }

    }



}

