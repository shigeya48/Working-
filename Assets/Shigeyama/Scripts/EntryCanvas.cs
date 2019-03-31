using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryCanvas : SingletonMonoBehaviour<EntryCanvas>
{
    [SerializeField]
    Image[] buttonImage = new Image[4];

    [SerializeField]
    Sprite[] sprites = new Sprite[2];

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < buttonImage.Length; i++)
        {
            buttonImage[i].sprite = sprites[0];
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EntryDone()
    {
        int playerCount = 0;
        while (playerCount < buttonImage.Length)
        {
            if (buttonImage[playerCount].sprite == sprites[0])
            {
                buttonImage[playerCount].sprite = sprites[1];
                playerCount = buttonImage.Length;
;           }
            playerCount++;
        }
    }

    public void EntryRelease()
    {
        int playerCount = buttonImage.Length;
        while (playerCount > 0)
        {
            playerCount--;
            if (buttonImage[playerCount].sprite == sprites[1])
            {
                buttonImage[playerCount].sprite = sprites[0];
                playerCount = -1;
            }
        }
    }
}
