using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorningSystem : MonoBehaviour
{
    Animator anim;

    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Data.ISSTOP)
        {
            anim.speed = 0;
            StopAllCoroutines();
        }
    }

    public void WorningStart(float time)
    {
        StartCoroutine(Worning(time));
    }

    public IEnumerator Worning(float time)
    {
        float timer = 0;

        anim.SetTrigger("IsWorning");
        
        while(timer < time)
        {
            timer += Time.deltaTime;

            if (time - timer < 2)
            {
                anim.speed = 5;
            }
            if (time - timer < 5)
            {
                anim.speed = 3;
            }

            yield return null;
        }

        anim.speed = 1;
        anim.SetTrigger("IsDecrement");
    }

    void WorningSound()
    {
        AudioManager.Instance.PlaySE(AUDIO.SE_WORNING);
    }
}
