using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonTest : SingletonMonoBehaviour<SingletonTest>
{
    public void TestDebug(Animator anim)
    {
        anim.SetTrigger("");
    }
}
