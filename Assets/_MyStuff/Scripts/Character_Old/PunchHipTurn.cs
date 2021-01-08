using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchHipTurn : MonoBehaviour {

    public Punching punch;
    public CharacterFaceDirection hipFacing;

    public float switchSpeed = 80f;


    // Use this for initialization
    void Start () {

        punch = GetComponent<Punching>();
        hipFacing = GetComponent<CharacterFaceDirection>();

    }
	
	// Update is called once per frame
	void Update () {

        if (punch.leftWindUp)
        {
            hipFacing.bodyForward.y = 0;
        }
        if (punch.rightWindUp)
        {
            hipFacing.bodyForward.y = 0;
        }

    }
}
