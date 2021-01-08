using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovement : MonoBehaviour
{

    // public Rigidbody chestBody;

    // public Vector3 inputDirection;
    // public CharacterInput input;
    // public CharacterFaceDirection faceDirection;

    // protected Vector3 currentFacing = Vector3.zero;

    public Rigidbody[] thighsBody = new Rigidbody[2];
    public ConfigurableJoint[] thighsJoint = new ConfigurableJoint[2];

    //public Transform[] feetBody;
    public Rigidbody[] feetBody;
    public ConfigurableJoint[] feetJoint;

    public PlayerController1 pcntrl;
    public float legMovementStrength = 15f;

    public float forward;
    public float sideways;

    /*public Transform leftFootTarget;
    public Transform rightFootTarget;*/

    public Transform[] feetTarget;
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
    void Start()
    {
        pcntrl = GetComponent<PlayerController1>();

        Time.fixedDeltaTime = fixedDeltaTime; // Set the physics loop update intervall
                                              //		Debug.Log("The script AnimFollow has set the fixedDeltaTime to " + fixedDeltaTime); // Remove this line if you don't need the "heads up"
        reciFixedDeltaTime = 1f / fixedDeltaTime; // Cache the reciprocal

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
       // print("Forward : " + forwardAmount);
        //print("Sideways : " + turnAmount);
        forward = forwardAmount;
        sideways = turnAmount;

    }

    // Update is called once per frame
    void Update()
    {

        ConvertMoveInputAndPassItToAnimator(pcntrl.inputDirection);


    }

    public static void PDControl(float P, float D, out Vector3 signal, Vector3 error, ref Vector3 lastError, float reciDeltaTime) // A PD controller
    {
        // theSignal = P * (theError + D * theDerivative) This is the implemented algorithm.
        signal = P * (error + D * (error - lastError) * reciDeltaTime);
        lastError = error;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < feetBody.Length ; i++)
        {
            rigidbodiesPosToCOM = Quaternion.Inverse(feetBody[i].transform.rotation) * (feetBody[i].worldCenterOfMass - feetBody[i].transform.position);

            // Force error
            Vector3 masterRigidTransformsWCOM = feetTarget[i].position + feetTarget[i].rotation * rigidbodiesPosToCOM;
            forceError = masterRigidTransformsWCOM - feetBody[i].GetComponent<Rigidbody>().worldCenterOfMass; // Doesn't work if collider is trigger
            totalForceError += forceError * forceErrorWeightProfile;


            PDControl(PForce * PForceProfile, DForce, out forceSignal, forceError, ref forceLastError, reciFixedDeltaTime);
            forceSignal = Vector3.ClampMagnitude(forceSignal, maxForce * maxForceProfile);
            feetBody[i].AddForce(forceSignal, ForceMode.VelocityChange);
        }






        /* Vector3 a = (leftFootTarget.transform.position - feetBody[0].transform.position).normalized;
         feetBody[0].AddForce((a * legMovementStrength) * Time.deltaTime, ForceMode.VelocityChange);

         Vector3 a1 = (rightFootTarget.transform.position - feetBody[1].transform.position).normalized;
         feetBody[1].AddForce((a1 * legMovementStrength) * Time.deltaTime, ForceMode.VelocityChange);
         */


        /*JointDrive x = thighsJoint[0].slerpDrive;
        x.positionDamper = 100f;
        x.positionSpring = 1000f;
        x.maximumForce = 1000f;
        thighsJoint[0].slerpDrive = x;

        Vector3 jointValue = new Vector3(Mathf.Abs(forward) * legMovementStrength, Mathf.Abs(sideways) * legMovementStrength, 0f);

        thighsJoint[0].targetAngularVelocity = jointValue;

        JointDrive x1 = thighsJoint[1].slerpDrive;
        x1.positionDamper = 100f;
        x1.positionSpring = 1000f;
        x1.maximumForce = 1000f;
        thighsJoint[1].slerpDrive = x1;

        Vector3 jointValue1 = new Vector3(-Mathf.Abs(forward) * legMovementStrength, -Mathf.Abs(sideways) * legMovementStrength, 0f);

        thighsJoint[1].targetAngularVelocity = jointValue1;
        */
        /* JointDrive x1 = thighsJoint[0].slerpDrive;
         x1.positionDamper = 100f;
         x1.positionSpring = 1000f;
         x1.maximumForce = 1000f;
         thighsJoint[0].slerpDrive = x1;

         Vector3 jointValue1 = new Vector3(-20f, 0f, 0f);

         thighsJoint[0].targetAngularVelocity = jointValue1;*/
    }
}
