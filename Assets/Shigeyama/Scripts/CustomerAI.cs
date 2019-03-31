using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{

    // AIの状態を管理するステート
    public enum CustomerAIState
    {
        CHECK,
        IDLE,
        MOVE,
        ACTION,
        ACCIDENT,
        REGISTERMOVE,
        REGISTERIDLE,
        REGISTERACTION
    }

    // ステートの宣言
    CustomerAIState state;
    
    // NavMeshAgentの宣言
    NavMeshAgent agent;

    // 全棚を管理する配列
    GameObject[] shelfObj;

    // 全レジを管理する配列
    GameObject[] registerObj;

    // アクションを起こす対象を保存しておく変数
    GameObject actionObj;

    // 実際に移動するポジションを記憶する変数
    GameObject movePosObj;

    // レジの順番
    int registerNum;

    // タイマー
    float timer = 0;

    // 棚から商品を取った数
    int stockCount = 0;

    bool isMoveEnd = false;

    Animator anim;

    //-- ヤンキー用 -------------------

    bool trashFlg = false;

    int trashNumber = 0;
    
    //---------------------------------

    // Use this for initialization
    void Start()
    {
        // 初期化
        agent = GetComponent<NavMeshAgent>();
        // シーン上にある棚オブジェクトを全取得
        shelfObj = GameObject.FindGameObjectsWithTag("Shelf");
        // シーン上にあるレジオブジェクトを全取得
        registerObj = GameObject.FindGameObjectsWithTag("Register");
        // アニメーションの初期化
        anim = GetComponent<Animator>();
        // 初期ステートの設定
        state = CustomerAIState.CHECK;

        trashNumber = Random.Range(1, 3);
        //trashNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Data.ISSTOP)
        {
            state = CustomerAIState.IDLE;
            agent.speed = 0;
        }

        timer += Time.deltaTime;

        // 各ステートを実行
        switch (state)
        {
            case CustomerAIState.CHECK:
                Check();
                break;
            case CustomerAIState.IDLE:
                Idle();
                break;
            case CustomerAIState.MOVE:
                Move();
                break;
            case CustomerAIState.ACTION:
                Action();
                break;
            case CustomerAIState.ACCIDENT:
                Accident();
                break;
            case CustomerAIState.REGISTERMOVE:
                RegisterMove();
                break;
            case CustomerAIState.REGISTERIDLE:
                RegisterIdle();
                break;
            case CustomerAIState.REGISTERACTION:
                RegisterAction();
                break;
        }
    }

    /// <summary>
    /// 移動先の検索とその後の行動を指定
    /// </summary>
    void Check()
    {
        anim.SetBool("Walk", false);

        List<GameObject> canMovePosObjcts = new List<GameObject>();
        
        for (int i = 0; i < shelfObj.Length; i++)
        {
            if (shelfObj[i].GetComponent<ItemStockEvent>().CheckPos() && shelfObj[i] != actionObj && stockCount < 4)
            {
                canMovePosObjcts.Add(shelfObj[i]);
            }
        }

        for (int i = 0; i < registerObj.Length; i++)
        {
            if (registerObj[i].GetComponent<RegisterEvent>().CheckPos() && stockCount > 0)
            {
                canMovePosObjcts.Add(registerObj[i]);
            }
        }

        if (canMovePosObjcts.Count > 0)
        {
            int rnd = Random.Range(0, canMovePosObjcts.Count);

            actionObj = canMovePosObjcts[rnd];

            if (actionObj.GetComponent<ItemStockEvent>())
            {
                movePosObj = actionObj.GetComponent<ItemStockEvent>().AIMovePosObj();

                timer = 0;
                state = CustomerAIState.MOVE;
            }
            else
            {
                registerNum = actionObj.GetComponent<RegisterEvent>().AIMovePosObjNum();
                movePosObj = actionObj.GetComponent<RegisterEvent>().AIMovePosObj();

                timer = 0;
                state = CustomerAIState.REGISTERMOVE;
            }
        }
        else
        {
            timer = 0;
            state = CustomerAIState.IDLE;
        }
    }

    /// <summary>
    /// 待機状態
    /// </summary>
    void Idle()
    {
        anim.SetBool("Walk", false);
        float timeInterval = 1.0f;

        if (timer > timeInterval)
        {
            state = CustomerAIState.CHECK;
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        anim.SetBool("Walk", true);
        agent.SetDestination(movePosObj.transform.position);

        float timeInterval = 0.5f;

        if (gameObject.tag == "BadBoy" && stockCount == trashNumber && timer > timeInterval && !trashFlg)
        {
            GameObject trash = Instantiate(EffectManager.Instance.trashPres[Random.Range(0, EffectManager.Instance.trashPres.Length)],
                transform.position, Quaternion.Euler(20, 100, 10));
            trash.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-0.5f, 0.5f), 0.7f, -1) * 200);
            trashFlg = true;
        }

        if (Vector3.Distance(transform.position, movePosObj.transform.position) < 1.0f)
        {
            timer = 0;
            state = CustomerAIState.ACTION;
        }
    }

    /// <summary>
    /// ギミックの発動
    /// </summary>
    void Action()
    {
        anim.SetBool("Walk", false);
        float timeInterval = 2.0f;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation
                    (new Vector3(actionObj.transform.position.x - transform.position.x,
                    0,
                    actionObj.transform.position.z - transform.position.z)), 0.1f);

        if (timer > timeInterval)
        {
            int rnd = Random.Range(1, 5);

            actionObj.GetComponent<ItemStockEvent>().StockDecrement(rnd);
            actionObj.GetComponent<ItemStockEvent>().IsAIMoveRelease(movePosObj);

            stockCount++;

            state = CustomerAIState.CHECK;
        }
    }

    /// <summary>
    /// 一定確率で起こるアクシデント(MoveまたはIdle状態から)
    /// </summary>
    void Accident()
    {
        int rnd = Random.Range(0, 5);

        switch (rnd)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    void RegisterMove()
    {
        anim.SetBool("Walk", true);
        agent.SetDestination(movePosObj.transform.position);

        float timeInterval = 0.5f;

        if (isMoveEnd && !trashFlg)
        {
            if (gameObject.tag == "BadBoy" && timer > timeInterval)
            {
                GameObject trash = Instantiate(EffectManager.Instance.trashPres[Random.Range(0, EffectManager.Instance.trashPres.Length)], 
                    transform.position, Quaternion.Euler(20, 90, 10));
                trash.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0.0f, 0.5f), 0.7f, -1) * 200);
                trashFlg = true;
            }
        }

        if (Vector3.Distance(transform.position, movePosObj.transform.position) < 1.0f)
        {
            if (isMoveEnd)
            {
                AIGenerator.Instance.RemoveList(gameObject);
                Destroy(gameObject);
            }
            else
            {
                state = CustomerAIState.REGISTERIDLE;
            }
        }
    }

    void RegisterIdle()
    {
        anim.SetBool("Walk", false);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation
                    (new Vector3(actionObj.transform.position.x - transform.position.x,
                    0,
                    actionObj.transform.position.z - transform.position.z)), 0.1f);

        if (registerNum <= 0)
        {
            actionObj.GetComponent<RegisterEvent>().IsPlayerActionEnd = false;
            state = CustomerAIState.REGISTERACTION;
        }
        else if (actionObj.GetComponent<RegisterEvent>().IsAIMoveLine(registerNum))
        {
            movePosObj = actionObj.GetComponent<RegisterEvent>().AIMoveLineObj(registerNum);
            registerNum--;
            timer = 0;
            state = CustomerAIState.REGISTERMOVE;
        }
    }

    void RegisterAction()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation
                    (new Vector3(actionObj.transform.position.x - transform.position.x,
                    0,
                    actionObj.transform.position.z - transform.position.z)), 0.1f);

        actionObj.GetComponent<RegisterEvent>().IsCustomer = true;

        if (actionObj.GetComponent<RegisterEvent>().IsPlayerActionEnd)
        {
            actionObj.GetComponent<RegisterEvent>().IsCustomer = false;
            isMoveEnd = true;
            movePosObj = GameObject.Find("CustomerEntryPos");
            timer = 0;
            state = CustomerAIState.REGISTERMOVE;
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }
}
