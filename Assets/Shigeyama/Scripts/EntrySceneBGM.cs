using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrySceneBGM : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.Instance.PlayBGM(AUDIO.BGM_ENTRY);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
