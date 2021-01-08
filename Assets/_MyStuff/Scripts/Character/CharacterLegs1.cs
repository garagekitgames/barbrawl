using UnityEngine;
using System.Collections;

public class CharacterLegs1 : MonoBehaviour
{

    protected float legsCounter = 0;
    protected int legIndex = 0;
    protected int altlegIndex = 1;
    public Rigidbody[] feet = new Rigidbody[2];
    public Rigidbody[] legs = new Rigidbody[2];
    public Rigidbody[] thighs = new Rigidbody[2];
    public Rigidbody[] shins = new Rigidbody[2];
    public CharacterMaintainHeight[] legHeights = new CharacterMaintainHeight[2];
    public Rigidbody chestBody;
    //
    public float legRate = 0.4f;
    public float legRateIncreaseByVelocity = 0.1f;
    //
    public float liftForce = 80;
    public float holdDownForce = 500;
    public float moveForwardForce = 150;
    public float inFrontVelocityM = 0.4f;
    public float chestBendDownForce = 0;

    //public float initialholdDownForce; 
    //
    public bool walking = false;
    //
    public PlayerController1 pControl;
    public Vector3 inputDirection;
    
   
    public void StopWalking()
    {
        legHeights[legIndex].enabled = false;
        legHeights[altlegIndex].enabled = false;
        //
        walking = false;
        //
        legsCounter = legRate * 0.99f;
    }
    //
    public void StartWalking()
    {
        TakeStep();
        walking = true;
    }
    void Start()
    {
        StopWalking();
        foreach (Rigidbody r in legs) r.maxAngularVelocity = 40;
        foreach (Rigidbody r in feet) r.maxAngularVelocity = 40;
        foreach (Rigidbody r in thighs) r.maxAngularVelocity = 40;
        foreach (Rigidbody r in shins) r.maxAngularVelocity = 40;
        pControl = this.GetComponent<PlayerController1>();

    }

    void ConvertMoveInputAndPassItToAnimator(Vector3 moveInput)
    {
        //Convert the move input from world positions to local positions so that they have the correct values
        //depending on where we look
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        localMove.Normalize();
        float turnAmount = localMove.y;
        float forwardAmount = localMove.z;

        if (turnAmount != 0)
            turnAmount *= 2;

       // moveDirection = new Vector3(forwardAmount, 0, turnAmount);

        //print("Forward : " + forwardAmount);
        // print("Sideways : " + turnAmount);
        // forward = forwardAmount;
        //sideways = turnAmount;


        // anim.SetBool("LockOn", true);
        // anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        // anim.SetFloat("Sideways", turnAmount, 0.1f, Time.deltaTime);

    }
    void FixedUpdate()
    {

        ConvertMoveInputAndPassItToAnimator(pControl.inputDirection);

        // *************  I FIND APPLYING FORCES IN FIXED UPDATE TO BE MORE RELIABLE THAN IN UPDATE ****
        //
        //
        if (walking)
        {
            // ********************** THIS USES COUNTERS AND VARIOUS FORCES ON DIFFERENT PARTS OF THE LEGS TO PERFORM A GOOSE STEP *********
            //
            Vector3 horizontalVelocity = pControl.inputDirection;
            /*print("horizontalVelocity : " + horizontalVelocity);
            print("moveDirection : " + moveDirection);
            print("pcntrl.inputDirection : " + pcntrl.inputDirection);*/
            /* if (moveDirection.x > 0)
             {
                  horizontalVelocity = chestBody.transform.forward;
             }
             if(moveDirection.x < 0)
             {
                  horizontalVelocity = -chestBody.transform.forward;
             }
             if (moveDirection.z > 0)
             {
                 horizontalVelocity = chestBody.transform.right;
             }
             if (moveDirection.z < 0)
             {
                 horizontalVelocity = -chestBody.transform.right;
             }*/


            horizontalVelocity.y = 0;
            //
            float speed = chestBody.velocity.magnitude;
            horizontalVelocity.Normalize();
            //
            //
            legsCounter += Time.deltaTime * (1 + speed * legRateIncreaseByVelocity); // *** THE GOOSESTEP IS FASTER THE FASTER THE BODY TRAVELS ***
            //
            if (legsCounter >= legRate)
            {
                //
                TakeStep();  // ** CHANGE LEG ***
                //
            }
            //
            if (legsCounter > legRate * 0.75f)
            {
                // ***********************  END OF STEP, PLACING FOOT BACK ON THE GROUND ********
                //
                legs[legIndex].AddForce(Vector3.up * liftForce * Time.deltaTime, ForceMode.Impulse);
                //
                float inFrontM = Mathf.Clamp01(speed * inFrontVelocityM + 0.75f);
                //
                legs[legIndex].AddForce(horizontalVelocity * moveForwardForce * inFrontM * Time.deltaTime, ForceMode.Impulse);
                legs[altlegIndex].AddForce(-horizontalVelocity * moveForwardForce * inFrontM * Time.deltaTime, ForceMode.Impulse);
                //
                legs[legIndex].AddForce(-Vector3.up * holdDownForce * 0.5f * Time.deltaTime, ForceMode.Impulse);
                //
                legs[altlegIndex].AddForce(-Vector3.up * holdDownForce * Time.deltaTime, ForceMode.Impulse);
                //
              //  chestBody.AddForceAtPosition((chestBody.transform.forward - Vector3.up * 2) * chestBendDownForce * 0.66f * Time.deltaTime, chestBody.transform.TransformPoint(Vector3.right * 2), ForceMode.Impulse);
              //  chestBody.AddForceAtPosition((chestBody.transform.forward - Vector3.up * 2) * -chestBendDownForce * 0.5f * Time.deltaTime, chestBody.transform.TransformPoint(Vector3.right * -2), ForceMode.Impulse);

            }
            else if (legsCounter > legRate * 0.5f)
            {
                // ***********************  3rd PHASE OF STEP, STRAIGHTENING LEG OUT IN FRONT ********
                //
                feet[legIndex].AddForce(horizontalVelocity * moveForwardForce * 0.4f * Time.deltaTime, ForceMode.Impulse);
                feet[altlegIndex].AddForce(-horizontalVelocity * moveForwardForce * 0.4f * Time.deltaTime, ForceMode.Impulse);
                //
               // chestBody.AddForceAtPosition(-Vector3.up * liftForce * 0.1f * Time.deltaTime, legs[legIndex].transform.position, ForceMode.Impulse);
                //
                thighs[legIndex].AddForceAtPosition(((horizontalVelocity + Vector3.up * 0.5f) * liftForce * 2) * Time.deltaTime, legs[legIndex].transform.TransformPoint(Vector3.left * 2), ForceMode.Impulse);
                thighs[legIndex].AddForceAtPosition(((-horizontalVelocity + Vector3.up * 0.5f) * liftForce * 2) * Time.deltaTime, legs[legIndex].transform.TransformPoint(Vector3.right * 2), ForceMode.Impulse);
                //
                shins[legIndex].AddForceAtPosition(horizontalVelocity * liftForce * Time.deltaTime * 3, legs[legIndex].transform.TransformPoint(Vector3.left * 2), ForceMode.Impulse);
                shins[legIndex].AddForceAtPosition(-horizontalVelocity * liftForce * Time.deltaTime * 3, legs[legIndex].transform.TransformPoint(Vector3.right * 2), ForceMode.Impulse);
                //
                legs[altlegIndex].AddForce(-Vector3.up * holdDownForce * Time.deltaTime, ForceMode.Impulse);
                //
               // chestBody.AddForceAtPosition(-horizontalVelocity * moveForwardForce * 1 * Time.deltaTime, chestBody.transform.TransformPoint(Vector3.right * 1.5f), ForceMode.Impulse);
                shins[legIndex].AddForceAtPosition(horizontalVelocity * moveForwardForce * 1 * Time.deltaTime, legs[legIndex].transform.TransformPoint(Vector3.left * 2), ForceMode.Impulse);
                //
                if (legsCounter > legRate * 0.66f)
                {
                    // *** ABOUT TO PLANT FOOT ***
                    //
                    if (chestBody.transform.InverseTransformPoint(feet[legIndex].transform.position).z < 0)
                    {
                        // **** FOOT BEHIND CHEST ****
                        //
                        //legsCounter = legRate * 0.66f; // **** DON'T PROGRESS YET ****** HOLD FOOTSTEP ****
                    }
                }
            }
            else if (legsCounter > legRate * 0.25f)
            {
                // ***********************  2nd PHASE OF STEP, BENDING THE KNEE AND LIFTING THIGH ********
                //
                feet[legIndex].AddForce(horizontalVelocity * moveForwardForce * 0.4f * Time.deltaTime, ForceMode.Impulse);
                feet[altlegIndex].AddForce(-horizontalVelocity * moveForwardForce * 0.4f * Time.deltaTime, ForceMode.Impulse);
                //
               // chestBody.AddForceAtPosition(Vector3.up * liftForce * 0.1f * Time.deltaTime, legs[legIndex].transform.position, ForceMode.Impulse);
                thighs[legIndex].AddForceAtPosition(((horizontalVelocity + Vector3.up * 0.5f) * liftForce * 2) * Time.deltaTime, legs[legIndex].transform.TransformPoint(Vector3.left * 2), ForceMode.Impulse);
                thighs[legIndex].AddForceAtPosition(((-horizontalVelocity - Vector3.up * 0.5f) * liftForce * 2) * Time.deltaTime, legs[legIndex].transform.TransformPoint(Vector3.right * 2), ForceMode.Impulse);
                //
                shins[legIndex].AddForceAtPosition(-horizontalVelocity * liftForce * Time.deltaTime, legs[legIndex].transform.TransformPoint(Vector3.left * 2), ForceMode.Impulse);
                shins[legIndex].AddForceAtPosition(horizontalVelocity * liftForce * Time.deltaTime, legs[legIndex].transform.TransformPoint(Vector3.right * 2), ForceMode.Impulse);
                //Vector3.right
                legs[altlegIndex].AddForce(-Vector3.up * holdDownForce * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                // ***********************  BEGINNING OF STEP, LIFTING NEW FOOT OFF THE GROUND ********
                //
                feet[legIndex].AddForce(Vector3.up * liftForce * Time.deltaTime, ForceMode.Impulse);
                // 
              // chestBody.AddForceAtPosition(-Vector3.up * liftForce * 0.1f * Time.deltaTime, legs[legIndex].transform.position, ForceMode.Impulse);
                //
                //
                thighs[legIndex].AddForceAtPosition(((horizontalVelocity) * liftForce * 2) * Time.deltaTime, legs[legIndex].transform.TransformPoint(Vector3.left * 2), ForceMode.Impulse);
                thighs[legIndex].AddForceAtPosition(((-horizontalVelocity) * liftForce * 2) * Time.deltaTime, legs[legIndex].transform.TransformPoint(Vector3.right * 2), ForceMode.Impulse);
                //
                legs[legIndex].AddForce(horizontalVelocity * moveForwardForce * 1 * Time.deltaTime, ForceMode.Impulse);
                legs[altlegIndex].AddForce(-horizontalVelocity * moveForwardForce * 1 * Time.deltaTime, ForceMode.Impulse);
                // 
                feet[altlegIndex].AddForce(-Vector3.up * holdDownForce * Time.deltaTime, ForceMode.Impulse);
            }
            //
        }
        else
        {
            // *****************  NOT WALKING - HOLD THE FEET TO THE GROUND ***********
            //
            legs[legIndex].AddForce(-Vector3.up * holdDownForce * Time.deltaTime, ForceMode.Impulse);
            legs[altlegIndex].AddForce(-Vector3.up * holdDownForce * Time.deltaTime, ForceMode.Impulse);
        }
        //
    }

    private void TakeStep()
    {
        // *** CHANGES THE LEG ***
        //
        legIndex = (legIndex + 1) % 2;
        //
        altlegIndex = 1 - legIndex;
        //
        legsCounter -= legRate;
        //
        legHeights[legIndex].enabled = true;
        legHeights[altlegIndex].enabled = false;
    }
}
