using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{

    public static inventory instance;

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<item>items = new List<item>();

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
        
    }

    private void Start()
    {
        SlotCnt = 27;
    }
    public bool AddItem(item _item)
    {
        items.Add(_item);
        if(onChangeItem!= null)
        onChangeItem.Invoke();
        return true;
    }

    private void TestItem()
    {
        //fieldItems
        if (Input.GetKeyDown(KeyCode.B))
        {
            //Debug.Log("BBBBBBBBBBBBBBBBBBBB");
           // fieldItems a = GameObject.FindGameObjectWithTag("Item").GetComponent<fieldItems>();
           // Debug.Log(a);
            //Debug.Log("aaaaaaaaaaaaaaaaaa");
           // Debug.Log(AddItem(a.GetItem()));
            for (int i = 0; i < itemDatabase.instance.itemDB.Count; i++) 
            {
                AddItem(itemDatabase.instance.itemDB[i]);
            }
        }

        // Update is called once per frame
       
    }
    private void Update()
    {
        TestItem();
    }
}
