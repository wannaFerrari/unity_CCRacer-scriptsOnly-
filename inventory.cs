using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using static UnityEditor.Progress;

public class inventory : MonoBehaviour
{

    public static inventory instance;

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<item> PowerItems = new List<item>();
    public List<item> DriveItems = new List<item>();
    public List<item> SpecialItems= new List<item>();

    private int slotCnt;
    public item test2;

    public int SlotCnt
    {
        get => slotCnt;
        set 
        { 
            slotCnt = value; 
            onSlotCountChange.Invoke(slotCnt);
        }
    }
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //SlotCnt = 27;
        
    }

    private void Start()
    {
        SlotCnt = 27;
        //Invoke("SetSlot", 1f);
        //Invoke("InitialMakeItem", 0.1f);
    }
    public void SetSlot()
    {
        SlotCnt = 27;
    }
    public bool AddItem(item _item)
    {
        if (_item.itemType == ItemType.PowerUp)
        {
            PowerItems.Add(_item);
            Debug.Log(_item.itemCode + "---" + _item.itemName);
        }
        else if(_item.itemType == ItemType.DrivingUP)
        {
            DriveItems.Add(_item);
        }
        else if(_item.itemType ==ItemType.Special)
        {
            SpecialItems.Add(_item);
        }
        if (onChangeItem != null)
        {
            onChangeItem.Invoke();
            //Debug.Log("invoke");
        }
        return true;
    }

    public void RemoveItem(int _index, item _item)
    {
        if(_item.itemType == ItemType.PowerUp)
        {
            PowerItems.RemoveAt(_index);
        }
        else if(_item.itemType == ItemType.DrivingUP)
        {
           // Debug.Log(DriveItems[_index].itemName);
            DriveItems.RemoveAt(_index);
        }
        else if(_item.itemType == ItemType.Special)
        {
            SpecialItems.RemoveAt(_index);
        }

        onChangeItem.Invoke();
    }

    public void InitialMakeItem()
    {
        if (savedData.data.ReturnInitialInventory())
        {



            for (int i = 1; i < 28/*itemDatabase.instance.itemDB.Count*/; i++)
            {
                itemDatabase.instance.itemDB[i].itemCode = i;
                AddItem(itemDatabase.instance.itemDB[i]);
            }
            for (int j = 28; j < 46;j++)
            {
                itemDatabase.instance.itemDB[j].itemCode = j;
                AddItem(itemDatabase.instance.itemDB[j]);
                Debug.Log(itemDatabase.instance.itemDB[j].itemCode);
            }

            for (int k=46; k< 50; k++)
            {
                itemDatabase.instance.itemDB[k].itemCode = k;
                AddItem(itemDatabase.instance.itemDB[k]);
            }
           
            savedData.data.ChangeInitialInventory();
        }
       
        
    }

    public void UpLoadInvenToSavedData()
    {
        savedData.data.ClearInvenList();
        Debug.Log("inventory");
        for(int i = 0; i<PowerItems.Count; i++)
        {
            savedData.data.AddPowerInvenItems(PowerItems[i]);
        }
        for(int i = 0; i<DriveItems.Count; i++)
        {
            savedData.data.AddDriveInvenItems(DriveItems[i]);
        }
        for(int i = 0; i<SpecialItems.Count;i++)
        {
            savedData.data.AddSpeicalInvenItems(SpecialItems[i]);
        }
    }

    public void DownLoadInvenToSavedData()
    {
        //          itemDatabase.instance.itemDB[userData.includedPowerList[i].itemCode
        PowerItems.Clear();
        DriveItems.Clear();
        SpecialItems.Clear();
        onChangeItem.Invoke();
        //Debug.Log("---");
        List<item> _item1;
        List<item> _item2;
        List<item> _item3;
        _item1 = savedData.data.ReturnPowerInvenItems().ToList();
        Debug.Log(_item1.Count);
        //Debug.Log(_item.Count);
        for (int i = 0; i < _item1.Count; i++)
        {
            //items.Add(_item[i]); Debug.Log(items[i].itemName + "ssssssssssssssssssssssssss");
            //AddItem(_item1[i]);
            AddItem(itemDatabase.instance.itemDB[_item1[i].itemCode]);
            //Debug.Log("inven" + _item1[i].itemName);
            /*if (onChangeItem != null)
            {
                onChangeItem.Invoke();
                Debug.Log("invoke");
            }*/
        }
        //_item.Clear();
        _item2 = savedData.data.ReturnDriveInvenItems().ToList();
        for (int i = 0; i < _item2.Count; i++)
        {
            //Debug.Log(_item2[i].itemName);
            //AddItem(_item2[i]);
            AddItem(itemDatabase.instance.itemDB[_item2[i].itemCode]);
        }
        //_item.Clear();
        _item3 = savedData.data.ReturnSpecialInvenItems().ToList();
        for (int i = 0; i < _item3.Count; i++)
        {
            //AddItem(_item3[i]);
            AddItem(itemDatabase.instance.itemDB[_item3[i].itemCode]);
        }
        onChangeItem.Invoke();

    }
  
}
