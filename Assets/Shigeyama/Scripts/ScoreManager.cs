using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    int score = 0;

    int claimPoint = 0;

    int totalScore = 0;

    [SerializeField]
    Text scoreText;

    [SerializeField]
    GameObject incrementTextPre;

    [SerializeField]
    GameObject decrementTextPre;

    void Start()
    {
        scoreText.text = "Score : " + score.ToString();
    }


    public void ScoreDecrement()
    {
        if (score < 0)
        {
            score = 0;
        }
        claimPoint++;

        totalScore -= 150;
        if (totalScore < 0)
        {
            totalScore = 0;
        }

        Instantiate(decrementTextPre, scoreText.rectTransform.position + new Vector3(-80, 0, 0), Quaternion.identity, transform);

        AudioManager.Instance.PlaySE(AUDIO.SE_MISTAKE);

        scoreText.text = "Score : " + totalScore.ToString();
    }

    public void ScoreIncrement(int plusPoint)
    {
        score += plusPoint;

        totalScore += plusPoint;

        GameObject incrementText = Instantiate(incrementTextPre, scoreText.rectTransform.position + new Vector3(-80, -70, 0), Quaternion.identity, transform);
        incrementText.transform.GetChild(0).GetComponent<Text>().text = "+" + plusPoint;

        AudioManager.Instance.PlaySE(AUDIO.SE_MONEY);

        scoreText.text = "Score : " + totalScore.ToString();
    }

    public int Score
    {
        get { return score; }
    }

    public int ClaimPoint
    {
        get { return claimPoint; }
    }

    public int TotalScore
    {
        get { return totalScore; }
    }
}
