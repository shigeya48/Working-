using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeSystem : MonoBehaviour
{
    // ゲージのオブジェクト
    GameObject gauge;

    Animator anim;

    // Use this for initialization
    void Awake()
    {
        gauge = transform.GetChild(1).gameObject;
        gauge.transform.localScale = new Vector3(0, gauge.transform.localScale.y, gauge.transform.localScale.z);

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Data.ISSTOP) StopAllCoroutines();
    }

    public void GaugeStart(float time)
    {
        StartCoroutine(GaugeTimer(time));
    }

    public IEnumerator GaugeTimer(float time)
    {
        float timer = 0.0f;
        float gaugeScale = 0.0f;
        while(time > timer)
        {
            timer += Time.deltaTime;

            gaugeScale = (0.75f * timer) / time;

            gauge.transform.localScale = new Vector3(gaugeScale,
                gauge.transform.localScale.y, gauge.transform.localScale.z);
            yield return null;
        }

        gauge.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        anim.SetTrigger("IsCheck");

        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}
