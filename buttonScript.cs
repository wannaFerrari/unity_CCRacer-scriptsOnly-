using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonScript : MonoBehaviour
{


    
    public inputManager inputM;
    public GameManager gm;

    private void Start()
    {
       inputM = this.GetComponent<GameManager>().car.GetComponent<inputManager>();
        gm = this.GetComponent<GameManager>();
    }



    public void ContinueClicked()
    {
        inputM.MenuStatus = false;
        //inputM.Menu.SetActive(false);
        gm.MenuCanvasClose();
    }

    public void MenuClicked()
    {
        //loadingSceneController.LoadScene("GarageScene");
        gm.ReturnToMainInGameClicked();

    }

    public void QuitClicked()
    {
        //Application.Quit();
        gm.QuitInGameClicked();
    }
}
