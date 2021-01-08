using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchKick : MonoBehaviour {

    public Vector3 inputDirection;
    public CharacterInput input;

    public float switchSpeed = 20f;

    public CharacterFaceDirection hipFacing;



    // Use this for initialization
    void Start () {

        input = GetComponent<CharacterInput>();
        hipFacing = GetComponent<CharacterFaceDirection>();

    }
	
	// Update is called once per frame
	void Update () {

        
        inputDirection = Vector3.zero;
        if (input.RHoldRight())
        {
            inputDirection += Vector3.right;
        }
        if (input.RHoldLeft())
        {
            inputDirection += Vector3.left;
        }
        if (input.RHoldUp())
        {
            inputDirection += Vector3.forward;
        }
        if (input.RHoldDown())
        {
            inputDirection += Vector3.back;
        }
        if (inputDirection != Vector3.zero)
        {
            // *** MOVE BASED ON INPUT DIRECTION ****
            //
            inputDirection.Normalize();
            /*if (true)
            {
                inputDirection = Camera.main.transform.TransformDirection(inputDirection);
                inputDirection.y = 0.0f;
            }*/

        }
        else
        {
            
        }

        print("Switch Kick inputDirection : " + inputDirection);

        hipFacing.bodyForward.y = inputDirection.x * switchSpeed;


    }
}
