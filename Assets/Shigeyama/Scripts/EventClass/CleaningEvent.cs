using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningEvent : MonoBehaviour,IGimmick
{
    bool isEvent;

    GameObject worningIcon;

    GameObject checkIcon;

    void Start()
    {
        StartCoroutine(EventAlert());
    }

    IEnumerator EventAlert()
    {
        yield return new WaitForSeconds(3);

        worningIcon = Instantiate(EffectManager.Instance.worningIconEffect,
            transform.position + new Vector3(0, 1.5f, 0),
            Quaternion.Euler(Camera.main.transform.localEulerAngles));

        float timeInterval = 10;

        float timer = 0;

        worningIcon.GetComponent<WorningSystem>().WorningStart(timeInterval);

        while (timer < timeInterval)
        {
            timer += Time.deltaTime;

            worningIcon.transform.position = transform.position + new Vector3(0, 1.5f, 0);

            if (timer > timeInterval)
            {
                timer = timeInterval;
            }

            if (isEvent)
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

    /// <summary>
    /// プレイヤーがアクセスするメソッド
    /// </summary>
    /// <param name="player"></param>
    public void PlayGimmick(GameObject player)
    {
        if (player.GetComponent<PlayerSystem>().GetCatchItem!= null
            && player.GetComponent<PlayerSystem>().GetCatchItem.GetComponent<BrushItem>())
        {
            isEvent = true;
            player.GetComponent<PlayerSystem>().IsEvent = isEvent;
            StartCoroutine(PlayCleaning(player));
        }
    }

    /// <summary>
    /// イベント用コルーチン
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    IEnumerator PlayCleaning(GameObject player)
    {
        checkIcon = Instantiate(EffectManager.Instance.gaugeIconEffect,
            transform.position + new Vector3(0, 2.5f, 0),
            Quaternion.Euler(Camera.main.transform.localEulerAngles));

        float timeInterval = 2;

        checkIcon.GetComponent<GaugeSystem>().GaugeStart(timeInterval);

        yield return new WaitForSeconds(timeInterval);

        int incrementPoint = 200;

        // スコア加算
        ScoreManager.Instance.ScoreIncrement(incrementPoint);

        // プレイヤーが動けるか判断する変数ギミック終了時「false」に設定
        isEvent = false;
        player.GetComponent<PlayerSystem>().IsEvent = isEvent;

        Destroy(gameObject);
    }

    public bool GimmickIsEvent()
    {
        return isEvent;
    }
}
