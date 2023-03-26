using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingSceneBackground : MonoBehaviour
{
    public Sprite[] sprites;
    public Image image;
    public GameObject tip1;
    public GameObject tip2;
    public GameObject tip3;

    // Start is called before the first frame update
    void Start()
    {
        randomImage();
    }
  
    private void randomImage()
    {
        int tip = Random.Range(0, 3);
        if (tip == 0)
        {
            tip1.SetActive(true);
        }
        else if (tip == 1)
        {
            tip2.SetActive(true);
        }
        else
        {
            tip3.SetActive(true);

        }
        int index = Random.Range(0, sprites.Length);
        Sprite select = sprites[index];
        image.sprite = select;
    }
}
