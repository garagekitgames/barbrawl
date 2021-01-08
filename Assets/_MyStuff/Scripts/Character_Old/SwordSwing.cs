using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour {

    //public Rigidbody chestBody;
    //public ConfigurableJoint chestJoint;


    public Vector3 inputDirection;
    public CharacterInput input;

    public float swordPower = 20f;

    //public float fitnessSpring = 1000f;
   // public float fitnessDamper = 100f;
   // public float fitnessForce = 1000f;


   // public CharacterFaceDirection hipFacing;

    //public bool log = false;

   // public Vector3 testVector;

   // public Vector3 testVector2;

    public Transform swordTarget;

    public Rigidbody[] hands;


    public Vector3 rigidbodiesPosToCOM;

    Vector3 forceSignal;
    Vector3 forceError;
    Vector3 forceLastError = new Vector3();

    [HideInInspector] public Vector3 totalForceError; // Total world position error. a vector.
    public float forceErrorWeightProfile = 1f;

    //PD Controller values
    [Range(0f, .64f)] public float PTorque = .16f; // For all limbs Torque strength
    [Range(0f, 160f)] public float PForce = 30f;

    [Range(0f, .008f)] public float DTorque = .002f; // Derivative multiplier to PD controller
    [Range(0f, .064f)] public float DForce = .01f;

    [Range(0f, 100f)] public float maxForce = 100f;

    public float maxForceProfile = 0.2f;

    float reciFixedDeltaTime; // 1f / fixedDeltaTime
    //	public float[] PTorqueProfile = {20f, 30f, 10f, 30f, 10f, 30f, 30f, 30f, 10f, 30f, 10f}; // Per limb world space torque strength
    public float PForceProfile = 1f;
    public float fixedDeltaTime = 0.01f; // If you choose to go to longer times you need to lower PTorque, PLocalTorque and PForce or the system gets unstable. Can be done, longer time is better performance but worse mimicking of master.


    // Use this for initialization
    void Start () {
        input = GetComponent<CharacterInput>();

        Time.fixedDeltaTime = fixedDeltaTime; // Set the physics loop update intervall
                                              //		Debug.Log("The script AnimFollow has set the fixedDeltaTime to " + fixedDeltaTime); // Remove this line if you don't need the "heads up"
        reciFixedDeltaTime = 1f / fixedDeltaTime; // Cache the reciprocal
        //hipFacing = GetComponent<CharacterFaceDirection>();
    }

    void ConvertMoveInputAndPassItToAnimator(Vector3 moveInput)
    {
        //Convert the move input from world positions to local positions so that they have the correct values
        //depending on where we look
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        localMove.Normalize();
        float turnAmount = localMove.y;
        float forwardAmount = localMove.z;

        /* if (turnAmount != 0)
             turnAmount *= 2;

         */
       // if (log)
        //{
       //     print("Forward : " + forwardAmount);
        //    print("Sideways : " + turnAmount);
       //     print("testVector : " + testVector);
       //     print("inputdirection : " + inputDirection);
       //     print("hipFacing : " + hipFacing.facingDirection);
       // }

       // testVector2 = new Vector3(-forwardAmount, turnAmount, 0f);
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

            swordTarget.transform.localPosition = new Vector3(Input.GetAxisRaw("R_XAxis_" + (input.controllerID + 1)), -Input.GetAxisRaw("R_YAxis_" + (input.controllerID + 1)), 0);
            //For blender char
            //swordTarget.transform.localPosition = new Vector3(Input.GetAxisRaw("R_YAxis_" + (input.controllerID + 1)), Input.GetAxisRaw("R_XAxis_" + (input.controllerID + 1)), 0);
            print("Right Stick Value : " + new Vector3(Input.GetAxisRaw("R_XAxis_" + (input.controllerID + 1)), -Input.GetAxisRaw("R_YAxis_" + (input.controllerID + 1)), 0));

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
            swordTarget.transform.localPosition = Vector3.zero;
            //
            /*if (legs.walking)
            {
                legs.StopWalking();
            }*/
        }


    }


    public static void PDControl(float P, float D, out Vector3 signal, Vector3 error, ref Vector3 lastError, float reciDeltaTime) // A PD controller
    {
        // theSignal = P * (theError + D * theDerivative) This is the implemented algorithm.
        signal = P * (error + D * (error - lastError) * reciDeltaTime);
        lastError = error;
    }


    private void FixedUpdate()
    {
        /*Vector3 a = (swordTarget.transform.position - hands[0].transform.position).normalized;
        hands[0].AddForce((a * swordPower) * Time.deltaTime, ForceMode.VelocityChange);

        Vector3 a1 = (swordTarget.transform.position - hands[1].transform.position).normalized;
        hands[1].AddForce((a1 * swordPower) * Time.deltaTime, ForceMode.VelocityChange);
        */

        for (int i = 0; i < hands.Length; i++)
        {
            rigidbodiesPosToCOM = Quaternion.Inverse(hands[i].transform.rotation) * (hands[i].worldCenterOfMass - hands[i].transform.position);

            // Force error
            Vector3 masterRigidTransformsWCOM = swordTarget.position + swordTarget.rotation * rigidbodiesPosToCOM;
            forceError = masterRigidTransformsWCOM - hands[i].GetComponent<Rigidbody>().worldCenterOfMass; // Doesn't work if collider is trigger
            totalForceError += forceError * forceErrorWeightProfile;


            PDControl(PForce * PForceProfile, DForce, out forceSignal, forceError, ref forceLastError, reciFixedDeltaTime);
            forceSignal = Vector3.ClampMagnitude(forceSignal, maxForce * maxForceProfile);
            hands[i].AddForce(forceSignal, ForceMode.VelocityChange);
        }


        // Force error
        

    }
}
