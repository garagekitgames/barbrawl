using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickHipTurn : MonoBehaviour {

    public Kicking kick;
    public CharacterFaceDirection hipFacing;

    public float switchSpeed = 80f;


    // Use this for initialization
    void Start () {

        kick = GetComponent<Kicking>();
        hipFacing = GetComponent<CharacterFaceDirection>();

    }
	
	// Update is called once per frame
	void Update () {

        if (kick.leftWindUp)
        {
            hipFacing.bodyForward.y = -1 * switchSpeed;
        }
        if (kick.rightWindUp)
        {
            hipFacing.bodyForward.y = 1 * switchSpeed;
        }

    }
}
