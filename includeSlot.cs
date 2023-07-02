using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;
//using static System.Net.Mime.MediaTypeNames;

public class includeSlot : MonoBehaviour, IPointerClickHandler ,IPointerEnterHandler, IPointerExitHandler
{
    public int slotnum;
    public item item;
    public Image itemIcon;
    public ItemManager itemManager;

    [Header("DefaultImages")]
    /*
    public Sprite turbo;
    public Sprite air;
    public Sprite intake;
    public Sprite throttleBody;
    public Sprite hicam;
    public Sprite throttle;
    public Sprite interCooler;
    public Sprite oilCooler;
    public Sprite radiator;*/
    public Sprite[] defaultPowerImgs;
    public Sprite[] defaultDriveImgs;
    public Sprite[] defaultSpecialImgs;

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
    /*
    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
        this.GetComponent<Button>().interactable = true;
    }
    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
        this.GetComponent<Button>().interactable = false;
    }
    */

    void Start()
    {
        CheckStatus();
    }


    public void OnMouseEnter()
    {
        //Debug.Log(this.item.itemType);
    }
    
    public void CheckStatus()
    {
        if(this.GetComponent<fieldItems>().item.itemType != ItemType.None)
        {
            this.GetComponent<Button>().interactable = true;
        }
        else
        {
            this.GetComponent<Button>().interactable = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //bool slotAvailable;
        if (eventData.clickCount == 2)
        {
            itemManager.CloseToolTip();
            item item = new item();

            /*
            item.itemType = this.GetComponent<fieldItems>().item.itemType;
            item.parts = this.GetComponent<fieldItems>().item.parts;
            item.itemName = this.GetComponent<fieldItems>().item.itemName;
            item.itemImage = this.GetComponent<fieldItems>().item.itemImage;
            item.torque = this.GetComponent<fieldItems>().item.torque;
            item.weight = this.GetComponent<fieldItems>().item.weight;
            item.damper = this.GetComponent<fieldItems>().item.damper;
            item.spring = this.GetComponent<fieldItems>().item.spring;
            item.grip = this.GetComponent<fieldItems>().item.grip;
            item.angle = this.GetComponent<fieldItems>().item.angle;
            item.breakVal= this.GetComponent<fieldItems>().item.breakVal;
            item.description= this.GetComponent<fieldItems>().item.description;*/
            //it.itemType = item.itemType;
            //item = this.GetComponent<fieldItems>().GetItem();
            //Debug.Log("-------------------" + item.torque);
            item = itemManager.CopyItem(this.GetComponent<fieldItems>().item);
            inventory.instance.AddItem(item);
            if (item.itemType == ItemType.PowerUp)
            {

                for (int i = 1; i < 10; i++)
                {
                    if (itemDatabase.instance.itemDB[i].parts == item.parts)  //(int)
                    {
                        this.GetComponent<fieldItems>().SetItem(itemDatabase.instance.itemDB[0]);
                        this.transform.GetChild(0).GetComponent<Image>().sprite = defaultPowerImgs[i - 1];
                        //itemManager.UpdatePowerTunings(itemDatabase.instance.itemDB[0], i - 1, -item.torque, -item.weight);
                        itemManager.UpdateEjectedTunings(item, itemDatabase.instance.itemDB[0], i - 1);
                    }
                }
            }
            else if(item.itemType == ItemType.DrivingUP)
            {
                for (int i = 28; i < 34; i++)
                {
                    if (itemDatabase.instance.itemDB[i].parts == item.parts)
                    {
                        this.GetComponent<fieldItems>().SetItem(itemDatabase.instance.itemDB[0]);
                        this.transform.GetChild(0).GetComponent<Image>().sprite = defaultDriveImgs[i - 28];
                        //itemManager.UpdatePowerTunings(itemDatabase.instance.itemDB[0], i - 1, -item.torque, -item.weight);
                        itemManager.UpdateEjectedTunings(item, itemDatabase.instance.itemDB[0], i - 28);
                    }
                }
            }
            else if(item.itemType == ItemType.Special)
            {
                for (int i = 46; i < 48; i++)
                {
                    if (itemDatabase.instance.itemDB[i].parts == item.parts)
                    {
                        this.GetComponent<fieldItems>().SetItem(itemDatabase.instance.itemDB[0]);
                        this.transform.GetChild(0).GetComponent<Image>().sprite = defaultDriveImgs[i - 46];
                        //itemManager.UpdatePowerTunings(itemDatabase.instance.itemDB[0], i - 1, -item.torque, -item.weight);
                        itemManager.UpdateEjectedTunings(item, itemDatabase.instance.itemDB[0], i - 46);
                    }
                }
            }
            
            this.GetComponent<Button>().interactable = false;
            /*
            Debug.Log(this.item.itemName);
            bool isUse = item.Use();
            if (isUse)
            {
                
                slotAvailable = itemManager.CheckToInclude(item);
                if (slotAvailable)
                {
                    inventory.instance.RemoveItem(slotnum);
                }
                //inventory.instance.RemoveItem(slotnum);
            }*/
        }
        //Debug.Log(this.item.itemName);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.GetComponent<Button>().interactable == true)
        {
            //Debug.Log(this.item.itemName);
            itemManager.ShowToolTip(this.GetComponent<fieldItems>().item);
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
