using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGimmick
{
    void PlayGimmick(GameObject player);

    bool GimmickIsEvent();
}

public interface IItem
{
    void PlayItem(GameObject player);

    // プレイヤーに持たせる場合のローカルの座標と回転
    // 各アイテムクラスでそれぞれ設定する
    Vector3 LocalPosition();
    Vector3 LocalRotation();

    Vector3 PlayerColliderSize();
    Vector3 PlayerColliderCenter();

    Vector3 PlayerColliderIsTriggerSize();
    Vector3 PlayerColliderIsTriggerCenter();

    bool isItemCatch
    {
        get;
        set;
    }
}