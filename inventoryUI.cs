using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventoryUI : MonoBehaviour
{
    inventory inven;
    public GameObject inventoryPanel;
    bool activeInventory = true;

    public slot[] slots;
    public Transform slotHolder;

    // Start is called before the first frame update
    private void Start()
    {
        inven = inventory.instance;
        slots = slotHolder.GetComponentsInChildren<slot>();
        inven.onSlotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        inventoryPanel.SetActive(activeInventory); 
    }

    public void RedrawSlotUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        for(int i = 0; i < inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();
        }
    }
    private void SlotChange(int vat)
    {
        for (int i = 0; i < slots.Length; i++)
        { 
            if(i < inven.SlotCnt)
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       // RedrawSlotUI();
    }
}
