using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    public GameObject dashEffect;

    public GameObject alertIconEffect;

    public GameObject angryIconEffect;

    public GameObject angryEffect;

    public GameObject heartIconEffect;

    public GameObject heartEffect;

    public GameObject gaugeIconEffect;

    public GameObject worningIconEffect;

    public GameObject[] trashPres;

    public void InstanceDashEffect(Vector3 pos)
    {
        Instantiate(dashEffect, pos, Quaternion.identity);
    }

}
