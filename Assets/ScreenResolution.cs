using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution : MonoBehaviour {
    private void Awake()
    {
        Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, true);
    }
    // Use this for initialization
    void Start () {

        //Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, true);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
