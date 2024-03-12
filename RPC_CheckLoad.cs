using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SocialPlatforms.Impl;


public class RPC_CheckLoad : MonoBehaviourPunCallbacks
{
    public int loadedUser = 0;
    public int colorFinishedUser = 0;
    public string timer = "00.00.000";
    public float totalSeconds = 0;

    public int masterUserScore = 0;
    public int otherUserScore = 0;
    public GameManager manager;
    public Material sup1;
    public Material sup2;
    public Material por1;
    public Material por2;
    public Material chi1;
    public Material chi2;

    // Start is called before the first frame update
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
    }
    [PunRPC]
    public void UserLoaded()
    {
        loadedUser++;
    }

    [PunRPC]
    public void SetRPC_Timer(string time)
    {
        timer = time;
    }

    [PunRPC]
    public void SetRPC_TotalSeconds(float totalTime)
    {
        totalSeconds = totalTime;
    }


    public string GetRPC_Timer()
    {
        return timer;
    }

    public float GetRPC_TotalSeconds()
    {
        return totalSeconds;
    }


    [PunRPC]
    public void UpdateScore(bool isMaster, int score)
    {
        if(isMaster)
        {
            masterUserScore = score;
            manager.CurrentPosition(masterUserScore, otherUserScore);
        }
        else
        {
            otherUserScore= score;
            manager.CurrentPosition(masterUserScore, otherUserScore);
        }
    }
    public bool ReturnCurrentPosition(bool isMaster)
    {
        if (isMaster)
        {
           
            if (masterUserScore > otherUserScore)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
           
            if (otherUserScore > masterUserScore)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    [PunRPC]
    public void AddOrEjectWings(int actorNumber, int carSelected, float wingsOn)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i< objects.Length; i++)
        {
            if (objects[i].GetComponent<PhotonView>().OwnerActorNr == actorNumber)
            {
                if (wingsOn == 0f)
                {
                    if (carSelected == 2)
                    {
                        Debug.Log(actorNumber + "  "+ carSelected+"  "+ wingsOn);
                        objects[i].transform.Find("spoiler").gameObject.SetActive(false);
                        objects[i].transform.Find("closedSpoiler").gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.Log(actorNumber + "  " + carSelected + "  " + wingsOn);
                        objects[i].transform.Find("spoiler").gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (carSelected == 2)
                    {
                        Debug.Log(actorNumber + "  " + carSelected + "  " + wingsOn);
                        objects[i].transform.Find("spoiler").gameObject.SetActive(true);
                        objects[i].transform.Find("closedSpoiler").gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.Log(actorNumber + "  " + carSelected + "  " + wingsOn);
                         objects[i].transform.Find("spoiler").gameObject.SetActive(true);
                    }
                }
            }
        }
       // PhotonNetwork.CurrentRoom.GetPlayer
    }

    [PunRPC]
    public void SetColorToServer(int actorNumber1, int carSelected1, float colorR1, float colorG1, float colorB1 , 
        int actorNumber2, int carSelected2, float colorR2, float colorG2, float colorB2)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(objects.Length + "---- objects.length");

        GameObject[] findSu = GameObject.FindGameObjectsWithTag("supraColor");
        List<GameObject> su = new List<GameObject>();
        List<GameObject> su2 = new List<GameObject>();
        Debug.Log(findSu.Length);

        for (int j = 0; j < findSu.Length; j++)
        {
            if (findSu[j].gameObject.transform.root.transform.GetComponentInChildren<PhotonView>().GetComponent<PhotonView>().OwnerActorNr == actorNumber1)
            {
                su.Add(findSu[j]);
                Debug.Log(findSu[j] +"-------------" + j);
            }
            else if(findSu[j].gameObject.transform.root.transform.GetComponentInChildren<PhotonView>().GetComponent<PhotonView>().OwnerActorNr == actorNumber2)
            {

                su2.Add(findSu[j]);
                Debug.Log(findSu[j] + "-------------" + j);
            }
        }
        Debug.Log(su.Count + " --su count--" + su2.Count);
        


        GameObject[] findPo = GameObject.FindGameObjectsWithTag("porColor");
        List<GameObject> po = new List<GameObject>();
        List<GameObject> po2 = new List<GameObject>();
        for (int j = 0; j < findPo.Length ; j++)
        {
            if (findPo[j].gameObject.transform.root.transform.GetComponentInChildren<PhotonView>().GetComponent<PhotonView>().OwnerActorNr == actorNumber1)
            {
                po.Add(findPo[j]);
            }
            else if (findPo[j].gameObject.transform.root.transform.GetComponentInChildren<PhotonView>().GetComponent<PhotonView>().OwnerActorNr == actorNumber2)
            {
                po2.Add(findPo[j]);
            }
        }



        GameObject[] findCh = GameObject.FindGameObjectsWithTag("chiColor");
        List<GameObject> ch = new List<GameObject>();
        List<GameObject> ch2 = new List<GameObject>();
        for (int j = 0; j < findCh.Length ; j++)
        {
            if (findCh[j].gameObject.transform.root.transform.GetComponentInChildren<PhotonView>().GetComponent<PhotonView>().OwnerActorNr == actorNumber1)
            {
                ch.Add(findCh[j]);
            }
            else if (findCh[j].gameObject.transform.root.transform.GetComponentInChildren<PhotonView>().GetComponent<PhotonView>().OwnerActorNr == actorNumber2)
            {
                ch2.Add(findCh[j]);
            }
        }

        for (int i = 0; i < objects.Length; i++)
        {
            Debug.Log(objects[i].GetComponent<PhotonView>().OwnerActorNr + " objects[i] number" + i);
            if (objects[i].GetComponent<PhotonView>().OwnerActorNr == actorNumber1)
            {
                if (carSelected1 == 0)
                {
                    sup1.color = new Color(colorR1/255f, colorG1/255f, colorB1 / 255f);
                    /*
                    GameObject[] findSu = GameObject.FindGameObjectsWithTag("supraColor");
                    List<GameObject> su = new List<GameObject>();
                    
                    for (int j = 0; j < findSu.Length-1; j++)
                    {
                        if (findSu[j].gameObject.GetComponent<PhotonView>().OwnerActorNr == actorNumber1)
                        {
                            su.Add(findSu[j]);
                        }
                    }*/


                    Material[] m0 = su[0].GetComponent<MeshRenderer>().materials;
                    m0[2] = sup1;
                    su[0].GetComponent<MeshRenderer>().materials = m0;

                    Material[] m1 = su[1].GetComponent<MeshRenderer>().materials;
                    m1[3] = sup1;
                    su[1].GetComponent<MeshRenderer>().materials = m1;

                    Material[] m2 = su[2].GetComponent<MeshRenderer>().materials;
                    m2[3] = sup1;
                    su[2].GetComponent<MeshRenderer>().materials = m2;

                    Material[] m3 = su[3].GetComponent<MeshRenderer>().materials;
                    m3[3] = sup1;
                    su[3].GetComponent<MeshRenderer>().materials = m3;

                    Material[] m4 = su[4].GetComponent<MeshRenderer>().materials;
                    m4[3] = sup1;
                    su[4].GetComponent<MeshRenderer>().materials = m4;

                    Material[] m5 = su[5].GetComponent<MeshRenderer>().materials;
                    m5[9] = sup1;
                    su[5].GetComponent<MeshRenderer>().materials = m5;

                    Material[] m6 = su[6].GetComponent<MeshRenderer>().materials;
                    m6[8] = sup1;
                    su[6].GetComponent<MeshRenderer>().materials = m6;

                    Material[] m7 = su[7].GetComponent<MeshRenderer>().materials;
                    m7[1] = sup1;
                    su[7].GetComponent<MeshRenderer>().materials = m7;

                }
                else if (carSelected1 == 1)
                {
                    por1.color = new Color(colorR1 / 255f, colorG1 / 255f, colorB1 / 255f); 
                    /*
                    GameObject[] findPo = GameObject.FindGameObjectsWithTag("porColor");
                    List<GameObject> po = new List<GameObject>();
                    List<GameObject> po2 = new List<GameObject>();
                    for (int j = 0; j < findSu.Length - 1; j++)
                    {
                        if (findSu[j].gameObject.GetComponent<PhotonView>().OwnerActorNr == actorNumber1)
                        {
                            po.Add(findSu[j]);
                        }
                        else if (findSu[j].gameObject.GetComponent<PhotonView>().OwnerActorNr == actorNumber2)
                        {
                            po2.Add(findSu[j]);
                        }
                    }
                    */

                    Material[] m0 = po[0].GetComponent<MeshRenderer>().materials;
                    m0[0] = por1;
                    po[0].GetComponent<MeshRenderer>().materials = m0;
                }
                else if (carSelected1 == 2)
                {
                    chi1.color = new Color(colorR1 / 255f, colorG1 / 255f, colorB1 / 255f);
                   // GameObject[] ch = GameObject.FindGameObjectsWithTag("chiColor");

                    Material[] m0 = ch[0].GetComponent<MeshRenderer>().materials;
                    m0[0] = chi1;
                    ch[0].GetComponent<MeshRenderer>().materials = m0;

                    Material[] m1 = ch[1].GetComponent<MeshRenderer>().materials;
                    m1[0] = chi1;
                    ch[1].GetComponent<MeshRenderer>().materials = m1;

                    Material[] m2 = ch[2].GetComponent<MeshRenderer>().materials;
                    m2[0] = chi1;
                    ch[2].GetComponent<MeshRenderer>().materials = m2;

                    Material[] m3 = ch[3].GetComponent<MeshRenderer>().materials;
                    m3[0] = chi1;
                    ch[3].GetComponent<MeshRenderer>().materials = m3;

                    Material[] m4 = ch[4].GetComponent<MeshRenderer>().materials;
                    m4[0] = chi1;
                    ch[4].GetComponent<MeshRenderer>().materials = m4;

                    Material[] m5 = ch[5].GetComponent<MeshRenderer>().materials;
                    m5[0] = chi1;
                    ch[5].GetComponent<MeshRenderer>().materials = m5;
                }
            }


            if(objects[i].GetComponent<PhotonView>().OwnerActorNr == actorNumber2)
            {
                if (carSelected2 == 0)
                {
                    sup2.color = new Color(colorR2/255f, colorG2/255f, colorB2/255f);
                    //GameObject[] su2 = GameObject.FindGameObjectsWithTag("supraColor");

                    Material[] n0 = su2[0].GetComponent<MeshRenderer>().materials;
                    n0[2] = sup2;
                    su2[0].GetComponent<MeshRenderer>().materials = n0;

                    Material[] n1 = su2[1].GetComponent<MeshRenderer>().materials;
                    n1[3] = sup2;
                    su2[1].GetComponent<MeshRenderer>().materials = n1;

                    Material[] n2 = su2[2].GetComponent<MeshRenderer>().materials;
                    n2[3] = sup2;
                    su2[2].GetComponent<MeshRenderer>().materials = n2;

                    Material[] n3 = su2[3].GetComponent<MeshRenderer>().materials;
                    n3[3] = sup2;
                    su2[3].GetComponent<MeshRenderer>().materials = n3;

                    Material[] n4 = su2[4].GetComponent<MeshRenderer>().materials;
                    n4[3] = sup2;
                    su2[4].GetComponent<MeshRenderer>().materials = n4;

                    Material[] n5 = su2[5].GetComponent<MeshRenderer>().materials;
                    n5[9] = sup2;
                    su2[5].GetComponent<MeshRenderer>().materials = n5;

                    Material[] n6 = su2[6].GetComponent<MeshRenderer>().materials;
                    n6[8] = sup2;
                    su2[6].GetComponent<MeshRenderer>().materials = n6;

                    Material[] n7 = su2[7].GetComponent<MeshRenderer>().materials;
                    n7[1] = sup2;
                    su2[7].GetComponent<MeshRenderer>().materials = n7;

                }
                else if (carSelected2 == 1)
                {
                    por2.color = new Color(colorR2 / 255f, colorG2 / 255f, colorB2 / 255f);
                    //GameObject[] po2 = GameObject.FindGameObjectsWithTag("porColor");

                    Material[] n0 = po2[0].GetComponent<MeshRenderer>().materials;
                    n0[0] = por2;
                    po2[0].GetComponent<MeshRenderer>().materials = n0;
                }
                else if (carSelected2 == 2)
                {
                    chi2.color = new Color(colorR2 / 255f, colorG2 / 255f, colorB2 / 255f);
                    //GameObject[] ch2 = GameObject.FindGameObjectsWithTag("chiColor");

                    Material[] n0 = ch2[0].GetComponent<MeshRenderer>().materials;
                    n0[0] = chi2;
                    ch2[0].GetComponent<MeshRenderer>().materials = n0;

                    Material[] n1 = ch2[1].GetComponent<MeshRenderer>().materials;
                    n1[0] = chi2;
                    ch2[1].GetComponent<MeshRenderer>().materials = n1;

                    Material[] n2 = ch2[2].GetComponent<MeshRenderer>().materials;
                    n2[0] = chi2;
                    ch2[2].GetComponent<MeshRenderer>().materials = n2;

                    Material[] n3 = ch2[3].GetComponent<MeshRenderer>().materials;
                    n3[0] = chi2;
                    ch2[3].GetComponent<MeshRenderer>().materials = n3;

                    Material[] n4 = ch2[4].GetComponent<MeshRenderer>().materials;
                    n4[0] = chi2;
                    ch2[4].GetComponent<MeshRenderer>().materials = n4;

                    Material[] n5 = ch2[5].GetComponent<MeshRenderer>().materials;
                    n5[0] = chi2;
                    ch2[5].GetComponent<MeshRenderer>().materials = n5;
                }
            }
        }
        
    }

    [PunRPC]
    public void SetColorToServerSingle(int actorNumber1, int carSelected1, float colorR1, float colorG1, float colorB1)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(objects.Length + "---- objects.length");

        GameObject[] findSu = GameObject.FindGameObjectsWithTag("supraColor");
        List<GameObject> su = new List<GameObject>();
        List<GameObject> su2 = new List<GameObject>();
        Debug.Log(findSu.Length);

        for (int j = 0; j < findSu.Length; j++)
        {
            if (findSu[j].gameObject.transform.root.transform.GetComponentInChildren<PhotonView>().GetComponent<PhotonView>().OwnerActorNr == actorNumber1)
            {
                su.Add(findSu[j]);
                Debug.Log(findSu[j] + "-------------" + j);
            }
            
        }
        Debug.Log(su.Count + " --su count--" + su2.Count);



        GameObject[] findPo = GameObject.FindGameObjectsWithTag("porColor");
        List<GameObject> po = new List<GameObject>();
        List<GameObject> po2 = new List<GameObject>();
        for (int j = 0; j < findPo.Length; j++)
        {
            if (findPo[j].gameObject.transform.root.transform.GetComponentInChildren<PhotonView>().GetComponent<PhotonView>().OwnerActorNr == actorNumber1)
            {
                po.Add(findPo[j]);
            }
            
        }



        GameObject[] findCh = GameObject.FindGameObjectsWithTag("chiColor");
        List<GameObject> ch = new List<GameObject>();
        List<GameObject> ch2 = new List<GameObject>();
        for (int j = 0; j < findCh.Length; j++)
        {
            if (findCh[j].gameObject.transform.root.transform.GetComponentInChildren<PhotonView>().GetComponent<PhotonView>().OwnerActorNr == actorNumber1)
            {
                ch.Add(findCh[j]);
            }
            
        }

        for (int i = 0; i < objects.Length; i++)
        {
            Debug.Log(objects[i].GetComponent<PhotonView>().OwnerActorNr + " objects[i] number" + i);
            if (objects[i].GetComponent<PhotonView>().OwnerActorNr == actorNumber1)
            {
                if (carSelected1 == 0)
                {
                    sup1.color = new Color(colorR1 / 255f, colorG1 / 255f, colorB1 / 255f);
                    /*
                    GameObject[] findSu = GameObject.FindGameObjectsWithTag("supraColor");
                    List<GameObject> su = new List<GameObject>();
                    
                    for (int j = 0; j < findSu.Length-1; j++)
                    {
                        if (findSu[j].gameObject.GetComponent<PhotonView>().OwnerActorNr == actorNumber1)
                        {
                            su.Add(findSu[j]);
                        }
                    }*/


                    Material[] m0 = su[0].GetComponent<MeshRenderer>().materials;
                    m0[2] = sup1;
                    su[0].GetComponent<MeshRenderer>().materials = m0;

                    Material[] m1 = su[1].GetComponent<MeshRenderer>().materials;
                    m1[3] = sup1;
                    su[1].GetComponent<MeshRenderer>().materials = m1;

                    Material[] m2 = su[2].GetComponent<MeshRenderer>().materials;
                    m2[3] = sup1;
                    su[2].GetComponent<MeshRenderer>().materials = m2;

                    Material[] m3 = su[3].GetComponent<MeshRenderer>().materials;
                    m3[3] = sup1;
                    su[3].GetComponent<MeshRenderer>().materials = m3;

                    Material[] m4 = su[4].GetComponent<MeshRenderer>().materials;
                    m4[3] = sup1;
                    su[4].GetComponent<MeshRenderer>().materials = m4;

                    Material[] m5 = su[5].GetComponent<MeshRenderer>().materials;
                    m5[9] = sup1;
                    su[5].GetComponent<MeshRenderer>().materials = m5;

                    Material[] m6 = su[6].GetComponent<MeshRenderer>().materials;
                    m6[8] = sup1;
                    su[6].GetComponent<MeshRenderer>().materials = m6;

                    Material[] m7 = su[7].GetComponent<MeshRenderer>().materials;
                    m7[1] = sup1;
                    su[7].GetComponent<MeshRenderer>().materials = m7;

                }
                else if (carSelected1 == 1)
                {
                    por1.color = new Color(colorR1 / 255f, colorG1 / 255f, colorB1 / 255f);
                    /*
                    GameObject[] findPo = GameObject.FindGameObjectsWithTag("porColor");
                    List<GameObject> po = new List<GameObject>();
                    List<GameObject> po2 = new List<GameObject>();
                    for (int j = 0; j < findSu.Length - 1; j++)
                    {
                        if (findSu[j].gameObject.GetComponent<PhotonView>().OwnerActorNr == actorNumber1)
                        {
                            po.Add(findSu[j]);
                        }
                        else if (findSu[j].gameObject.GetComponent<PhotonView>().OwnerActorNr == actorNumber2)
                        {
                            po2.Add(findSu[j]);
                        }
                    }
                    */

                    Material[] m0 = po[0].GetComponent<MeshRenderer>().materials;
                    m0[0] = por1;
                    po[0].GetComponent<MeshRenderer>().materials = m0;
                }
                else if (carSelected1 == 2)
                {
                    chi1.color = new Color(colorR1 / 255f, colorG1 / 255f, colorB1 / 255f);
                    // GameObject[] ch = GameObject.FindGameObjectsWithTag("chiColor");

                    Material[] m0 = ch[0].GetComponent<MeshRenderer>().materials;
                    m0[0] = chi1;
                    ch[0].GetComponent<MeshRenderer>().materials = m0;

                    Material[] m1 = ch[1].GetComponent<MeshRenderer>().materials;
                    m1[0] = chi1;
                    ch[1].GetComponent<MeshRenderer>().materials = m1;

                    Material[] m2 = ch[2].GetComponent<MeshRenderer>().materials;
                    m2[0] = chi1;
                    ch[2].GetComponent<MeshRenderer>().materials = m2;

                    Material[] m3 = ch[3].GetComponent<MeshRenderer>().materials;
                    m3[0] = chi1;
                    ch[3].GetComponent<MeshRenderer>().materials = m3;

                    Material[] m4 = ch[4].GetComponent<MeshRenderer>().materials;
                    m4[0] = chi1;
                    ch[4].GetComponent<MeshRenderer>().materials = m4;

                    Material[] m5 = ch[5].GetComponent<MeshRenderer>().materials;
                    m5[0] = chi1;
                    ch[5].GetComponent<MeshRenderer>().materials = m5;
                }
            }


            
        }

    }

    [PunRPC]
    public void ColorLoaded()
    {
        colorFinishedUser++;
    }

    [PunRPC]
    public void FirstPlayerEntered(int enteredActorNum, string enteredLapTime)
    {
        manager.FirstPlayerEntered(enteredActorNum, enteredLapTime);
    }
    
    public void ReturnCurrentScore()
    {

    }
}
