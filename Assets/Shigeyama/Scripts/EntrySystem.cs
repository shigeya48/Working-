using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class EntrySystem : SingletonMonoBehaviour<EntrySystem>
{
    public GameObject[] characterPrefabs;
    public GameObject gameStartImage;
    public enum PLAYERNUM
    {
        ONE,
        TWO,
        THREE,
        FOUR
    }

    // 実際は-1スタート
    public static int[] playerNumber = { 1, -1, -1, -1 };
    //public static int[] playerNumber = { -1, -1, -1, -1 };

    int playerCount = 0;

    float[] rotations = { 0, 0, 0, 0 };

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            playerNumber[i] = -1;
        }
        Data.SCORE = 0;
        Data.CLAIM_POINT = 0;
        gameStartImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCount < 5)
        {
            // int = 0 かも?
            for (int i = 1; i < 5; i++)
            {
                //各プレイヤーの対応コントローラーを設定する
                if (GamePad.GetButtonDown(GamePad.Button.A, (GamePad.Index)i))
                {
                    SetPlayerNumber((PLAYERNUM)i);
                }
            }
        }

        if (playerCount > 0)
        {
            // int = 0 かも?
            for (int i = 1; i < 5; i++)
            {
                //各プレイヤーの対応コントローラーを解除する
                if (GamePad.GetButtonDown(GamePad.Button.B, (GamePad.Index)i))
                {
                    CharacterActiveFalse((PLAYERNUM)i);
                    ReleasePlayerNumber((PLAYERNUM)i);
                }

                if (GamePad.GetButtonDown(GamePad.Button.Y, (GamePad.Index)i))
                {
                    GameStart((PLAYERNUM)i);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        StickInput();
    }

    void StickInput()
    {
        if (playerCount > 0)
        {
            for (int i = 1; i < 5; i++)
            {
                rotations[i - 1] = GamePad.GetAxis(GamePad.Axis.LeftStick, (GamePad.Index)i, true).x;
                if (rotations[i - 1] >= 1.0f) rotations[i] = 1.0f;

                //foreach(int _number in playerNumber)
                //{
                //    if (i == _number)
                //    {
                //        characterPrefabs[i-1].transform.Rotate(new Vector3(0, rotations[_number - 1], 0));
                //    }
                //}
                for(int j = 0; j < playerNumber.Length; j++)
                {
                    if (playerNumber[j] == i)
                    {
                        characterPrefabs[j].transform.Rotate(new Vector3(0, rotations[i - 1] * -3.0f, 0));
                    }
                }
            }
        }
    }

    private void SetPlayerNumber(PLAYERNUM _player)
    {
        bool flg = false;

        //登録済みのコントローラーかを調べる
        foreach (int _number in playerNumber)
        {
            //登録済みの場合は、登録できない
            if (_number == (int)_player)
            {
                flg = true;

                Debug.Log("登録できません");
            }
        }

        if (!flg)
        {
            //コントローラー番号を1Pから割り当てる
            playerNumber[playerCount] = (int)_player;

            Debug.Log((PLAYERNUM)playerCount + " Player 登録完了");

            EntryCanvas.Instance.EntryDone();

            CharacterActiveTrue();

            playerCount++;
        }
    }

    private void ReleasePlayerNumber(PLAYERNUM _player)
    {
        for (int i = 0; i < playerNumber.Length; i++)
        {
            if ((int)_player == playerNumber[i])
            {
                playerNumber[i] = -1;
                playerCount--;
            }
        }

        List<int> array = new List<int>();
        int[] temp = { -1, -1, -1, -1 };

        for (int i = 0; i < playerNumber.Length; i++)
        {
            if (playerNumber[i] != -1)
            {
                array.Add(playerNumber[i]);
            }
        }

        for (int i = 0; i < array.Count; i++)
        {
            temp[i] = array[i];
        }

        for (int i = 0; i < temp.Length; i++)
        {
            playerNumber[i] = temp[i];
            Debug.Log(playerNumber[i]);

        }
    }

    private void GameStart(PLAYERNUM _player)
    {
        //登録済みのコントローラーかを調べる
        foreach (int _number in playerNumber)
        {
            // 登録済みの場合
            if (_number == (int)_player)
            {
                Data.ISSTOP = true;

                SceneFader.Instance.LoadLevel("Game");
            }
        }
    }

    void CharacterActiveTrue()
    {
        for (int i = 0; i < characterPrefabs.Length; ++i)
        {
            if (characterPrefabs[i].activeSelf) continue;

            characterPrefabs[i].transform.rotation = new Quaternion(0,180,0,0);
            characterPrefabs[i].SetActive(true);
            if (!gameStartImage.activeSelf) gameStartImage.SetActive(true);

            return;
        }
    }

    void CharacterActiveFalse(PLAYERNUM _player)
    {
        // 登録済みのコントローラーかを調べる
        foreach (int _number in playerNumber)
        {
            // 登録済みの場合
            if (_number != (int)_player) continue;

            int characterCount = characterPrefabs.Length;

            while (characterCount > 0)
            {

                characterCount--;
                if (!characterPrefabs[characterCount].activeSelf) continue;

                characterPrefabs[characterCount].SetActive(false);
                if (characterCount==0) gameStartImage.SetActive(false);
                EntryCanvas.Instance.EntryRelease();

                return;
            }
        }
        
    }
}
