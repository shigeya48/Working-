using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class TitleInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 1; i < 5; i++)
        {
            //各プレイヤーの対応コントローラーを設定する
            if (GamePad.GetButtonDown(GamePad.Button.A, (GamePad.Index)i))
            {
                SceneFader.Instance.LoadLevel("Entry_Sanoki");
            }
        }
    }
}
