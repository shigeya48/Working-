using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class PlayerSystem : MonoBehaviour
{
    [SerializeField, Header("Playerのナンバリング")]
    EntrySystem.PLAYERNUM PlayerNum;

    // ゲームパッドのナンバー
    int gamePadNumber;

    // 移動方向のベクトル
    Vector3 moveDirection;

    [Header("静止状態からの回転量")]
    [SerializeField]
    [Range(0, 360)]
    float m_StationaryTurnSpeed;

    [Header("動いている状態からの回転量")]
    [SerializeField]
    [Range(0, 360)]
    float m_MovingTurnSpeed;

    // Rigidbody
    Rigidbody rd;

    // Collider
    BoxCollider[] colliders;

    // 移動スピード
    float moveSpeed = 7;

    // ダッシュのスピード
    float dashSpeed = 30;

    // カメラ
    public static Camera PlayerCam;

    // 移動の際のカメラ
    GameObject DummyPlayerCam;

    // 当たり判定に当たっているオブジェクト
    GameObject hitItem;

    // プレイヤーが持っているオブジェクト
    GameObject catchItem;

    // アイテムを持っているか判断するフラグ
    bool isCatch = false;

    // イベント行動中かを判断するフラグ
    bool isEvent = false;

    // コリジョンのサイズとピボット
    Vector3[] playerColliderSize = new Vector3[2];
    Vector3[] playerColliderCenter = new Vector3[2];

    // Animator
    Animator animator;

    // Use this for initialization
    void Start()
    {
        gamePadNumber = EntrySystem.playerNumber[(int)GetPlayerNumber()];

        PlayerCam = Camera.main;

        rd = GetComponent<Rigidbody>();

        colliders = GetComponents<BoxCollider>();

        animator = GetComponent<Animator>();

        DummyPlayerCam = new GameObject();
        DummyPlayerCam.name = "PLAYERCAM_" + gamePadNumber + "_DUMMY";
        DummyPlayerCam.transform.position = PlayerCam.transform.position;
        DummyPlayerCam.transform.eulerAngles = new Vector3(0f, PlayerCam.transform.eulerAngles.y, 0f);
        DummyPlayerCam.transform.parent = PlayerCam.transform;

        playerColliderSize[0] = colliders[0].size;
        playerColliderCenter[0] = colliders[0].center;
        playerColliderSize[1] = colliders[1].size;
        playerColliderCenter[1] = colliders[1].center;

        Debug.Log("Test");
    }

    // Update is called once per frame
    void Update()
    {
        if (Data.ISSTOP)
        {
            animator.SetBool("Catch", false);
            animator.SetBool("Walk", false);
            return;
        }

        if (isEvent)
        {
            animator.SetBool("Catch", true);
            rd.isKinematic = true;
            return;
        }
        rd.isKinematic = false;
        animator.SetBool("Catch", false);
        ButtonInput();
    }

    void FixedUpdate()
    {
        if (Data.ISSTOP) return;

        if (isEvent)
        {
            return;
        }

        // 移動処理
        StickInput();
    }

    /// <summary>
    /// ボタンのインプット
    /// </summary>
    private void ButtonInput()
    {
        // つかむ
        if (GamePad.GetButtonDown(GamePad.Button.A, (GamePad.Index)gamePadNumber))
        {
            // アイテムを持っていない場合
            if (!isCatch && hitItem != null && hitItem.GetComponent<IItem>() != null && !hitItem.GetComponent<IItem>().isItemCatch)
            {
                CatchItem(hitItem);
            }
            else if (!isCatch && hitItem != null && hitItem.GetComponent<ItemBox>() != null && hitItem.GetComponent<ItemBox>().Stock > 0)
            {
                CatchItem(hitItem.GetComponent<ItemBox>().Item());
            }
            else if (!isCatch && hitItem != null && hitItem.GetComponent<LockerBox>() != null && hitItem.GetComponent<LockerBox>().BrushExistence)
            {
                CatchItem(hitItem.GetComponent<LockerBox>().Item());
            }
            // 持っている場合
            else if (isCatch)
            {
                ReleaseItem();
            }
        }
        // アクション
        if (GamePad.GetButtonDown(GamePad.Button.X, (GamePad.Index)gamePadNumber))
        {
            if (hitItem != null && hitItem.GetComponent<IGimmick>() != null && !hitItem.GetComponent<IGimmick>().GimmickIsEvent())
            {
                hitItem.GetComponent<IGimmick>().PlayGimmick(gameObject);
            }
        }
        // ダッシュ
        if (GamePad.GetButtonDown(GamePad.Button.B, (GamePad.Index)gamePadNumber))
        {
            Dash();
        }
    }

    /// <summary>
    /// スティックのインプット
    /// </summary>
    private void StickInput()
    {
        //カメラの方向ベクトルを取得
        Vector3 forward = DummyPlayerCam.transform.TransformDirection(Vector3.forward);
        Vector3 right = DummyPlayerCam.transform.TransformDirection(Vector3.right);

        //Axisにカメラの方向ベクトルを掛ける
        moveDirection = GamePad.GetAxis(GamePad.Axis.LeftStick, (GamePad.Index)gamePadNumber, true).x * right +
                        GamePad.GetAxis(GamePad.Axis.LeftStick, (GamePad.Index)gamePadNumber, true).y * forward;

        //１以上ならば、正規化(Normalize)をし、1にする
        if (moveDirection.magnitude > 1f) moveDirection.Normalize();

        //ワールド空間での方向をローカル空間に逆変換する
        //※ワールド空間でのカメラは、JoyStickと逆の方向ベクトルを持つため、Inverseをしなければならない
        Vector3 C_move = transform.InverseTransformDirection(moveDirection);

        //アークタンジェントをもとに、最終的になる角度を求める
        float m_TurnAmount = Mathf.Atan2(C_move.x, C_move.z);

        //最終的な前方に代入する
        float m_ForwardAmount = C_move.z;

        //最終的な前方になるまでの時間を計算する
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);

        //Y軸を最終的な角度になるようにする
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);

        //移動スピードを掛ける
        moveDirection *= moveSpeed * Time.deltaTime;

        moveDirection.y = 0;

        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            animator.SetBool("Walk", true);
            
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        //プレイヤーを移動させる
        rd.MovePosition(transform.position + moveDirection);
    }

    /// <summary>
    /// プレイヤーナンバーを返す
    /// </summary>
    /// <returns>プレイヤーナンバー</returns>
    public EntrySystem.PLAYERNUM GetPlayerNumber()
    {
        return PlayerNum;
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.GetComponent<IItem>() != null || collider.gameObject.GetComponent<IGimmick>() != null
            || collider.gameObject.GetComponent<ItemBox>() != null || collider.gameObject.GetComponent<LockerBox>() != null)
        {
            hitItem = collider.gameObject;
        }
    }

    /// <summary>
    /// 当たり判定から外れた場合
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerExit(Collider collider)
    {
        hitItem = null;
    }

    /// <summary>
    /// アイテムをつかむ
    /// </summary>
    private void CatchItem(GameObject itemObject)
    {
        isCatch = true;
        // アイテム側で持たれていることを保存
        itemObject.GetComponent<IItem>().isItemCatch = true;

        catchItem = itemObject;

        catchItem.transform.parent = transform;

        catchItem.GetComponent<Rigidbody>().useGravity = false;
        catchItem.GetComponent<Rigidbody>().isKinematic = true;
        catchItem.GetComponent<BoxCollider>().enabled = false;

        catchItem.transform.localRotation = Quaternion.Euler(catchItem.GetComponent<IItem>().LocalRotation());
        catchItem.transform.localPosition = catchItem.GetComponent<IItem>().LocalPosition();

        colliders[0].size = catchItem.GetComponent<IItem>().PlayerColliderSize();
        colliders[0].center = catchItem.GetComponent<IItem>().PlayerColliderCenter();

        colliders[1].size = catchItem.GetComponent<IItem>().PlayerColliderIsTriggerSize();
        colliders[1].center = catchItem.GetComponent<IItem>().PlayerColliderIsTriggerCenter();

        // アイテムの機能を発動(なんとなくプレイヤーを渡しています)
        //catchItem.GetComponent<IItem>().PlayItem(gameObject);
    }

    /// <summary>
    /// アイテムを離す
    /// </summary>
    private void ReleaseItem()
    {
        isCatch = false;
        catchItem.GetComponent<IItem>().isItemCatch = false;

        colliders[0].size = playerColliderSize[0];
        colliders[0].center = playerColliderCenter[0];

        colliders[1].size = playerColliderSize[1];
        colliders[1].center = playerColliderCenter[1];

        catchItem.GetComponent<Rigidbody>().useGravity = true;
        catchItem.GetComponent<Rigidbody>().isKinematic = false;
        catchItem.GetComponent<BoxCollider>().enabled = true;

        catchItem.transform.parent = null;
        catchItem = null;
    }

    /// <summary>
    /// イベント中かどうかを設定するプロパティ
    /// </summary>
    public bool IsEvent
    {
        set { isEvent = value; }
    }

    /// <summary>
    /// プレイヤーが持っているアイテムを返す
    /// </summary>
    public GameObject GetCatchItem
    {
        get { return catchItem; }
    }

    /// <summary>
    /// 持っているアイテムを破棄する
    /// </summary>
    public void DestroyItem()
    {
        isCatch = false;

        colliders[0].size = playerColliderSize[0];
        colliders[0].center = playerColliderCenter[0];

        colliders[1].size = playerColliderSize[1];
        colliders[1].center = playerColliderCenter[1];

        catchItem.transform.parent = null;

        Destroy(catchItem);

        catchItem = null;
    }

    /// <summary>
    /// ダッシュ
    /// </summary>
    void Dash()
    {
        EffectManager.Instance.InstanceDashEffect(transform.position);
        rd.velocity = rd.velocity + transform.forward * dashSpeed;
    }
}
