using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndAnimation : MonoBehaviour
{
    void GameEnd()
    {
        SceneFader.Instance.LoadLevel("Result");
    }
}
