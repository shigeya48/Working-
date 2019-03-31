using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushItem : MonoBehaviour, IItem
{
    // 各アイテムの持った際の座標と回転を設定する変数
    Vector3 catchPosition;
    Vector3 catchRotation;

    Vector3 playerColliderSize;
    Vector3 playerColliderCenter;

    Vector3 playerColliderIsTriggerSize;
    Vector3 playerColliderIsTriggerCenter;

    bool isCatch = false;

    // Use this for initialization
    void Awake()
    {
        catchPosition = new Vector3(0.007f, 0.02f, 0.12f);
        catchRotation = new Vector3(-10.0f, 0.0f, 22.0f);

        playerColliderCenter = new Vector3(0.0f, 0.08f, 0.01f);
        playerColliderSize = new Vector3(0.08f, 0.15f, 0.1f);

        playerColliderIsTriggerCenter = new Vector3(0.0f, 0.055f, 0.07f);
        playerColliderIsTriggerSize = new Vector3(0.08f, 0.1f, 0.03f);
    }

    // 段ボール側でプレイヤーがアクセスする機能をつける場合はここに書くといいはず
    public void PlayItem(GameObject player)
    {

    }

    public Vector3 LocalPosition()
    {
        return catchPosition;
    }

    public Vector3 LocalRotation()
    {
        return catchRotation;
    }

    public Vector3 PlayerColliderSize()
    {
        return playerColliderSize;
    }

    public Vector3 PlayerColliderCenter()
    {
        return playerColliderCenter;
    }

    public Vector3 PlayerColliderIsTriggerSize()
    {
        return playerColliderIsTriggerSize;
    }

    public Vector3 PlayerColliderIsTriggerCenter()
    {
        return playerColliderIsTriggerCenter;
    }

    public bool isItemCatch
    {
        get { return isCatch; }
        set { isCatch = value; }
    }
}
