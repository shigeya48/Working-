using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField]
    GameObject SetItem;

    OrderEvent orderEvent;

    int stock = 0;

    GameObject checkIcon;

    void Start()
    {
        stock = transform.childCount;

        orderEvent = FindObjectOfType<OrderEvent>();
    }

    public GameObject Item()
    {
        stock--;
        transform.GetChild(stock).gameObject.GetComponent<MeshRenderer>().enabled = false;

        if (stock < 1 && !orderEvent.EventAccessFlg)
        {
            orderEvent.EventStart();
        }

        return Instantiate(SetItem, transform.position, Quaternion.identity);
    }

    public void AddStockStart()
    {
        StartCoroutine(AddStock());
    }

    IEnumerator AddStock()
    {
        checkIcon = Instantiate(EffectManager.Instance.gaugeIconEffect,
            transform.GetChild(7).transform.position + new Vector3(0, 1.0f, 0),
            Quaternion.Euler(Camera.main.transform.localEulerAngles));

        float timeInterval = 10;

        checkIcon.GetComponent<GaugeSystem>().GaugeStart(timeInterval);

        yield return new WaitForSeconds(timeInterval);

        int incrementPoint = (transform.childCount - stock) * 30;

        // スコア加算
        ScoreManager.Instance.ScoreIncrement(incrementPoint);

        stock = transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

        orderEvent.EventAccessFlg = false;
    }

    public int Stock
    {
        get { return stock; }
        set { stock = value; }
    }

    public int MacStock
    {
        get {return transform.childCount; }
    }
}
