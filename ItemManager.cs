using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;
using TMPro;
//using UnityEngine.UIElements;
//using static System.Net.Mime.MediaTypeNames;

public class ItemManager : MonoBehaviour
{
    inventory inven;
    public item item;
    [Header("Panels")]
    public GameObject PowerPanel;
    public GameObject DrivingPanel;
    public GameObject SpecialPanel;

    public GameObject TuningCanvas;

    [Header("Variables")]
    private int ActiveMenuTab = 0;

    private float itemTorque = 0f;
    private float itemWeight = 0f;

    private float grip;
    private float angle;
    [Header("PowerSlot")]
    /*public GameObject turbo;
    public GameObject air;
    public GameObject intakePipe;
    public GameObject throttleBody;
    public GameObject hicam;
    public GameObject throttle;
    public GameObject interCooler;
    public GameObject oilCooler;
    public GameObject Radiator;*/
    public List<GameObject> PowerSlots = new List<GameObject>();

    [Header("DrivingSlot")]
    /*public GameObject spring;
    public GameObject shoba;
    public GameObject steerAngle;
    public GameObject pad;
    public GameObject disk;
    public GameObject tire;*/
    public List<GameObject> DrivingSlots = new List<GameObject>();

    [Header("SpeicialSlot")]
    public List<GameObject> SpecialSlots = new List<GameObject>();

    [Header("Bodykit")]
    public GameObject body;

    [Header("ToolTip")]
    public GameObject powerTooltip;
    public TextMeshProUGUI powerName;
    public TextMeshProUGUI powerValue;
    public TextMeshProUGUI powerDes;

    [Header("TuningValueInfo")]
    public TextMeshProUGUI torqueV;
    public TextMeshProUGUI brakeV;
    public TextMeshProUGUI damperV;
    public TextMeshProUGUI susV;
    public TextMeshProUGUI tireV;
    public TextMeshProUGUI angleV;
    public TextMeshProUGUI weightV;
    public TextMeshProUGUI speedV;

    [Header("Upside Buttons and TMP")]
    public Button poBtn;
    public Button drBtn;
    public Button spBtn;
    public TextMeshProUGUI poTmp;
    public TextMeshProUGUI drTmp;
    public TextMeshProUGUI spTmp;

    public Sprite img;
   
    public bool CheckToInclude(item _item)
    {
        //ItemType none;
        //none = ItemType.None;
        //ItemType po;
        //po = ItemType.PowerUp;
        //Debug.Log((int)_item.itemType);
        //Debug.Log(_item.itemType);
        //Debug.Log((int)po);
        //Debug.Log((int)_item.parts);
        //if ((int)po == (int)_item.itemType)
        if(_item.itemType == ItemType.PowerUp)
        {
            //Debug.Log((int)_item.parts);
            /*
            for(int i = 0; i < PowerSlots.Count; i++)
            {
                if((int)_item.itemType == i + 1)
                {
                    PowerSlots[i].GetComponent<fieldItems>().SetItem(_item);
                    PowerSlots[i].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = _item.itemImage;
                }
            }*/
            //if((int)PowerSlots[(int)_item.parts - 1].GetComponent<fieldItems>().item.itemType == (int)none)
            if (PowerSlots[(int)_item.parts -1].GetComponent<fieldItems>().item.itemType == ItemType.None)
            {
                PowerSlots[(int)_item.parts - 1].GetComponent<fieldItems>().SetItem(_item);
                PowerSlots[(int)_item.parts - 1].transform.GetChild(0).GetComponent<Image>().sprite = _item.itemImage;
                PowerSlots[(int)_item.parts - 1].GetComponent<Button>().interactable = true;
                UpdateIncludedTunings(_item, (int)_item.parts - 1);
               // UpdatePowerTunings(_item, (int)_item.parts - 1, _item.torque, _item.weight);
                return true;
            }
            else
            {
                return false;
            }
           // PowerSlots[(int)_item.parts - 1].GetComponent<fieldItems>().SetItem(_item);
           // PowerSlots[(int)_item.parts - 1].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = _item.itemImage;
           // UpdateTunings(_item.torque, _item.weight);
        }
        else if (_item.itemType == ItemType.DrivingUP)
        {
           // Debug.Log(_item.itemName);
            //Debug.Log(_item.parts + "   ---   " + (int)_item.parts);
            if (DrivingSlots[(int)_item.parts - 10].GetComponent<fieldItems>().item.itemType == ItemType.None)
            {
                DrivingSlots[(int)_item.parts - 10].GetComponent<fieldItems>().SetItem(_item);
                DrivingSlots[(int)_item.parts - 10].transform.GetChild(0).GetComponent<Image>().sprite = _item.itemImage;
                DrivingSlots[(int)_item.parts - 10].GetComponent<Button>().interactable = true;
               // Debug.Log((int)_item.parts - 10);
                UpdateIncludedTunings(_item, (int)_item.parts - 10);
                return true;
            }
        }
        else if (_item.itemType == ItemType.Special)
        {
            // Debug.Log(_item.itemName);
            //Debug.Log(_item.parts + "   ---   " + (int)_item.parts);
            if (SpecialSlots[(int)_item.parts - 16].GetComponent<fieldItems>().item.itemType == ItemType.None)
            {
                SpecialSlots[(int)_item.parts - 16].GetComponent<fieldItems>().SetItem(_item);
                SpecialSlots[(int)_item.parts - 16].transform.GetChild(0).GetComponent<Image>().sprite = _item.itemImage;
                SpecialSlots[(int)_item.parts - 16].GetComponent<Button>().interactable = true;
                // Debug.Log((int)_item.parts - 10);
                UpdateIncludedTunings(_item, (int)_item.parts - 16);
                return true;
            }
        }
        return false;
        // turbo.GetComponent<item>().itemType = _item.itemType;
       
        //turbo.GetComponent<fieldItems>().SetItem(_item);
        //turbo.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = _item.itemImage;
        //return true;
        //return false;
    }
    /*
    private void MakePowerSlots()
    {
        PowerSlots.Add(turbo);
        PowerSlots.Add(air);
        PowerSlots.Add(intakePipe);
        PowerSlots.Add(throttleBody);
        PowerSlots.Add(hicam);
        PowerSlots.Add(throttle);
        PowerSlots.Add(interCooler);
        PowerSlots.Add(oilCooler);
        PowerSlots.Add(Radiator);
    }*/
    public void UpdateIncludedTunings(item _item, int i)
    {
        /*
        item it = new item();
        
        it.itemType = _item.itemType;
        it.parts = _item.parts;
        it.itemName = _item.itemName;
        it.itemImage = _item.itemImage;
        it.torque = _item.torque;
        it.weight = _item.weight;
        it.damper = _item.damper;
        it.spring = _item.spring;
        it.grip = _item.grip;
        it.angle = _item.angle;
        it.breakVal= _item.breakVal;
        it.description= _item.description;
        it.korParts= _item.korParts;
        */
        //CopyItem(_item, it);
        //itemTorque += torque;
        //itemWeight+= weight;
        savedData.data.savedTorque += _item.torque;//torque;
        savedData.data.savedWeight += _item.weight;//weight;
        savedData.data.savedDamper+= _item.damper;
        savedData.data.savedSpring+= _item.spring;
        savedData.data.savedGrip+= _item.grip;
        savedData.data.savedAngle+= _item.angle;
        savedData.data.savedBrake += _item.brakeVal;
        savedData.data.savedDownforce += _item.downForce;
        /*public float damper = 0f;
    public float spring = 0f;
    public float grip = 0f;
    public float angle = 0f;
    public float breakVal = 0f;*/
    //savedData.data.turbo = _item;
        if(_item.itemType == ItemType.PowerUp) {
            savedData.data.includedPowerList[i] = CopyItem(_item);

        }
        else if( _item.itemType == ItemType.DrivingUP)
        {
            savedData.data.includedDriveList[i] = CopyItem(_item);

        }
        else if( _item.itemType == ItemType.Special)
        {
            savedData.data.includedSpecialList[i] = CopyItem(_item);

        }
        //Debug.Log(savedData.data.savedTorque);
        //Debug.Log(savedData.data.savedWeight);
        //Debug.Log(savedData.data.includedList[i].itemName);
        //Debug.Log(savedData.data.includedList[i].itemType);
        UpdateValueInfo();
    }

    public void UpdateEjectedTunings(item _item, item none, int i)
    {
        savedData.data.savedTorque -= _item.torque;//torque;
        savedData.data.savedWeight -= _item.weight;//weight;
        savedData.data.savedDamper -= _item.damper;
        savedData.data.savedSpring -= _item.spring;
        savedData.data.savedGrip -= _item.grip;
        savedData.data.savedAngle -= _item.angle;
        savedData.data.savedBrake -= _item.brakeVal;
        savedData.data.savedDownforce-= _item.downForce;
        if(_item.itemType == ItemType.PowerUp)
        {
            savedData.data.includedPowerList[i] = CopyItem(none);

        }
        else if(_item.itemType == ItemType.DrivingUP)
        {
            savedData.data.includedDriveList[i] = CopyItem(none);

        }
        else if(_item.itemType == ItemType.Special)
        {
            savedData.data.includedSpecialList[i] = CopyItem(none);

        }
        UpdateValueInfo();
    }
    public item CopyItem(item from)
    {
        item to = new item();
        to.itemType = from.itemType;
        to.parts = from.parts;
        to.itemName = from.itemName;
        to.itemImage = from.itemImage;
        to.torque = from.torque;
        to.weight = from.weight;
        to.damper = from.damper;
        to.spring = from.spring;
        to.grip = from.grip;
        to.angle = from.angle;
        to.brakeVal = from.brakeVal;
        to.downForce= from.downForce;
        to.description = from.description;
        to.korParts = from.korParts;
        to.itemCode = from.itemCode;

        return to;
    }

    public void RefreshIncludedSlots()
    {
        for(int i = 0; i<PowerSlots.Count; i++)
        {
            if (savedData.data.includedPowerList[i].itemType != ItemType.None)
            {
                Debug.Log(savedData.data.includedPowerList[i].itemType);
                PowerSlots[i].GetComponent<fieldItems>().SetItem(savedData.data.includedPowerList[i]);
                PowerSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = savedData.data.includedPowerList[i].itemImage;
            }
        }
        for(int i = 0; i < DrivingSlots.Count; i++)
        {
            if (savedData.data.includedDriveList[i].itemType != ItemType.None)
            {
                Debug.Log(savedData.data.includedDriveList[i].itemType);
                DrivingSlots[i].GetComponent<fieldItems>().SetItem(savedData.data.includedDriveList[i]);
                DrivingSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = savedData.data.includedDriveList[i].itemImage;
            }
        }
        for (int i = 0; i < SpecialSlots.Count; i++)
        {
            if (savedData.data.includedSpecialList[i].itemType != ItemType.None)
            {
                Debug.Log(savedData.data.includedSpecialList[i].itemType);
                SpecialSlots[i].GetComponent<fieldItems>().SetItem(savedData.data.includedSpecialList[i]);
                SpecialSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = savedData.data.includedSpecialList[i].itemImage;
            }
        }

        UpdateValueInfo();
    }
    
    public void UpdateValueInfo()
    {
        torqueV.text = (savedData.data.savedTorque != 0) ? "??? (" + savedData.data.savedTorque.ToString("+#;-#;0") + ")" : "???";
        brakeV.text = (savedData.data.savedBrake != 0) ? "??? (" + savedData.data.savedBrake.ToString("+#;-#;0") + ")" : "???";
        damperV.text = (savedData.data.savedDamper != 0) ? "??? (" + savedData.data.savedDamper.ToString("+#;-#;0") + ")" : "???";
        susV.text = (savedData.data.savedSpring != 0) ? "??? (" + savedData.data.savedSpring.ToString("+#;-#;0") + ")" : "???";
        tireV.text = (savedData.data.savedGrip != 0) ? "??? (" + savedData.data.savedGrip.ToString("+#;-#;0") + ")" : "???";
        angleV.text = (savedData.data.savedAngle != 0) ? "??? (" + savedData.data.savedAngle.ToString("+#;-#;0") + ")" : "???";
        weightV.text = (savedData.data.savedWeight != 0) ? "??? (" + savedData.data.savedWeight.ToString("+#;-#;0") + ")" : "???";
        //speedV.text = (savedData.data.savedTorque != 0) ? "??? (" + savedData.data.savedTorque.ToString("+#;-#;0") + ")" : "???";


    }

    public void ShowToolTip(item _item)
    {
        //Debug.Log(_item.description);
        //Debug.Log("des");
        float mx = Input.mousePosition.x + 200f;
        float my = Input.mousePosition.y - 200f;

        if (mx > 1689)
        {
            mx = 1689;
        }
        if (mx < 231)
        {
            mx = 231;
        }

        if (_item.itemType == ItemType.PowerUp)
        {
            
            powerTooltip.transform.position = new Vector3(mx, my);
            powerTooltip.SetActive(true);
            powerName.text = _item.itemName.ToString();
            powerValue.text = "장착위치: " + _item.korParts.ToString() + "\n출력 " + _item.torque.ToString("+#;-#;0") + "\n무게 " + _item.weight.ToString("+#;-#;0");
            powerDes.text = _item.description.ToString();
            
            //Debug.Log(powerTooltip.transform.position);

        }
        else if (_item.itemType == ItemType.DrivingUP)
        {
           
            if ((_item.parts == parts.dDisk) || (_item.parts == parts.dPad))
            {
                powerTooltip.transform.position = new Vector3(mx, my);
                powerTooltip.SetActive(true);
                powerName.text = _item.itemName.ToString();
                powerValue.text = "장착위치: " + _item.korParts.ToString() + "\n브레이크 " + _item.brakeVal.ToString("+#;-#;0");
                powerDes.text = _item.description.ToString();
            }
            else if(_item.parts == parts.dShoba)
            {
                powerTooltip.transform.position = new Vector3(mx, my);
                powerTooltip.SetActive(true);
                powerName.text = _item.itemName.ToString();
                powerValue.text = "장착위치: " + _item.korParts.ToString() + "\n뎀퍼 " + _item.damper.ToString("+#;-#;0");
                powerDes.text = _item.description.ToString();
            }
            else if(_item.parts == parts.dSpring)
            {
                powerTooltip.transform.position = new Vector3(mx, my);
                powerTooltip.SetActive(true);
                powerName.text = _item.itemName.ToString();
                powerValue.text = "장착위치: " + _item.korParts.ToString() + "\n서스펜션 길이 " + _item.spring.ToString("+#;-#;0")+ "\n뎀퍼 " + _item.damper.ToString(" +#;-#;0");
                powerDes.text = _item.description.ToString();
            }
            else if(_item.parts == parts.dTire)
            {
                powerTooltip.transform.position = new Vector3(mx, my);
                powerTooltip.SetActive(true);
                powerName.text = _item.itemName.ToString();
                powerValue.text = "장착위치: " + _item.korParts.ToString() + "\n브레이크 " + _item.brakeVal.ToString("+#;-#;0") + "\n접지력 " + _item.grip.ToString(" +#;-#;0");
                powerDes.text = _item.description.ToString();
            }
            else if (_item.parts == parts.dAngle)
            {
                powerTooltip.transform.position = new Vector3(mx, my);
                powerTooltip.SetActive(true);
                powerName.text = _item.itemName.ToString();
                powerValue.text = "장착위치: " + _item.korParts.ToString() + "\n조향 각도 " + _item.angle.ToString("+#;-#;0");
                powerDes.text = _item.description.ToString();
            }
            //Debug.Log(powerTooltip.transform.position);

        }
        else if(_item.itemType == ItemType.Special)
        {
           
            if (_item.parts == parts.Wing)
            {
                powerTooltip.transform.position = new Vector3(mx, my);
                powerTooltip.SetActive(true);
                powerName.text = _item.itemName.ToString();
                powerValue.text = "장착위치: " + _item.korParts.ToString() + "\n무게 " + _item.weight.ToString("+#;-#;0") + "\n다운포스 " + _item.downForce.ToString(" +#;-#;0");
                powerDes.text = _item.description.ToString();
            }
            else if(_item.parts ==parts.Bodykit)
            {
                powerTooltip.transform.position = new Vector3(mx, my);
                powerTooltip.SetActive(true);
                powerName.text = _item.itemName.ToString();
                powerValue.text = "장착위치: " + _item.korParts.ToString() + "\n출력 " + _item.torque.ToString("+#;-#;0") + "\n무게 " + _item.weight.ToString(" +#;-#;0");
                powerDes.text = _item.description.ToString();
            }
        }
    }

    public void CloseToolTip()
    {
        powerTooltip.SetActive(false);
    }
    public void RequestUpLoadToSavedData()
    {
        this.GetComponent<inventory>().UpLoadInvenToSavedData();
    }

    public void RequestDownLoadFromSavedData()
    {
        this.GetComponent<inventory>().DownLoadInvenToSavedData();
    }

    public void RequestMakeItems()
    {
        this.GetComponent<inventory>().InitialMakeItem();
    }

    public void PowerBtnClicked()
    {
        DrivingPanel.SetActive(false);
        SpecialPanel.SetActive(false);
        PowerPanel.SetActive(true);

        drBtn.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        spBtn.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        poBtn.GetComponent<Image>().color = new Color(255, 255, 255, 255);

        drTmp.color = new Color(207,207,207,255);
        spTmp.color = new Color(207, 207, 207, 255);
        poTmp.color = new Color(0, 0, 0, 255);
    }

    public void DriveBtnClicked()
    {
        SpecialPanel.SetActive(false);
        PowerPanel.SetActive(false);
        DrivingPanel.SetActive(true);

        drBtn.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        spBtn.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        poBtn.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        poTmp.color = new Color(207, 207, 207, 255);
        spTmp.color = new Color(207, 207, 207, 255);
        drTmp.color = new Color(0, 0, 0, 255);
    }

    public void SpecialBtnClicked()
    {
        DrivingPanel.SetActive(false);
        PowerPanel.SetActive(false);
        SpecialPanel.SetActive(true);

        spBtn.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        drBtn.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        poBtn.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        poTmp.color = new Color(207, 207, 207, 255);
        drTmp.color = new Color(207, 207, 207, 255);
        spTmp.color = new Color(0, 0, 0, 255);

    }
    // Update is called once per frame
    void Start()
    {
        //inven = this.GetComponent<inventory>();
        /*
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("CCCCCCCCCCCCCCCC");
            test222();
        }*/

        RefreshIncludedSlots();
        //TuningCanvas.SetActive(false);
        Invoke("offtun", 0.1f);
        /*
        for (int i = 0;i<PowerSlots.Count;i++)
        {
            Debug.Log(savedData.data.savedTorque);
            /*
            if (savedData.data.includedList[i]== null)
            {
                Debug.Log("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
            }*/
        // }*/
    }
    void offtun()
    {
        TuningCanvas.SetActive(false);
        PowerPanel.SetActive(false);
        DrivingPanel.SetActive(false);
        SpecialPanel.SetActive(false);
    }

}
