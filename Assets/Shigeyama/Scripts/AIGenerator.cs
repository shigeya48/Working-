using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGenerator : SingletonMonoBehaviour<AIGenerator>
{
    [SerializeField, Header("AIPrefab")]
    GameObject[] aiObjPre;

    Vector3 entryPos;

    List<GameObject> AIList = new List<GameObject>();

    float timer = 0;
    float timeInterval;

    // 店の中にいるAIの最大数
    int MAXCUSTOMER = 7;

    // Use this for initialization
    void Start()
    {
        entryPos = GameObject.Find("CustomerEntryPos").transform.position;
        timeInterval = 0;

        int playerCount = 0;

        for (int i = 0; i < EntrySystem.playerNumber.Length; i++)
        {
            if (EntrySystem.playerNumber[i] != -1)
            {
                playerCount++;
            }
        }

        switch (playerCount)
        {
            case 1:
                MAXCUSTOMER = 7;
                break;
            case 2:
                MAXCUSTOMER = 10;
                break;
            case 3:
                MAXCUSTOMER = 12;
                break;
            case 4:
                MAXCUSTOMER = 15;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Data.ISSTOP) return;

        if (AIList.Count < MAXCUSTOMER)
        {
            timer += Time.deltaTime;

            if (timer > timeInterval)
            {
                int rnd = Random.Range(0, aiObjPre.Length);

                AIList.Add(Instantiate(aiObjPre[rnd], entryPos, Quaternion.identity));
                timer = 0;
                timeInterval = Random.Range(1.0f, 6.0f);
            }
        }
    }

    public void RemoveList(GameObject removeObj)
    {
        for (int i = 0; i < AIList.Count; i++)
        {
            if (AIList[i] == removeObj)
            {
                AIList.RemoveAt(i);
            }
        }
    }
}
