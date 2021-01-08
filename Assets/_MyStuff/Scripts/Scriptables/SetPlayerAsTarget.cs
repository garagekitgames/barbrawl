using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

public class SetPlayerAsTarget : MonoBehaviour {

    public TransformVariable target;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        target.value = transform;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
