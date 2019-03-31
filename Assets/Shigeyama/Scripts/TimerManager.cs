using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField]
    Text timerText;

    float timer = 0;

    // Use this for initialization
    void Start()
    {
        timer = 120.0f;
        timerText.text = "Time:" + timer.ToString("F0");
    }

    // Update is called once per frame
    void Update()
    {
        if (Data.ISSTOP) return;

        timer -= Time.deltaTime;

        if (timer < 0.5f)
        {
            timer = 0;
            AudioManager.Instance.PlaySE(AUDIO.SE_TIMEUP);
            Data.SCORE = ScoreManager.Instance.Score;
            Data.CLAIM_POINT = ScoreManager.Instance.ClaimPoint;
            Data.TOTAL_SCORE = ScoreManager.Instance.TotalScore;
            Data.ISSTOP = true;
            GameObject.Find("GameEndAnimationCanvas").GetComponent<Animator>().SetTrigger("IsTimeUp");
        }

        timerText.text = "Time:" + timer.ToString("F0");
    }

    public float Timer
    {
        get { return timer; }
    }
}
