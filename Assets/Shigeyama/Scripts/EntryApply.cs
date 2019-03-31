using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryApply : MonoBehaviour
{
    [SerializeField]
    GameObject[] players;

    void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            if (EntrySystem.playerNumber[i] == -1)
            {
                players[i].SetActive(false);
            }
        }
    }
}
