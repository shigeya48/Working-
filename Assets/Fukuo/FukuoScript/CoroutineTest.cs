using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour {

    private IEnumerator coroutine;
    void Start()
    {
        StartCoroutine("Sample1");
        StartCoroutine(loop());


    }

    private IEnumerator Sample1()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("Sample1 i:" + i);

            yield return StartCoroutine("Sample2");

        }
    }

    private IEnumerator Sample2()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Sample2 i:" + i);
            yield return new WaitForSeconds(1f);
            
        }
    }

    void Update()
    {
        Debug.Log("update-----------");

    }

    private IEnumerator loop()
    {
        for(int i = 0; i < 5; i++)
        {
            yield return null;
            Debug.Log("loop" + i.ToString());

        }
    }
    //public class Test2
    //{
    //    public IEnumerator Sample()
    //    {
    //        for (int i = 0; i < 3; i++)
    //        {
    //            Debug.Log("Test2 Sample:" + i);
    //            yield return new WaitForSeconds(1f);
    //        }
    //    }
    //}



    //private IEnumerator Start()
    //{
    //    enabled = false;
    //    yield return new WaitForSeconds(1);
    //    enabled=true;
    //}

    //void Update()
    //{
    //    Debug.Log("Update");
    //}



    //IEnumerator coroutineMethod;
    //// Use this for initialization
    //void Start()
    //{
    //    //StartCoroutine("Sample", 60);

    //    coroutineMethod = Sample(1000);
    //}

    //void OnMouseDown()
    //{
    //    StartCoroutine(coroutineMethod);
    //}
    //void OnMouseUp()
    //{
    //    StopCoroutine(coroutineMethod);
    //}

    //IEnumerator Sample(int len)
    //{
    //    for (int i = 0; i < len; i++)
    //    {
    //        yield return null;
    //        Debug.Log("i:" + i);
    //    }

    //}

    // Update is called once per frame

    //IEnumerator Sample(int len)
    //{
    //    for (int i = 0; i < len; i++)
    //    {
    //        yield return null;
    //        Debug.Log("i:" + i);
    //    }
    //}

}
