using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorSliderControll : MonoBehaviour
{
    [Header("Slider")]
    public Slider redS;
    public Slider greenS;
    public Slider blueS;

    [Header("Materials")]
    public Material supra;
    public Material porsche;
    public Material chiron;

    [Header("ValueText")]
    public Text redValueText;
    public Text greenValueText;
    public Text blueValueText;

    public int currentCar;

    private float savedRed;
    private float savedGreen;
    private float savedBlue;
    //
    private float currentRed;
    private float currentGreen;
    private float currentBlue;
    
    //

    private float resetSupraRed = 238f;
    private float resetSupraGreen = 102f;
    private float resetSupraBlue = 255f;

    private float resetPorscheRed = 178f;
    private float resetPorscheGreen = 255f;
    private float resetPorscheBlue = 0f;

    private float resetChironRed = 37f;
    private float resetChironGreen = 93f;
    private float resetChironBlue = 200f;
    //
    private bool isCarChanging = false;

    private bool menuPageLoaded = false;



    // Start is called before the first frame update
    void Awake()
    {
        //currentCar = savedData.data.currentCar;
        //GetColor();
        //SetSlider();
    }

    // Update is called once per frame
    void Update()
    {
        if (menuPageLoaded)
        {
            redValueText.text = ((int)redS.value).ToString();
            greenValueText.text = ((int)greenS.value).ToString();
            blueValueText.text = ((int)blueS.value).ToString();
            if (!isCarChanging) SetColor(redS.value, greenS.value, blueS.value);
        }
        /*
        redValueText.text = ((int)redS.value).ToString();
        greenValueText.text = ((int)greenS.value).ToString();
        blueValueText.text = ((int)blueS.value).ToString();
        if(!isCarChanging)SetColor(redS.value, greenS.value, blueS.value);
        */
    }

  
    public void SetColorToInitialColor()
    {
        supra.color = new Color(resetSupraRed / 255f, resetSupraGreen / 255f, resetSupraBlue / 255f);
        porsche.color = new Color(resetPorscheRed / 255f, resetPorscheGreen / 255f, resetPorscheBlue / 255f);
        chiron.color = new Color(resetChironRed / 255f, resetChironGreen / 255f, resetChironBlue / 255f);
    }

    public void GetColor()
    {
        currentCar = savedData.data.currentCar;
        if (currentCar == 0)
        {
            savedRed = savedData.data.supColor.r * 255; 
            savedGreen = savedData.data.supColor.g * 255;
            savedBlue = savedData.data.supColor.b * 255;
        }
        else if (currentCar == 1)
        {
            savedRed = savedData.data.porColor.r * 255;
            savedGreen = savedData.data.porColor.g * 255;
            savedBlue = savedData.data.porColor.b * 255;
        }
        else if (currentCar == 2)
        {
            savedRed = savedData.data.chiColor.r * 255;
            savedGreen = savedData.data.chiColor.g * 255;
            savedBlue = savedData.data.chiColor.b * 255;
        }
        /*
        if (currentCar == 0)
        {
            savedRed = supra.color.r * 255;
            savedGreen = supra.color.g * 255;
            savedBlue = supra.color.b * 255;
        }
        else if (currentCar == 1)
        {
            savedRed = porsche.color.r * 255;
            savedGreen = porsche.color.g * 255;
            savedBlue = porsche.color.b * 255;
        }
        else if (currentCar == 2)
        {
            savedRed = chiron.color.r * 255;
            savedGreen = chiron.color.g * 255;
            savedBlue = chiron.color.b * 255;
        }*/
    }

    private void SetSlider()
    {
        redS.value = savedRed;
        greenS.value = savedGreen;
        blueS.value = savedBlue;
    }

    public void SetSlider(float r, float g, float b)
    {
        redS.value = r * 255f;
        greenS.value = g * 255f;
        blueS.value = b * 255f;
    }

    public void SetColor(float r, float g, float b)
    {
        currentCar = savedData.data.currentCar;
        if (currentCar == 0)
        {
            supra.color = new Color(r / 255f, g / 255f, b / 255f);
            savedData.data.supColor = supra.color;
        }
        else if (currentCar == 1)
        {
            porsche.color = new Color(r / 255f, g / 255f, b / 255f);
            savedData.data.porColor = porsche.color;
        }
        else if (currentCar == 2)
        {
            chiron.color = new Color(r / 255f, g / 255f, b / 255f);
            savedData.data.chiColor = chiron.color;
        }
    }

    public void RecoverBackColor()
    {
        SetSlider();
        //Debug.Log(savedData.data.currentCar);
    }
    
    public void ResetColorToDefault()
    {
        currentCar = savedData.data.currentCar;
        //Debug.Log(savedData.data.currentCar);
        if (currentCar == 0)
        {
            redS.value = resetSupraRed;
            greenS.value = resetSupraGreen;
            blueS.value = resetSupraBlue;
        }
        else if (currentCar == 1)
        {
            redS.value = resetPorscheRed;
            greenS.value = resetPorscheGreen;
            blueS.value = resetPorscheBlue;
        }
        else if (currentCar == 2)
        {
            redS.value = resetChironRed;
            greenS.value = resetChironGreen;
            blueS.value = resetChironBlue;
        }
    }
    public void carChanged()
    {
        currentCar = savedData.data.currentCar;
        if (currentCar == 0)
        {/*
            redS.value = supra.color.r * 255;
            greenS.value = supra.color.g * 255;
            blueS.value = supra.color.b * 255;
            */
            redS.value = savedData.data.supColor.r * 255;
            greenS.value = savedData.data.supColor.g * 255;
            blueS.value = savedData.data.supColor.b * 255;
        }
        else if (currentCar == 1)
        {/*
            redS.value = porsche.color.r * 255;
            greenS.value = porsche.color.g * 255;
            blueS.value = porsche.color.b * 255;
            */
            redS.value = savedData.data.porColor.r * 255;
            greenS.value = savedData.data.porColor.g * 255;
            blueS.value = savedData.data.porColor.b * 255;
        }
        else if (currentCar == 2)
        {/*
            redS.value = chiron.color.r * 255;
            greenS.value = chiron.color.g * 255;
            blueS.value = chiron.color.b * 255;
            */
            redS.value = savedData.data.chiColor.r * 255;
            greenS.value = savedData.data.chiColor.g * 255;
            blueS.value = savedData.data.chiColor.b * 255;
        }
    }
    public void carChanging()
    {
        isCarChanging= true;
    }

    public void MenuLoaded()
    {
        menuPageLoaded = true;
    }
}
