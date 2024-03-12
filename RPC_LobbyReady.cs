using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class RPC_LobbyReady : MonoBehaviourPunCallbacks
{
    public LobbyManager lobbyManager;
    public GameObject loadingPanel;
    //public GameObject loadingRainPanel;
    public GameObject mainPanel;
    public TextMeshProUGUI leftName;
    public TextMeshProUGUI rightName;
    public UnityEngine.UI.Image loadingBackgroundPanel;
    public Sprite[] trackImgs;
    // Start is called before the first frame update
    void Start()
    {
        lobbyManager = GameObject.FindGameObjectWithTag("lobbyManager").GetComponent<LobbyManager>();
    }

    [PunRPC]
    public void PopUpLoadingPanel(string lName, string rName, int trackNum)
    {
        leftName.text = lName;
        rightName.text = rName;
        mainPanel.SetActive(false);
        loadingBackgroundPanel.sprite = trackImgs[trackNum];
        loadingPanel.SetActive(true);
    }

    [PunRPC]
    public void JoinedUserClickedReady()
    {
        lobbyManager.JoinedUserClickedReady();
    }

    [PunRPC]
    public void JoinedUserCanceledReady()
    {
        lobbyManager.JoinedUserCanceledReady();
    }

    [PunRPC]
    public void SetIsRainingOrNot(bool isRaining)
    {
        savedData.data.isRaining = isRaining;
    }

    

}
