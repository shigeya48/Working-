using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MillionaireEvent : MonoBehaviour
{
    public enum MillionaireActionState
    {
        IDLE,
        STARTMOVE,
        CHECK,
        HEART,
        ANGRY,
        ENDMOVE
    }

    MillionaireActionState state;

    Vector3 storeEntrancePos;

    Vector3 storeExitPos;

    GameObject eventEffect;

    float timer = 0;

    float timeInterval = 0;

    int allItemStockCount = 0;

    NavMeshAgent agent;

    Animator anim;

    // Use this for initialization
    void Start()
    {
        storeEntrancePos = GameObject.Find("StoreEntrancePos").transform.position;

        storeExitPos = GameObject.Find("CustomerEntryPos").transform.position;

        agent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();

        state = MillionaireActionState.STARTMOVE;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case MillionaireActionState.IDLE:
                Idle();
                break;
            case MillionaireActionState.STARTMOVE:
                StartMove();
                break;
            case MillionaireActionState.CHECK:
                Check();
                break;
            case MillionaireActionState.HEART:
                Heart();
                break;
            case MillionaireActionState.ANGRY:
                Angry();
                break;
            case MillionaireActionState.ENDMOVE:
                EndMove();
                break;
        }
    }

    void Idle()
    {
        anim.SetBool("Walk", false);

    }

    void StartMove()
    {
        anim.SetBool("Walk", true);
        agent.SetDestination(storeEntrancePos);

        if (Vector3.Distance(transform.position, storeEntrancePos) < 1.0f)
        {
            state = MillionaireActionState.CHECK;
            timeInterval = 5;
        }
    }

    void Check()
    {
        anim.SetBool("Walk", false);
        anim.SetTrigger("IsCheck");

        timer += Time.deltaTime;

        if (timer > timeInterval)
        {
            timer = 0;

            GameObject shelfs = GameObject.Find("Shelfs");

            // 商品の数を数える
            for (int i = 0; i < shelfs.transform.childCount; i++)
            {
                allItemStockCount += shelfs.transform.GetChild(i).GetChild(0).GetComponent<ItemStockEvent>().StockCount();
            }

            if (allItemStockCount > 110)
            {
                anim.SetTrigger("IsHappy");
                eventEffect = Instantiate(EffectManager.Instance.heartEffect, transform.position + new Vector3(0.0f, 3.0f, 0.0f), Quaternion.identity);
                timeInterval = 5;
                state = MillionaireActionState.HEART;
            }
            else
            {
                anim.SetTrigger("IsAngry");
                eventEffect = Instantiate(EffectManager.Instance.angryEffect, transform.position + new Vector3(0.0f, 3.0f, 0.0f), Quaternion.identity);
                timeInterval = 3;
                state = MillionaireActionState.ANGRY;
            }
        }
    }

    void Heart()
    {
        timer += Time.deltaTime;

        if (timer > timeInterval)
        {
            timer = 0;
            ScoreManager.Instance.ScoreIncrement(1000);
            Destroy(eventEffect);
            state = MillionaireActionState.ENDMOVE;
        }
    }

    void Angry()
    {
        timer += Time.deltaTime;

        if (timer > timeInterval)
        {
            timer = 0;
            state = MillionaireActionState.ENDMOVE;
        }
    }

    void EndMove()
    {
        anim.SetBool("Walk", true);

        allItemStockCount = 0;

        agent.SetDestination(storeExitPos);

        if (Vector3.Distance(transform.position, storeExitPos) < 1.0f)
        {
            Destroy(gameObject);
        }
    }
}
