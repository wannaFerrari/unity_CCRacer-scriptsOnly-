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
        item.itemType = _item.itemType;
        item.parts = _item.parts;
        item.itemName = _item.itemName;
        item.itemImage = _item.itemImage;
        item.torque = _item.torque;
        item.weight = _item.weight;
        item.damper = _item.damper;
        item.spring = _item.spring;
        item.grip = _item.grip;
        item.angle = _item.angle;
        item.brakeVal = _item.brakeVal;
        item.downForce= _item.downForce;
        item.description= _item.description;
        item.korParts= _item.korParts;
 
        //Debug.Log(this.GetComponentInParent<Image>());
        // image.sprite = item.itemImage;
        //this.sprite = _item.itemImage;
        //this.GetComponentInParent<Image>().sprite = _item.itemImage;
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
