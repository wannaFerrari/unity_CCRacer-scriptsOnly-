using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  public enum ItemType
    {
        PowerUp,
        DrivingUP,
        DressUp,
        Bodykit
    }
public enum parts
{
    pTurbo,
    pThrottle,
    pRadi,
    pBigbo,
    pAir,
    pOil,
    pInter,
    pPipe,
    pHicam,

    dDisk,
    dPad,
    dShoba,
    dSpring,
    dAngle,
    dTire,

    Wing,

    Bodykit,
}

[System.Serializable]
public class item
{
    public ItemType itemType;
    public parts parts;
    public string itemName;
    public Sprite itemImage;


    public bool Use()
    {
        return false;
    }
}
