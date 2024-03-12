using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemDatabase : MonoBehaviour
{
    public static itemDatabase instance;
    public GameObject test1;
    private void Awake()
    {
        instance = this; 
    }
    public List<item> itemDB = new List<item>();

    public GameObject fieldItemPrefab;
    public Vector3[] pos;

    private void Start()
    {
        //Debug.Log(itemDB[0].itemName);
        test1.GetComponent<fieldItems>().SetItem(itemDB[0]);
        //Debug.Log(test1.item.itemName);
        for(int i = 0; i < itemDB.Count; i++)
        {
            itemDB[i].itemCode = i;
        }

    }
}
