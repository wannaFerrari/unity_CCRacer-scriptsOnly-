using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  public enum ItemType
    {
        None,
        PowerUp,
        DrivingUP,
        Special
    }
public enum parts
{
    None,
    Turbo,
    Air,
    IntakePipe,
    ThrottleBody,
    HicamShaft,
    Throttle,
    InterCooler,
    OilCoolder,
    Radiator,

    dDisk,
    dPad,
    dShoba,
    dSpring,
    dAngle,
    dTire,

    Wing,

    Bodykit,

    /*
    PowerSlots.Add(turbo);
    PowerSlots.Add(air);
    PowerSlots.Add(intakePipe);
    PowerSlots.Add(throttleBody);
    PowerSlots.Add(hicam);
    PowerSlots.Add(throttle);
    PowerSlots.Add(interCooler);
    PowerSlots.Add(oilCooler);
    PowerSlots.Add(Radiator);*/
}

[System.Serializable]
public class item
{
    public ItemType itemType;
    public parts parts;
    public string itemName;
    public Sprite itemImage;
    public float torque = 0f;
    public float weight = 0f;
    public float damper = 0f;
    public float spring = 0f;
    public float grip = 0f;
    public float angle = 0f;
    public float brakeVal = 0f;
    public float downForce = 0f;
    public string description = "";
    public string korParts = "";


    public bool Use()
    {
        bool isUsed = false;
        isUsed = true;
        return isUsed;
    }
}
