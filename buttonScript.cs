using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonScript : MonoBehaviour
{


    
    public inputManager inputM;
    

   
    public void ContinueClicked()
    {
        inputM.MenuStatus = false;
        inputM.Menu.SetActive(false);
    }

    public void MenuClicked()
    {
        loadingSceneController.LoadScene("GarageScene");

    }

    public void QuitClicked()
    {
        Application.Quit();
    }
}
