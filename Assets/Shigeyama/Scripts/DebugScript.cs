using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugScript : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.R))
        {
            SceneFader.Instance.LoadLevel("Result");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneFader.Instance.LoadLevel("Game");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneFader.Instance.LoadLevel("Title");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SceneFader.Instance.LoadLevel("Entry_Sanoki");
        }

    }
}
