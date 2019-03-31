using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterEvent : MonoBehaviour, IGimmick
{

    // AIが移動可能かどうか
    bool isAIMove;

    // 各場所が空いているかどうか
    bool[] isAIMoves;

    // 移動先の座標
    GameObject[] aiMovePosObjects;

    // 移動先保存変数
    GameObject aiMovePosObj;

    GameObject worningIcon;

    GameObject checkIcon;

    int customerCounter = 0;

    // レジに客がいるかどうか
    bool isCustomer;

    // プレイヤーがアクションを終えたかどうか
    bool isPlayerActionEnd = false;

    bool updateStop = false;

    bool isEvent = false;

    void Awake()
    {
        isAIMoves = new bool[transform.childCount];
        aiMovePosObjects = new GameObject[transform.childCount];

        // 配列を子供の数分初期化して長さを決める
        for (int i = 0; i < transform.childCount; i++)
        {
            isAIMoves[i] = false;
            // 座標取得
            aiMovePosObjects[i] = transform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        if (Data.ISSTOP) StopAllCoroutines();

        if (updateStop) return;

        if (isCustomer)
        {
            updateStop = true;

            StartCoroutine(EventTimer());
        }
    }

    public bool CheckPos()
    {
        isAIMove = false;

        if (customerCounter < isAIMoves.Length)
        {
            isAIMove = true;
        }

        return isAIMove;
    }

    public int AIMovePosObjNum()
    {
        customerCounter++;
        isAIMoves[customerCounter - 1] = true;
        return customerCounter - 1;
    }

    public GameObject AIMovePosObj()
    {
        return aiMovePosObjects[customerCounter - 1];
    }

    //------------------------------------------------------------

    public bool IsAIMoveLine(int registerNum)
    {
        bool isMove = false;
        if (!isAIMoves[registerNum - 1])
        {
            isMove = true;
        }

        return isMove;
    }

    public GameObject AIMoveLineObj(int registerNum)
    {
        // 自身のいた場所は空に
        isAIMoves[registerNum] = false;
        // 向かう場所は誰も来れないように
        isAIMoves[registerNum - 1] = true;

        return aiMovePosObjects[registerNum - 1];
    }

    IEnumerator EventTimer()
    {
        yield return new WaitForSeconds(2);

        worningIcon = Instantiate(EffectManager.Instance.worningIconEffect,
           transform.position + new Vector3(0, 2.0f, 0),
           Quaternion.Euler(Camera.main.transform.localEulerAngles));

        float timeInterval = 10;

        float timer = 0;

        worningIcon.GetComponent<WorningSystem>().WorningStart(timeInterval);

        while (timer < timeInterval)
        {
            timer += Time.deltaTime;

            if (timer > timeInterval)
            {
                timer = timeInterval;
            }

            if (isEvent || !isCustomer)
            {
                Destroy(worningIcon);
                yield break;
            }

            yield return null;
        }

        // スコアの減少(クレーム)
        ScoreManager.Instance.ScoreDecrement();
        Destroy(worningIcon, 1.2f);

        yield return null;
        StartCoroutine(EventTimer());
    }

    //-------------------------------------------------------------

    public void PlayGimmick(GameObject player)
    {
        if (isCustomer && player.GetComponent<PlayerSystem>().GetCatchItem == null)
        {
            isEvent = true;
            player.GetComponent<PlayerSystem>().IsEvent = isEvent;

            StartCoroutine(PlayRegister(player));
        }
    }

    IEnumerator PlayRegister(GameObject player)
    {
        checkIcon = Instantiate(EffectManager.Instance.gaugeIconEffect,
            transform.position + new Vector3(0, 2.0f, 0),
            Quaternion.Euler(Camera.main.transform.localEulerAngles));

        float timeInterval = 2;

        checkIcon.GetComponent<GaugeSystem>().GaugeStart(timeInterval);

        yield return new WaitForSeconds(timeInterval);

        // スコア加算
        ScoreManager.Instance.ScoreIncrement(200);

        isPlayerActionEnd = true;
        isCustomer = false;
        isAIMoves[0] = false;
        updateStop = false;
        customerCounter--;
        isEvent = false;
        player.GetComponent<PlayerSystem>().IsEvent = isEvent;
        yield return null;
    }

    public bool IsCustomer
    {
        set { isCustomer = value; }
    }

    public bool IsPlayerActionEnd
    {
        get { return isPlayerActionEnd; }
        set { isPlayerActionEnd = value; } 
    }

    public bool GimmickIsEvent()
    {
        return isEvent;
    }
}
