using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fieldItems : MonoBehaviour
{

    public item item;
   // public SpriteRenderer image;
    public void SetItem(item _item)
    {
        item.itemName = _item.itemName;
        item.itemImage = _item.itemImage;
        item.itemType = _item.itemType;
        //Debug.Log(this.GetComponentInParent<Image>());
        // image.sprite = item.itemImage;
        //this.sprite = _item.itemImage;
        this.GetComponentInParent<Image>().sprite = _item.itemImage;
    }
    
    public item GetItem()
    {
        return item;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
