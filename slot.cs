using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class slot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int slotnum;
    public item item;
    public Image itemIcon;
    public ItemManager itemManager;
    /*void Update() 
    {
        if(item.itemName == "") 
        {
            this.GetComponent<Button>().interactable= false;
        }
        else
        {
            Debug.Log(this.item.itemName);
            this.GetComponent<Button>().interactable = true;

        }
    }*/
    void Awake()
    {
        itemManager = GameObject.FindGameObjectWithTag("MenuPageManager").GetComponent<ItemManager>();
    }
    public void UpdateSlotUI()
    {
       // Debug.Log("UpdateSlot");
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
        this.GetComponent<Button>().interactable = true;
    }
    public void RemoveSlot()
    {
        //Debug.Log("RemoveSlot");
        item = null;
        itemIcon.gameObject.SetActive(false);
        this.GetComponent<Button>().interactable = false;
    }

    


    public void OnMouseEnter()
    {
        //Debug.Log(this.item.itemType);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        bool slotAvailable;
        if (eventData.clickCount == 2)
        {
            itemManager.CloseToolTip();
            //Debug.Log(slotnum);
            bool isUse = item.Use();
            if (true)
            {
                slotAvailable = itemManager.CheckToInclude(item);
               // Debug.Log(slotAvailable);
                if(slotAvailable)
                {
                    //Debug.Log(slotnum + item.itemName);
                    inventory.instance.RemoveItem(slotnum, item);
                    //Debug.Log(slotnum + item.itemName);
                }
                //inventory.instance.RemoveItem(slotnum);
            }
        }
        //Debug.Log(this.item.itemName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(this.GetComponent<Button>().interactable == true)
        {
            //Debug.Log(this.item.itemName);
            itemManager.ShowToolTip(item);
        }
        //Debug.Log(this.item.itemName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.GetComponent<Button>().interactable == true)
        {
            itemManager.CloseToolTip();
            //Debug.Log(this.item.itemName + "EXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
        }
       // Debug.Log(this.item.itemName + "EXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
    }
}
