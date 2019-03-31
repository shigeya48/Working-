using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoar : MonoBehaviour
{
    public Animator anim1;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            anim1.SetTrigger("Auto");
        }
    }
}
