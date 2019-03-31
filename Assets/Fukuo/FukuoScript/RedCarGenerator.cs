using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCarGenerator : MonoBehaviour {

    public GameObject RedCar;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(RedCar);

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
