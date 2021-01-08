using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTest : MonoBehaviour
{

    public Rigidbody[] feet;
    public Rigidbody[] thigh;
    public Rigidbody[] leg;
    public Rigidbody[] hands;
    public Rigidbody hip;
    public Rigidbody chest;

    public bool kickAnticipation;
    public float kickCounter = 0;
    public float kickDelay = 1f;

    public AnimationCurve curve;

    public float bouncePower = 10f;

    public Transform handTarget;

    public Vector3 inputDirection;
    public CharacterInput input;
    public float swordPower;

    // Use this for initialization
    void Start()
    {

        kickAnticipation = false;
        input = GetComponent<CharacterInput>();

    }
    void Update()
    {

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

            handTarget.transform.localPosition = new Vector3(Input.GetAxisRaw("R_XAxis_" + (input.controllerID+1)), -Input.GetAxisRaw("R_YAxis_" + (input.controllerID+1)), 0);
            print("Right Stick Value : " + new Vector3(Input.GetAxisRaw("R_XAxis_" + (input.controllerID+1)), -Input.GetAxisRaw("R_YAxis_" + (input.controllerID+1)), 0));

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
            handTarget.transform.localPosition = Vector3.zero;
            //
            /*if (legs.walking)
            {
                legs.StopWalking();
            }*/
        }


       /* Rup = Rup || Input.GetAxisRaw("R_YAxis" + input.controllerID) < -0.25f;
        Rdown = Rdown || Input.GetAxisRaw("R_YAxis" + input.controllerID) > 0.25f;
        Rleft = Rleft || Input.GetAxisRaw("R_XAxis" + input.controllerID) < -0.25f;
        Rright = Rright || Input.GetAxisRaw("R_XAxis" + input.controllerID) > 0.25f;*/

       

        /* if (Input.GetKey(KeyCode.W))
         {
             StartKickAnticipation();
         }

         if (kickAnticipation)
         {
             //***********************************  CROUCHING BEFORE JUMP **********************
             //
             kickCounter += Time.deltaTime;
             if (kickCounter >= kickDelay)
             {
                 Kick();
             }
         }*/



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 a = (handTarget.transform.position - hands[0].transform.position).normalized;
        hands[0].AddForce((a * swordPower) * Time.deltaTime, ForceMode.VelocityChange);
        //hands[1].AddForce(a * 200f * Time.deltaTime, ForceMode.VelocityChange);
        // chest.AddForceAtPosition((chest.transform.forward) * 20 * Time.deltaTime, chest.transform.TransformPoint(Vector3.down * 2), ForceMode.Impulse);


        //Animation Curve force
        /*
                kickCounter += Time.deltaTime;
                if (kickCounter >= 1)
                {
                    kickCounter = 0;


                }

                float y = this.curve.Evaluate(kickCounter);

                print("y value : " + y);
                print("kickCounter value : " + kickCounter);
                hip.AddForce(new Vector3(0, 1, 0) * y * bouncePower, ForceMode.VelocityChange);
                */

        /* if (Input.GetKey(KeyCode.W))
         {



         }
         if (Input.GetKey(KeyCode.A))
         {
             foreach (Rigidbody r in hands)
             {
                // r.AddForceAtPosition((r.transform.forward + Vector3.up) * 10 * Time.deltaTime, r.transform.TransformPoint(Vector3.right * 2), ForceMode.Impulse);
                 r.AddForceAtPosition((r.transform.forward) * 20 * Time.deltaTime, r.transform.TransformPoint(Vector3.down * 2), ForceMode.Impulse);

             }

         }*/
    }

    private void StartKickAnticipation()
    {
        // ***********************  CROUCH A BIT UNTIL THE ACTUAL JUMP *******
        kickAnticipation = true;

        kickCounter = 0;
        foreach (Rigidbody r in feet)
        {
            //  r.AddForce((r.transform.forward * -1 + Vector3.up) * 5 * Time.deltaTime, ForceMode.Impulse);
        }
        foreach (Rigidbody r in thigh)
        {
            // r.AddForce((r.transform.forward  + Vector3.up) * 30 * Time.deltaTime, ForceMode.Impulse);
        }
        foreach (Rigidbody r in leg)
        {
            // r.AddForce((r.transform.forward * -1 + Vector3.up) * 12 * Time.deltaTime, ForceMode.Impulse);
        }

        feet[0].AddForce((feet[0].transform.forward + Vector3.up) * 40 * Time.deltaTime, ForceMode.Impulse);

        thigh[0].AddForceAtPosition((thigh[0].transform.forward + Vector3.up) * 10 * Time.deltaTime, feet[0].transform.TransformPoint(Vector3.down * 2), ForceMode.Impulse);
        thigh[0].AddForceAtPosition((thigh[0].transform.forward * -1 - Vector3.up) * 10 * Time.deltaTime, feet[0].transform.TransformPoint(Vector3.up * 2), ForceMode.Impulse);


        leg[0].AddForceAtPosition((leg[0].transform.forward) * 20 * Time.deltaTime, feet[0].transform.TransformPoint(Vector3.down * 2), ForceMode.Impulse);
        leg[0].AddForceAtPosition((leg[0].transform.forward * -1) * 20 * Time.deltaTime, feet[0].transform.TransformPoint(Vector3.up * 2), ForceMode.Impulse);


    }

    private void Kick()
    {
        // ***********************  CROUCH A BIT UNTIL THE ACTUAL JUMP *******
        // kickAnticipation = true;

        // kickCounter = 0;
        /*foreach (Rigidbody r in feet)
        {
            r.AddForce((r.transform.forward + Vector3.up) * 5 * Time.deltaTime, ForceMode.Impulse);
        }
        foreach (Rigidbody r in thigh)
        {
            r.AddForce((r.transform.forward + Vector3.up) * 30 * Time.deltaTime, ForceMode.Impulse);
        }
        foreach (Rigidbody r in leg)
        {
            r.AddForce((r.transform.forward  + Vector3.up) * 40 * Time.deltaTime, ForceMode.Impulse);
        }*/

        feet[0].AddForce((Vector3.up) * 40 * Time.deltaTime, ForceMode.Impulse);

        thigh[0].AddForceAtPosition((thigh[0].transform.forward + Vector3.up) * 30 * Time.deltaTime, feet[0].transform.TransformPoint(Vector3.down * 2), ForceMode.Impulse);
        thigh[0].AddForceAtPosition((thigh[0].transform.forward * -1 - Vector3.up) * 10 * Time.deltaTime, feet[0].transform.TransformPoint(Vector3.up * 2), ForceMode.Impulse);


        leg[0].AddForceAtPosition((leg[0].transform.forward) * 60 * Time.deltaTime, feet[0].transform.TransformPoint(Vector3.down * 2), ForceMode.Impulse);
        leg[0].AddForceAtPosition((leg[0].transform.forward * -1) * 20 * Time.deltaTime, feet[0].transform.TransformPoint(Vector3.up * 2), ForceMode.Impulse);

        kickCounter = 0;
        kickAnticipation = false;

    }
}
