using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour {
    public Rigidbody chest;

    public Vector3 inputDirection;
    public CharacterInput input;
    public float dodgeSpeed;

    public Transform dodgeTarget;

    public Vector3 torqueTest;

    public Vector3 testVector;
    // Use this for initialization
    void Start () {
        input = GetComponent<CharacterInput>();
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
            //               
            //Vector3.cl
            // handTarget.transform.localPosition = new Vector3(inputDirection.x, inputDirection.z, inputDirection.y);
            // print("Right Stick Value : " + inputDirection);

            dodgeTarget.transform.localPosition = new Vector3(0, Input.GetAxisRaw("R_XAxis_" + (input.controllerID + 1)), -Input.GetAxisRaw("R_YAxis_" + (input.controllerID + 1)));
            print("dodgeTarget Right Stick Value : " + new Vector3(0, Input.GetAxisRaw("R_XAxis_" + (input.controllerID + 1)), -Input.GetAxisRaw("R_YAxis_" + (input.controllerID + 1))));

            //
            /* if (!legs.walking)
             {
                 legs.StartWalking();
             }*/
            //

            //
        }
        else
        {
            // *** STAND STILL WHEN ZERO INPUT ****
            //
            dodgeTarget.transform.localPosition = Vector3.zero;
            //
            /*if (legs.walking)
            {
                legs.StopWalking();
            }*/
        }


    }


    void FixedUpdate()
    {/*
        Vector3 x = Vector3.Cross(chest.transform.position.normalized, dodgeTarget.transform.position.normalized);
        float theta = Mathf.Asin(x.magnitude);
        Vector3 w = x.normalized * theta / Time.fixedDeltaTime;

        Quaternion q = chest.transform.rotation * chest.inertiaTensorRotation;
        Vector3 T = q * Vector3.Scale(chest.inertiaTensor, (Quaternion.Inverse(q) * w));
        chest.AddTorque(T, ForceMode.Impulse);
        

        chest.AddTorque(torqueTest, ForceMode.Impulse);
        */

        chest.AddForceAtPosition(dodgeSpeed * ((-1*chest.transform.forward )+ Vector3.down) * Time.deltaTime, chest.transform.TransformDirection(testVector * 2), ForceMode.VelocityChange);

        //Adding force
        /*Vector3 a = (dodgeTarget.transform.position - chest.transform.position).normalized;
         chest.AddForce((a * dodgeSpeed) * Time.deltaTime, ForceMode.VelocityChange);
         */

        // Quaternion rotation = targetRotation * Quaternion.Inverse(rigidbody.rotation);
        //chest.AddTorque(rotation.x / Time.fixedDeltaTime, rotation.y / Time.fixedDeltaTime, rotation.z / Time.fixedDeltaTime, ForceMode.VelocityChange);


    }

}
