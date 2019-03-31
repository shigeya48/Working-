using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    Text[] telops;

    [SerializeField]
    GameObject[] eventObj;

    Animator anim;

    bool isEvent = false;

    Text telopText;

    float speed = 500;

    TimerManager timeManager;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

        timeManager = FindObjectOfType<TimerManager>();

        telopText = telops[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (timeManager.Timer < 60.0f && !isEvent)
        {
            isEvent = true;
            anim.SetTrigger("IsEvent");
        }
    }

    void TelopStart()
    {
        StartCoroutine(TelopMove());
    }

    IEnumerator TelopMove()
    {
        while (telopText.rectTransform.position.x > -2000)
        {
            telopText.rectTransform.position += Vector3.right * -speed * Time.deltaTime;
            yield return null;
        }

        anim.SetTrigger("IsTelopEnd");
        Destroy(telops[0]);

        Instantiate(eventObj[0], GameObject.Find("CustomerEntryPos").transform.position, Quaternion.identity);
    }
}
