using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

    public Rigidbody chestBody;

    public Vector3 inputDirection;
    public CharacterInput input;
    public CharacterFaceDirection faceDirection;
    public CharacterFaceDirection hipFaceDirection;
    protected Vector3 currentFacing = Vector3.zero;


    public float jumpCounter = 0;
    public float jumpDelay = 0.2f;
    public float bendKneeDelay = 0.4f;
    public float airTimeDelay = 0.55f;
    protected bool jumpAnticipation = false;
    public bool inAir = false;
    public float jumpForce = 200;
    public float jumpForwardForce = 150;
    public float facePlantForce = 30;
    protected float facePlantM = 1;
    protected float getUpCounter = 0;
    public bool canJump = true;
    public bool cameraBasedMovement = true;
    public bool applyDrag = true;


    public float dragMultiplier = 20f;
    public float dragAdded = 10f;
    public float reduceDragValue = 1f;


    public CharacterLegsSimple legs;
    //public CharacterUpright1 chestUpright;
    public CharacterMaintainHeight maintainHeight;
    //public CharacterFaceDirection faceDirection;

    public float maintainHeightStanding = 0.9f;
    public float maintainHeightCrouching = 0.6f;
    public Rigidbody[] feetBodies = new Rigidbody[2];

    public PlayerController1 pContrl;
    public QuadraticDrag qDrag;


    public Rigidbody[] thighsBody = new Rigidbody[2];
    public ConfigurableJoint[] thighsJoint = new ConfigurableJoint[2];

    public Rigidbody[] legsBody = new Rigidbody[2];
    public ConfigurableJoint[] legsJoint = new ConfigurableJoint[2];

    public Rigidbody[] feetBody = new Rigidbody[2];
    public ConfigurableJoint[] feetJoint = new ConfigurableJoint[2];

    public Kicking kick;

    public bool kickedInAir = false;

    // Use this for initialization
    void Start () {
        pContrl = GetComponent<PlayerController1>();

        qDrag = GetComponent<QuadraticDrag>();
        kick = GetComponent<Kicking>();

    }

    void CheckJumpInput()
    {
        if (input.Jump() && canJump)
        {
            StartJumpAnticipation();
        }

    }

    private void GetUpFromJump()
    {
        // ***********************  STAND UP AFTER BEING A RAGDOLL *******
        //
        /* foreach (CharacterMaintainHeight h in otherMaintainHeight)
         {
             h.enabled = true;
             h.desiredHeight = -3; // **** START ARMS ON GROUND AND THEN LERP THIS VALUE TO NORMAL HEIGHT ****
         }
         foreach (CharacterUpright1 h in otherUprights)
         {
             h.enabled = true;
         }*/
        //
        getUpCounter = 3; // ** JUST USED TO SETTLE THE ARMS ***
        //
        //
        // **** NEXT: REACTIVATE ALL THE OTHER COMPONENTS THAT MOVE THE LIMBS AND TORSO ****
        //
        inAir = false;
        maintainHeight.enabled = true;
        maintainHeight.desiredHeight = maintainHeightStanding;
        faceDirection.enabled = true;
        legs.enabled = true;
        qDrag.enabled = true;
        pContrl.speed = pContrl.startSpeed;
        
        /*if (chestUpright != null)
            chestUpright.enabled = true;*/
        //
        // *** DO A SMALL HOP UPWARD TO START GETTING UP ***
        //
        chestBody.AddForceAtPosition((chestBody.transform.forward * -1 + Vector3.up) * 20, chestBody.transform.TransformPoint(Vector3.up * 0.2f), ForceMode.Impulse);

        kickedInAir = false;
        for (int i = 0; i < 2; i++)
        {
           /* JointDrive x = thighsJoint[i].slerpDrive;
            x.positionDamper = 0;
            x.positionSpring = 0;
            x.maximumForce = 0;
            thighsJoint[i].slerpDrive = x;

            Vector3 jointValue = new Vector3(0, 0f, 0f);

            thighsJoint[i].targetAngularVelocity = jointValue;

            JointDrive x1 = legsJoint[i].slerpDrive;
            x1.positionDamper = 0;
            x1.positionSpring = 0;
            x1.maximumForce = 0;
            legsJoint[i].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(-0, 0f, 0f);

            legsJoint[i].targetAngularVelocity = jointValue1;
            */
            JointDrive x = thighsJoint[i].slerpDrive;
            x.positionDamper = 10;
            x.positionSpring = 100;
            x.maximumForce = 100;
            thighsJoint[i].slerpDrive = x;

            Vector3 jointValue = new Vector3(0, 0f, 0f);

            thighsJoint[i].targetAngularVelocity = jointValue;

            JointDrive x1 = legsJoint[i].slerpDrive;
            x1.positionDamper = 0.6f;
            x1.positionSpring = 50;
            x1.maximumForce = 50;
            legsJoint[i].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(-0, 0f, 0f);

            legsJoint[i].targetAngularVelocity = jointValue1;
        }
        canJump = true;
    }
    private void Jump2()
    {
        
        qDrag.enabled = false;
        // ***********************  ACTUALLY JUMP - Launch into the air *******
        //
        //
        // **** DISABLE SOME CONTROLLING COMPONENTS (the height maintaining script on the torso and upright forces on feet) ****
        //
        /*foreach (CharacterMaintainHeight h in otherMaintainHeight)
        {
            h.enabled = false;
        }
        foreach (CharacterUpright1 h in otherUprights)
        {
            h.enabled = false;
        }*/
            // **** LAUNCH INTO AIR HERE :
            //
            //Use the following to spin in the direction the user presses
            pContrl.speed = jumpForwardForce;
        chestBody.AddForce(Vector3.up * jumpForce + inputDirection * jumpForwardForce, ForceMode.Impulse);

        //chestBody.AddForce(Vector3.up * jumpForce + chestBody.transform.forward * jumpForwardForce, ForceMode.Impulse);
        //
        // **** NEXT: DISABLE ALL THE OTHER CONTROLLING COMPONENTS AND ESSENTIALLY BECOME A RAGDOLL ****
        //
        maintainHeight.enabled = false;
        jumpCounter = 0;
        jumpAnticipation = false;
        inAir = true;
        legs.enabled = false;
       /* if (chestUpright != null)
            chestUpright.enabled = false;*/
        //faceDirection.enabled = false;
        //
        // ****  SOMETIMES THE FACEPLANT IS GOING TO HAVE MORE FORCE ON IT, BECAUSE RANDOM STRENGTH FACEPLANTS ARE COOL ***
        //
        facePlantM = 0.9f + Random.value * 0.4f;
        
    }

    //
    private void StartJumpAnticipation()
    {
       
        // ***********************  CROUCH A BIT UNTIL THE ACTUAL JUMP *******
        canJump = false;
        legs.StopWalking();
        jumpAnticipation = true;
        maintainHeight.desiredHeight = maintainHeightCrouching;
        jumpCounter = 0;

       
       


    }

    private void KneeBend()
    {

        for (int i = 0; i < 2; i++)
        {
            JointDrive x = thighsJoint[i].slerpDrive;
            x.positionDamper = 100;
            x.positionSpring = 1000;
            x.maximumForce = 1000;
            thighsJoint[i].slerpDrive = x;

            Vector3 jointValue = new Vector3(10, 0f, 0f);

            thighsJoint[i].targetAngularVelocity = jointValue;

            JointDrive x1 = legsJoint[i].slerpDrive;
            x1.positionDamper = 100;
            x1.positionSpring = 1000;
            x1.maximumForce = 1000;
            legsJoint[i].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(-40, 0f, 0f);

            legsJoint[i].targetAngularVelocity = jointValue1;
        }




    }

    void Update()
    {
        CheckJumpInput();
        if (getUpCounter > 0)
        {
            getUpCounter -= Time.deltaTime;
            //
            // *****************  LIFT ARMS OFF OF THE GROUND SLOWLY WHEN GETTING UP ************
            //
            /*foreach (CharacterMaintainHeight h in otherMaintainHeight)
            {
                h.desiredHeight = Mathf.Lerp(h.desiredHeight, 0.2f, Time.deltaTime * 3);
            }*/
        }
        if (jumpAnticipation)
        {
            //***********************************  CROUCHING BEFORE JUMP **********************
            //
            jumpCounter += Time.deltaTime;
            if (jumpCounter >= jumpDelay)
            {
                Jump2();
            }
            
        }
        else if (inAir)
        {
            input.timerValue = 0.01f;
            //***********************************  AIR BORNE **********************
            //
            jumpCounter += Time.deltaTime;
            if (kick.leftKick || kick.rightKick )
            {
                kickedInAir = true;
            }
            
            if (jumpCounter >= bendKneeDelay && jumpCounter < airTimeDelay)
            {
                //if(!kick.leftKick && !kick.rightKick && !kick.leftWindUp && !kick.rightWindUp)
                if(!kickedInAir)
                    KneeBend();
            }
            if (jumpCounter >= airTimeDelay)
            {
                GetUpFromJump();
            }
            //
        }
    }
        // Update is called once per frame
        void FixedUpdate() {

        if (!inAir)
        {
            input.timerValue = 0.17f;
            pContrl.enableDrag = true;
           /* JointDrive x = thighsJoint[1].slerpDrive;
            x.positionDamper = 0;
            x.positionSpring = 0;
            x.maximumForce = 0;
            thighsJoint[1].slerpDrive = x;

            Vector3 jointValue = new Vector3(0, 0, 0);

            thighsJoint[1].targetAngularVelocity = jointValue;

            JointDrive x1 = legsJoint[1].slerpDrive;
            x1.positionDamper = 0;
            x1.positionSpring = 0;
            x1.maximumForce = 0;
            legsJoint[1].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(0, 0, 0);

            legsJoint[1].targetAngularVelocity = jointValue1;
            */

           

            // ****  APPLY DRAGS ****
            //
            /* if (applyDrag)
                 ApplyStandingAndWalkingDrag();*/
            //
            /* if (!jumpAnticipation)
             {
                 if (inputDirection != Vector3.zero)
                 {
                     // *********************  MOVE CHEST IN THE INPUT DIRECTION *******
                     //
                     // *** (THIS IS ZERO IN THE PROJECT BY DEFAULT, I PREFER HAVING THE LEGS PULL THE BODY FORWARD ***
                     //
                     chestBody.AddForceAtPosition(force * inputDirection * Time.deltaTime, chestBody.transform.TransformDirection(Vector3.forward * 2), ForceMode.Impulse);
                     //                   
                     //                    
                 }
             }*/

        }
        else if (inAir)
        {


            
            pContrl.enableDrag = false;
            //
            // *******************************************  TOWARDS END OF JUMP, FORCE A FACEPLANT *****
            //
            //if (jumpCounter > airTimeDelay * 0.15f && jumpCounter < airTimeDelay * 0.4f)
           // {
                //chestBody.AddForceAtPosition((chestBody.transform.forward + Vector3.down) * facePlantForce * facePlantM * Time.deltaTime, chestBody.transform.TransformPoint(Vector3.up * 2), ForceMode.Impulse);
                //
               // foreach (Rigidbody f in feetBodies)
               // {
                    //f.AddForce(Vector3.up * 10 * Time.deltaTime, ForceMode.Impulse);
                //}
            //}
        }
    }

    private void ApplyStandingAndWalkingDrag()
    {
        // ***********  APPLY DRAGS! **
        //
        // THIS, along with the powerful facing direction forces, ACTUALLY MAKES THE CHARACTERS LESS INTERACTIBLE, BECAUSE THEY CAN'T PUSH EACH OTHER MUCH *****
        // SOFTER FORCES CAN BE BETTER, BUT THOSE NEED MORE TWEEKING, IDEALLY JUST ENOUGH FORCE TO ACHIEVE THE EFFECT WITHOUT BECOMING LOCKED INTO THAT POSITION OR DIRECTION ***
        //
        if (inputDirection == Vector3.zero)
        {
            // ***** WHEN STANDING STILL, APPLY A DRAG BASED ON HOW FAST THE TORSO IS TRAVELLING ***
            //
            Vector3 horizontalVelocity = chestBody.velocity;
            horizontalVelocity.y = 0;
            //
            float speed = horizontalVelocity.magnitude;
            //
            chestBody.velocity *= (1 - (Mathf.Clamp(speed * dragMultiplier + dragAdded, 0, 50)) * reduceDragValue * Time.fixedDeltaTime);
        }
        else
        {
            // ***** APPLY A POWERFUL DRAG FORCE IF THE TORSO ISN'T TRAVELLING IN THE INPUT DIRECITON, ALLOWS FOR TIGHT TURNS ***
            //
            Vector3 horizontalVelocity = chestBody.velocity;
            horizontalVelocity.y = 0;
            //
            // print(horizontalVelocity);
            float m = 1 - (1 + Vector3.Dot(horizontalVelocity.normalized, inputDirection)) / 2f;
            chestBody.velocity *= (1 - (m * 100) * Time.fixedDeltaTime);
        }
        //
    }
}
