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

    public slot[] PowerInvenSlots;
    public slot[] DriveInvenSlots;
    public slot[] SpecialInvenSlots;

    public Transform PowerSlotHolder;
    public Transform DriveSlotHolder;
    public Transform SpecialSlotHolder;


    public bool initial = true; 
    // Start is called before the first frame update
    private void Start()
    {
        inven = inventory.instance;
        PowerInvenSlots = PowerSlotHolder.GetComponentsInChildren<slot>();
        DriveInvenSlots = DriveSlotHolder.GetComponentsInChildren<slot>();
        SpecialInvenSlots = SpecialSlotHolder.GetComponentsInChildren<slot>();
        inven.onSlotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        inventoryPanel.SetActive(activeInventory); 
    }

    public void RedrawSlotUI()
    {
        for(int i = 0; i < PowerInvenSlots.Length; i++)
        {
            PowerInvenSlots[i].RemoveSlot();
        }
        for(int i = 0; i < inven.PowerItems.Count; i++)
        {
            //Debug.Log(inven.PowerItems[i].itemName + " ----------"+ inven.PowerItems.Count);
            PowerInvenSlots[i].item = inven.PowerItems[i];
            PowerInvenSlots[i].slotnum = i;
            PowerInvenSlots[i].UpdateSlotUI();
        }


        for(int i = 0; i < DriveInvenSlots.Length; i++)
        {
            DriveInvenSlots[i].RemoveSlot();
        }
        for(int i = 0; i<inven.DriveItems.Count; i++)
        {
            DriveInvenSlots[i].item = inven.DriveItems[i];
            DriveInvenSlots[i].slotnum = i;
            DriveInvenSlots[i].UpdateSlotUI();
        }

        for (int i = 0; i < SpecialInvenSlots.Length; i++)
        {
            SpecialInvenSlots[i].RemoveSlot();
        }
        for (int i = 0; i < inven.SpecialItems.Count; i++)
        {
            SpecialInvenSlots[i].item = inven.SpecialItems[i];
            SpecialInvenSlots[i].slotnum = i;
            SpecialInvenSlots[i].UpdateSlotUI();
        }


    }
    private void SlotChange(int val)
    {
        if (initial)
        {
            initial = !initial;
        }
        else
        {
            for (int i = 0; i < PowerInvenSlots.Length; i++)
            {
                PowerInvenSlots[i].slotnum = i;
                if (i < inven.SlotCnt)
                {
                    PowerInvenSlots[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    PowerInvenSlots[i].GetComponent<Button>().interactable = false;
                }
            }

            for (int i = 0; i < DriveInvenSlots.Length; i++)
            {
                if(i < inven.SlotCnt)
                {
                    DriveInvenSlots[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    DriveInvenSlots[i].GetComponent<Button>().interactable = false;
                }

            }

            for (int i = 0; i < SpecialInvenSlots.Length; i++)
            {
                if (i < inven.SlotCnt)
                {
                    SpecialInvenSlots[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    SpecialInvenSlots[i].GetComponent<Button>().interactable = false;
                }

            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       // RedrawSlotUI();
    }
}
