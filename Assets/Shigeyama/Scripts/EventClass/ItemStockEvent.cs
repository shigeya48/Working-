using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//-----------------------------------------------
// 棚を管理するスクリプト
// プレイヤーからのアクセスと
// AIからのアクセスを取ります
//-----------------------------------------------

public class ItemStockEvent : MonoBehaviour, IGimmick
{
    // 現在のストック数
    int stock;

    //---------------------------------

    // AIが移動可能かどうか
    bool isAIMove;

    // 各場所が空いているかどうか
    bool[] isAIMoves;

    bool isEvent;

    // 移動先の座標
    GameObject[] aiMovePosObjects;

    // 移動先保存変数
    GameObject aiMovePosObj;

    GameObject worningIcon;

    GameObject checkIcon;

    GameObject[] shelfItems;

    //----------------------------------

    // Use this for initialization
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

        shelfItems = new GameObject[transform.parent.GetChild(1).childCount];

        for (int i = 0; i < shelfItems.Length; i++)
        {
            shelfItems[i] = transform.parent.GetChild(1).GetChild(i).gameObject;
        }

        // ストックの初期値
        stock = shelfItems.Length;
    }

    void Update()
    {
        if (Data.ISSTOP) StopAllCoroutines();
    }

    /// <summary>
    /// AIが呼び出す用のメソッド、指定した数ストック数が減る
    /// </summary>
    /// <param name="decrement"> 一度に減らすストック数 </param>
    public void StockDecrement(int decrement)
    {
        if (stock < 1) return;

        // ストックの減少
        stock -= decrement;

        // アイテムを減少させる処理
        for (int i = 0; i < decrement; i++)
        {
            int rnd = Random.Range(0, shelfItems.Length);

            if (shelfItems[rnd].GetComponent<MeshRenderer>().enabled == true)
            {
                shelfItems[rnd].GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                i--;
            }

            if (ShelfItemCheck())
            {
                break;
            }
        }

        // ストック数が0を下回ったら
        if (stock < 1)
        {
            // ストック数の下限は0にする
            stock = 0;
            // 補充しないとやばいってのを伝える
            StartCoroutine(EventTimer());
        }
    }

    bool ShelfItemCheck()
    {
        bool isItemNull = true;

        foreach(GameObject item in shelfItems)
        {
            if (item.GetComponent<MeshRenderer>().enabled == true)
            {
                isItemNull = false;
                break;
            }
        }

        return isItemNull;
    }

    /// <summary>
    /// 空いているかどうかチェックするメソッド(AI側でアクセス)
    /// </summary>
    /// <returns></returns>
    public bool CheckPos()
    {
        isAIMove = false;

        if (stock > 0)
        {
            for (int i = 0; i < isAIMoves.Length; i++)
            {
                if (!isAIMoves[i])
                {
                    // 移動可能と判断
                    isAIMove = true;
                    break;
                }
            }
        }

        return isAIMove;
    }

    /// <summary>
    /// 空いてるオブジェクトを返す
    /// </summary>
    public GameObject AIMovePosObj()
    {
        for (int i = 0; i < aiMovePosObjects.Length; i++)
        {
            if (!isAIMoves[i])
            {
                isAIMoves[i] = true;
                aiMovePosObj = aiMovePosObjects[i];

                break;
            }
        }

        return aiMovePosObj;
    }

    /// <summary>
    /// AIがイベントを終了したら移動可能にする
    /// </summary>
    /// <param name="movePosObj"></param>
    public void IsAIMoveRelease(GameObject movePosObj)
    {
        for (int i = 0; i < aiMovePosObjects.Length; i++)
        {
            if (movePosObj == aiMovePosObjects[i])
            {
                isAIMoves[i] = false;
            }
        }
    }

    /// <summary>
    /// プレイヤーが補充を行うまでの時間を計測するコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator EventTimer()
    {
        yield return new WaitForSeconds(2);

        worningIcon = Instantiate(EffectManager.Instance.worningIconEffect,
            transform.position + new Vector3(0, 2.5f, 0),
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

            if (isEvent || stock > 0)
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

    /// <summary>
    /// プレイヤーがギミック発動させるときに呼ばれるメソッド
    /// </summary>
    /// <param name="player"> プレイヤーオブジェクト </param>
    public void PlayGimmick(GameObject player)
    {
        if (stock < shelfItems.Length && Vector3.Distance(player.transform.position, gameObject.transform.position) < 3
            && player.GetComponent<PlayerSystem>().GetCatchItem != null
            && player.GetComponent<PlayerSystem>().GetCatchItem.GetComponent<CardboardItem>())
        {
            isEvent = true;
            player.GetComponent<PlayerSystem>().IsEvent = isEvent;
            StartCoroutine(AddStock(player));
        }
    }

    /// <summary>
    /// ストック数を増やすコルーチン、プレイヤーが行動することにより呼び出される
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    IEnumerator AddStock(GameObject player)
    {
        checkIcon = Instantiate(EffectManager.Instance.gaugeIconEffect,
            transform.position + new Vector3(0, 2.5f, 0),
            Quaternion.Euler(Camera.main.transform.localEulerAngles));

        float timeInterval = 2;

        checkIcon.GetComponent<GaugeSystem>().GaugeStart(timeInterval);

        yield return new WaitForSeconds(timeInterval);

        int incrementPoint = (shelfItems.Length - stock) * 20;

        // 補充完了
        stock = shelfItems.Length;
        for (int i = 0; i < shelfItems.Length; i++)
        {
            shelfItems[i].GetComponent<MeshRenderer>().enabled = true;
        }

        player.GetComponent<PlayerSystem>().DestroyItem();

        // スコア加算
        ScoreManager.Instance.ScoreIncrement(incrementPoint);

        // プレイヤーが動けるか判断する変数ギミック終了時「false」に設定
        isEvent = false;
        player.GetComponent<PlayerSystem>().IsEvent = isEvent;
    }

    public bool GimmickIsEvent()
    {
        return isEvent;
    }

    public int StockCount()
    {
        return stock;
    }
}
