using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {

	// Use this for initialization
	void Start () {

        AudioManager.Instance.PlayBGM(AUDIO.BGM_TOWN);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
