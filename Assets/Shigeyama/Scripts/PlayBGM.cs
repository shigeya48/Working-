using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        AudioManager.Instance.PlayBGM(AUDIO.BGM_GAMEBGM01);
    }

}
