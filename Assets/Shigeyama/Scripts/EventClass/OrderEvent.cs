using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderEvent : MonoBehaviour, IGimmick
{
    // プレイヤーがイベントを行っているかどうか
    bool isEvent = false;

    ItemBox itemBox;

    GameObject worningIcon;

    GameObject checkIcon;

    bool eventAccessFlg = false;

    // Use this for initialization
    void Start()
    {
        itemBox = FindObjectOfType<ItemBox>();
    }

    public void EventStart()
    {
        StartCoroutine(EventAlert());
    }

    IEnumerator EventAlert()
    {
        yield return new WaitForSeconds(2);

        worningIcon = Instantiate(EffectManager.Instance.worningIconEffect,
            transform.position + new Vector3(0, 1.5f, 0),
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

            if (isEvent || itemBox.Stock > 0)
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
        StartCoroutine(EventAlert());
    }

    public void PlayGimmick(GameObject player)
    {
        if (itemBox.MacStock > itemBox.Stock && !eventAccessFlg && player.GetComponent<PlayerSystem>().GetCatchItem == null)
        {
            isEvent = true;
            eventAccessFlg = true;
            player.GetComponent<PlayerSystem>().IsEvent = isEvent;
            StartCoroutine(AddStock(player));
        }
    }

    IEnumerator AddStock(GameObject player)
    {
        checkIcon = Instantiate(EffectManager.Instance.gaugeIconEffect,
            transform.position + new Vector3(0, 1.5f, 0),
            Quaternion.Euler(Camera.main.transform.localEulerAngles));

        float timeInterval = 2;

        checkIcon.GetComponent<GaugeSystem>().GaugeStart(timeInterval);

        yield return new WaitForSeconds(timeInterval);

        // 発注完了
        itemBox.AddStockStart();

        // プレイヤーが動けるか判断する変数ギミック終了時「false」に設定
        isEvent = false;
        player.GetComponent<PlayerSystem>().IsEvent = isEvent;
    }

    public bool GimmickIsEvent()
    {
        return isEvent;
    }

    public bool EventAccessFlg
    {
        get { return eventAccessFlg; }
        set { eventAccessFlg = value; }
    }
}
