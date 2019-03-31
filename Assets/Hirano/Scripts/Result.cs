using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    //稼いだお金
    private int money = Data.SCORE;
    //ミスした回数
    private int missscore = Data.CLAIM_POINT;
    //実際のスコア
    private int score = Data.TOTAL_SCORE;

    private int minscore = 0;
    [SerializeField]
    private Text ResultLabel;
    [SerializeField]
    private GameObject WhiteImage;
    [SerializeField]
    private GameObject ScoreText;

    [SerializeField]
    private GameObject[] playerModels = new GameObject[4];
    private Animator[] playerAnims = new Animator[4];

    private int playerCount = 0;
	// Use this for initialization
	void Start ()
    {
        //プレイヤーの数だけアニメーションを読み込む
        //いなかったらSetActiveをFalseにする
        for (int i = 0; i < playerModels.Length; i++)
        {
            if (EntrySystem.playerNumber[i] != -1)
            {
                playerCount += 1;
            }
            else
            {
                playerModels[i].SetActive(false);
            }
        }

        for (int i = 0; i < playerCount; i++)
        {
            playerAnims[i] = playerModels[i].GetComponent<Animator>();
        }

        switch(playerCount)
        {
            case 1:
                minscore = 4000;
                break;
            case 2:
                minscore = 6000;
                break;
            case 3:
                minscore = 9000;
                break;
            case 4:
                minscore = 12000;
                break;
            default:
                minscore = 4000;
                break;
        }

        
        if (score <= minscore)
        {
            ScoreLabel("FAILDE");
            StartCoroutine(AllAnimator("failed"));
        }
        else
        {
            ScoreLabel("CLEAR");
            StartCoroutine(AllAnimator("clear"));
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    void ScoreLabel(string label)
    {
        ResultLabel.text =label;
    }

    IEnumerator AllAnimator(string animname)
    {
        yield return new WaitForSeconds(2);
        //プレイヤーのアニメーション再生
        for (int i = 0; i < playerCount; i++)
        {
            playerAnims[i].SetBool(animname, true);
        }
        yield return new WaitForSeconds(1.5f);
       
        
        //Backの白を生成
        WhiteImage.SetActive(true);
        ScoreText.GetComponent<Text>().text = "稼いだお金:" + money + "\n" + "ミス:" + missscore + "\n" + "スコア:" + score;
        ScoreText.SetActive(true);
    }
}
