using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punching : MonoBehaviour {

    public Rigidbody chestBody;

    public PlayerController1 pContrl; 


    public Rigidbody[] armsBody = new Rigidbody[2];
    public ConfigurableJoint[] armsJoint = new ConfigurableJoint[2];

    public Rigidbody[] forearmsBody = new Rigidbody[2];
    public ConfigurableJoint[] forearmsJoint = new ConfigurableJoint[2];

    public Rigidbody[] handsBody = new Rigidbody[2];
    public ConfigurableJoint[] handsJoint = new ConfigurableJoint[2];

    public CharacterInput input;

    public float leftbuttonClickTimer = 0f;
    public float rightbuttonClickTimer = 0f;

    public bool leftBlock = false;
    public bool leftpunch = false;
    public bool leftThrow = false;
    public bool leftWindUp = false;
    

    public bool rightBlock = false;
    public bool rightpunch = false;
    public bool rightThrow = false;
    public bool rightWindUp = false;

    private float leftpunchTimer = 0.0f;

    private float rightpunchTimer = 0.0f;

    [Range(0, 1)]
    public float punchForceApplyPercent = 0.5f;// percentage of the punch duration the force is applied, shorter duration would mean less tracking
    [Range(0, 5)]
    public float punchPowerIncreaseRate = 2;//2 to 5 works good
    [Range(0f, 0.9f)]
    public float accuracyReduceFactor = 1f; //less value is better, percentage of 

    public float punchSpeed = 0.5f;
    public float punchPower = 1300f;
    public float startPunchPower = 0f;
    public float maxPunchPower = 3500;
    public float minPunchPower = 1300;

    public Vector3 target;

    public Vector3 leftPunchTarget = Vector3.zero;
    public Vector3 rightPunchTarget = Vector3.zero;
    public bool windupDrag = true;

    // Use this for initialization
    void Start () {
        startPunchPower = punchPower;
        input = GetComponent<CharacterInput>();
        pContrl = GetComponent<PlayerController1>();

        JointDrive x = armsJoint[0].slerpDrive;
        x.positionDamper = 100f;
        x.positionSpring = 1000f;
        x.maximumForce = 1000f;

        armsJoint[0].slerpDrive = x;

        Vector3 jointValue = new Vector3(-15f, -10f, 5f);

        armsJoint[0].targetAngularVelocity = jointValue;

        JointDrive x1 = forearmsJoint[0].slerpDrive;
        x1.positionDamper = 100f;
        x1.positionSpring = 1000f;
        x1.maximumForce = 1000f;
        forearmsJoint[0].slerpDrive = x1;

        Vector3 jointValue1 = new Vector3(-40f, 4f, 6f);

        forearmsJoint[0].targetAngularVelocity = jointValue1;

        JointDrive x2 = armsJoint[1].slerpDrive;
        x2.positionDamper = 100f;
        x2.positionSpring = 1000f;
        x2.maximumForce = 1000f;
        armsJoint[1].slerpDrive = x2;

        Vector3 jointValue2 = new Vector3(15f, 10f, 5f);

        armsJoint[1].targetAngularVelocity = jointValue2;

        JointDrive x3 = forearmsJoint[1].slerpDrive;
        x3.positionDamper = 100f;
        x3.positionSpring = 1000f;
        x3.maximumForce = 1000f;
        forearmsJoint[1].slerpDrive = x3;

        Vector3 jointValue3 = new Vector3(40f, -4f, 6f);

        forearmsJoint[1].targetAngularVelocity = jointValue3;

    }
	
	// Update is called once per frame
	void Update () {

        CheckInput();

        target = pContrl.target;



    }

    private void CheckInput()
    {
        //LEft and Right Punch Grab and Throw 
        if (input.PressLeftPunch())
        {
            leftbuttonClickTimer = 0f;
            leftWindUp = false;

        }

        else if (input.HoldLeftPunch())
        {
            leftbuttonClickTimer += Time.deltaTime;
            if (leftWindUp == false && !leftpunch)    // if the button is pressed for more than 0.2 seconds grab
            {
                leftWindUp = true;

            }
           

        }

        else if (input.ReleaseLeftPunch())
        {
            //print("Key Released");
            if (!leftpunch) // as long as key is held down increase time, this records how long the key is held down
            {
                punchPower = Mathf.Clamp(startPunchPower * (1 + leftbuttonClickTimer * punchPowerIncreaseRate), minPunchPower, maxPunchPower);

                leftpunch = true;
                leftbuttonClickTimer = 0f;
            }

            leftWindUp = false;
            /*JointDrive x = armsJoint[0].slerpDrive;
            x.positionDamper = 100f;
            x.positionSpring = 1000f;
            x.maximumForce = 1000f;
            armsJoint[0].slerpDrive = x;

            Vector3 jointValue = new Vector3(-15f, -10f, 5f);

            armsJoint[0].targetAngularVelocity = jointValue;

            JointDrive x1 = forearmsJoint[0].slerpDrive;
            x1.positionDamper = 100f;
            x1.positionSpring = 1000f;
            x1.maximumForce = 1000f;
            forearmsJoint[0].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(-40f, 4f, 6f);

            forearmsJoint[0].targetAngularVelocity = jointValue1;*/

        }




        //////////////////

        if (input.PressRightPunch())
        {
            // print("Just Pressed Key");

            rightbuttonClickTimer = 0f;
            rightWindUp = false;

        }

        else if (input.HoldRightPunch())
        {

            rightbuttonClickTimer += Time.deltaTime;
            if (rightWindUp == false && !rightpunch)    // if the button is pressed for more than 0.2 seconds grab
            {
                rightWindUp = true;

            }
            

        }

        else if (input.ReleaseRightPunch())
        {

            if (!rightpunch) // as long as key is held down increase time, this records how long the key is held down
            {
                punchPower = Mathf.Clamp(startPunchPower * (1 + rightbuttonClickTimer * punchPowerIncreaseRate), minPunchPower, maxPunchPower);

                rightpunch = true;
                rightbuttonClickTimer = 0f;
            }

            rightWindUp = false;

/*
            JointDrive x = armsJoint[1].slerpDrive;
            x.positionDamper = 100f;
            x.positionSpring = 1000f;
            x.maximumForce = 1000f;
            armsJoint[1].slerpDrive = x;

            Vector3 jointValue = new Vector3(15f, 10f, 5f);

            armsJoint[1].targetAngularVelocity = jointValue;

            JointDrive x1 = forearmsJoint[1].slerpDrive;
            x1.positionDamper = 100f;
            x1.positionSpring = 1000f;
            x1.maximumForce = 1000f;
            forearmsJoint[1].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(40f, -4f, 6f);

            forearmsJoint[1].targetAngularVelocity = jointValue1;*/

        }


    }

    private void FixedUpdate()
    {
       
        /*else
        {
            JointDrive x = armsJoint[0].slerpDrive;
            x.positionDamper = 100f;
            x.positionSpring = 1000f;
            x.maximumForce = 1000f;
            armsJoint[0].slerpDrive = x;

            Vector3 jointValue = new Vector3(-15f, -10f, 5f);

            armsJoint[0].targetAngularVelocity = jointValue;

            JointDrive x1 = forearmsJoint[0].slerpDrive;
            x1.positionDamper = 100f;
            x1.positionSpring = 1000f;
            x1.maximumForce = 1000f;
            forearmsJoint[0].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(-40f, 4f, 6f);

            forearmsJoint[0].targetAngularVelocity = jointValue1;

        }*/

       /* if (rightBlock)
        {


            JointDrive x = armsJoint[1].slerpDrive;
            x.positionDamper = 100f;
            x.positionSpring = 1000f;
            x.maximumForce = 1000f;
            armsJoint[1].slerpDrive = x;

            Vector3 jointValue = new Vector3(30f, 15f, 11f);

            armsJoint[1].targetAngularVelocity = jointValue;

            JointDrive x1 = forearmsJoint[1].slerpDrive;
            x1.positionDamper = 100f;
            x1.positionSpring = 1000f;
            x1.maximumForce = 1000f;
            forearmsJoint[1].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(40f, -50f, 6f);

            forearmsJoint[1].targetAngularVelocity = jointValue1;
            



        }*/
        /*else
        {
            JointDrive x = armsJoint[1].slerpDrive;
            x.positionDamper = 100f;
            x.positionSpring = 1000f;
            x.maximumForce = 1000f;
            armsJoint[1].slerpDrive = x;

            Vector3 jointValue = new Vector3(15f, 10f, 5f);

            armsJoint[1].targetAngularVelocity = jointValue;

            JointDrive x1 = forearmsJoint[1].slerpDrive;
            x1.positionDamper = 100f;
            x1.positionSpring = 1000f;
            x1.maximumForce = 1000f;
            forearmsJoint[1].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(40f, -4f, 6f);

            forearmsJoint[1].targetAngularVelocity = jointValue1;
            
        }*/
        if(leftWindUp)
        {
            JointDrive x = armsJoint[0].slerpDrive;
            x.positionDamper = 100f;
            x.positionSpring = 1000f;
            x.maximumForce = 1000f;
            armsJoint[0].slerpDrive = x;

            Vector3 jointValue = new Vector3(20f, 0f, 0f);

            armsJoint[0].targetAngularVelocity = jointValue;

            JointDrive x1 = forearmsJoint[0].slerpDrive;
            x1.positionDamper = 100f;
            x1.positionSpring = 1000f;
            x1.maximumForce = 1000f;
            forearmsJoint[0].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(-20f, 0f, 0f);

            forearmsJoint[0].targetAngularVelocity = jointValue1;
            if (windupDrag)
                chestBody.velocity *= -30f * Time.deltaTime;
        }

        if (leftpunch)
        {
            //EffectsController.PlayPunchSound(handsBody[0].position, handsBody[0].velocity.sqrMagnitude, handsBody[0].name);
            leftpunchTimer += Time.deltaTime;
            if (leftpunchTimer < punchSpeed * punchForceApplyPercent)
            {
                if (leftpunchTimer < punchSpeed * (punchForceApplyPercent - punchForceApplyPercent * accuracyReduceFactor))
                {
                    leftPunchTarget = (target - handsBody[0].transform.position).normalized;

                }
                JointDrive x = armsJoint[0].slerpDrive;
                x.positionDamper = 0f;
                x.positionSpring = 0f;
                x.maximumForce = 0f;
                armsJoint[0].slerpDrive = x;

                Vector3 jointValue = new Vector3(20f, 0f, 0f);

                armsJoint[0].targetAngularVelocity = jointValue;

                JointDrive x1 = forearmsJoint[0].slerpDrive;
                x1.positionDamper = 0f;
                x1.positionSpring = 0f;
                x1.maximumForce = 0f;
                forearmsJoint[0].slerpDrive = x1;

                Vector3 jointValue1 = new Vector3(0f, 0f, 0f);

                forearmsJoint[0].targetAngularVelocity = jointValue1;
                //handsBody[0].AddForceAtPosition((handsBody[0].transform.forward ) * 10, handsBody[0].transform.TransformPoint(Vector3.back * 0.2f), ForceMode.Impulse);

                //handsBody[0].AddForce((chestBody.transform.position + chestBody.transform.forward + (chestBody.transform.forward * -1) / 2f - handsBody[0].transform.position).normalized * 1000 * Time.deltaTime, ForceMode.VelocityChange);

                if (target != null)
                {


                    /*Vector3 a = (target.transform.position - handsBody[0].transform.position).normalized;
                    armsBody[0].AddForce(-(a * punchPower * 0.34f) * Time.deltaTime, ForceMode.VelocityChange);
                    handsBody[0].AddForce(a * punchPower * 1f * Time.deltaTime, ForceMode.VelocityChange);
                    */

                    //leftPunchTarget

                    armsBody[0].AddForce(-(leftPunchTarget * punchPower * 0.34f) * Time.deltaTime, ForceMode.VelocityChange);
                    handsBody[0].AddForce(leftPunchTarget * punchPower * 1f * Time.deltaTime, ForceMode.VelocityChange);

                    /* armsBody[0].AddForce(-(a * 20f), ForceMode.VelocityChange);
                     handsBody[0].AddForce(a * 30f , ForceMode.VelocityChange);*/
                    if (leftpunchTimer >= punchSpeed * (punchForceApplyPercent - 0.01f) && leftpunchTimer < punchSpeed * punchForceApplyPercent)
                    {
                        //print("Left Hand : handsBody[0].velocity.sqrMagnitude : " + handsBody[0].velocity.sqrMagnitude);

                        // EffectsController.PlayPunchSound(handsBody[0].position, handsBody[0].velocity.sqrMagnitude, handsBody[0].name);
                        //EffectsController.Shake(0.03f, 0.1f);
                    }

                }
                //if (leftpunch || rightpunch)
                // {
                chestBody.velocity *= -50f * Time.deltaTime;
                //}

            }
            /*if (leftpunchTimer >= punchSpeed * 0.2f && leftpunchTimer < punchSpeed * 0.4f)
            {
                


            }
            if (leftpunchTimer >= punchSpeed * 0.4f && leftpunchTimer < punchSpeed * 0.6f)
            {
                

            }
            if (leftpunchTimer >= punchSpeed * 0.6f && leftpunchTimer < punchSpeed * 1f)
            {
                

            }*/
            if (leftpunchTimer >= punchSpeed * 1f)
            {
                JointDrive x = armsJoint[0].slerpDrive;
                x.positionDamper = 100f;
                x.positionSpring = 1000f;
                x.maximumForce = 1000f;
                armsJoint[0].slerpDrive = x;

                Vector3 jointValue = new Vector3(-15f, -10f, 5f);

                armsJoint[0].targetAngularVelocity = jointValue;

                JointDrive x1 = forearmsJoint[0].slerpDrive;
                x1.positionDamper = 100f;
                x1.positionSpring = 1000f;
                x1.maximumForce = 1000f;
                forearmsJoint[0].slerpDrive = x1;

                Vector3 jointValue1 = new Vector3(-40f, 4f, 6f);

                forearmsJoint[0].targetAngularVelocity = jointValue1;
                leftpunch = false;
                leftpunchTimer = 0f;

            }
        }


        if (rightWindUp)
        {
            JointDrive x = armsJoint[1].slerpDrive;
            x.positionDamper = 100f;
            x.positionSpring = 1000f;
            x.maximumForce = 1000f;
            armsJoint[1].slerpDrive = x;

            Vector3 jointValue = new Vector3(-20f, 0f, 0f);

            armsJoint[1].targetAngularVelocity = jointValue;

            JointDrive x1 = forearmsJoint[1].slerpDrive;
            x1.positionDamper = 100f;
            x1.positionSpring = 1000f;
            x1.maximumForce = 1000f;
            forearmsJoint[1].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(20f, 0f, 0f);

            forearmsJoint[1].targetAngularVelocity = jointValue1;
            if (windupDrag)
                chestBody.velocity *= -30f * Time.deltaTime;
        }


        if (rightpunch)
        {
            rightpunchTimer += Time.deltaTime;
            if (rightpunchTimer < punchSpeed * punchForceApplyPercent)
            {
                if (rightpunchTimer < punchSpeed * (punchForceApplyPercent - punchForceApplyPercent * accuracyReduceFactor))
                {
                    rightPunchTarget = (target - handsBody[1].transform.position).normalized;
                }
                JointDrive x = armsJoint[1].slerpDrive;
                x.positionDamper = 0f;
                x.positionSpring = 0f;
                x.maximumForce = 0f;
                armsJoint[1].slerpDrive = x;

                Vector3 jointValue = new Vector3(0f, 0f, 0f);

                armsJoint[1].targetAngularVelocity = jointValue;

                JointDrive x1 = forearmsJoint[1].slerpDrive;
                x1.positionDamper = 0f;
                x1.positionSpring = 0f;
                x1.maximumForce = 0f;
                forearmsJoint[1].slerpDrive = x1;

                Vector3 jointValue1 = new Vector3(0f, 0f, 0f);

                forearmsJoint[1].targetAngularVelocity = jointValue1;
                //handsBody[0].AddForceAtPosition((handsBody[0].transform.forward ) * 10, handsBody[0].transform.TransformPoint(Vector3.back * 0.2f), ForceMode.Impulse);

                //handsBody[0].AddForce((chestBody.transform.position + chestBody.transform.forward + (chestBody.transform.forward * -1) / 2f - handsBody[0].transform.position).normalized * 1000 * Time.deltaTime, ForceMode.VelocityChange);

                if (target != null)
                {

                    /*Vector3 a = (target.transform.position - handsBody[1].transform.position).normalized;
                    armsBody[1].AddForce(-(a * punchPower * 0.34f) * Time.deltaTime, ForceMode.VelocityChange);
                    handsBody[1].AddForce(a * punchPower * 1f * Time.deltaTime, ForceMode.VelocityChange);
                    */
                    armsBody[1].AddForce(-(rightPunchTarget * punchPower * 0.34f) * Time.deltaTime, ForceMode.VelocityChange);
                    handsBody[1].AddForce(rightPunchTarget * punchPower * 1f * Time.deltaTime, ForceMode.VelocityChange);

                    if (rightpunchTimer >= punchSpeed * (punchForceApplyPercent - 0.01f) && rightpunchTimer < punchSpeed * punchForceApplyPercent)
                    {
                        // EffectsController.PlayPunchSound(handsBody[1].position, handsBody[1].velocity.sqrMagnitude, handsBody[1].name);
                        //EffectsController.Shake(0.03f, 0.1f);
                       // print("Right Hand : handsBody[1].velocity.sqrMagnitude : " + handsBody[1].velocity.sqrMagnitude);
                    }
                    /*armsBody[1].AddForce(-(a * 20f), ForceMode.VelocityChange);
                    handsBody[1].AddForce(a * 30f, ForceMode.VelocityChange);*/

                }
                //if (leftpunch || rightpunch)
                //{
                chestBody.velocity *= -30f * Time.deltaTime;
                //}
            }
            /*if (rightpunchTimer >= punchSpeed * 0.2f && rightpunchTimer < punchSpeed * 0.4f)
            {
               

            }
            if (rightpunchTimer >= punchSpeed * 0.4f && rightpunchTimer < punchSpeed * 0.6f)
            {
                


            }
            if (rightpunchTimer >= punchSpeed * 0.6f && rightpunchTimer < punchSpeed * 1f)
            {
                
            }*/
            if (rightpunchTimer >= punchSpeed * 1f)
            {
                JointDrive x = armsJoint[1].slerpDrive;
                x.positionDamper = 100f;
                x.positionSpring = 1000f;
                x.maximumForce = 1000f;
                armsJoint[1].slerpDrive = x;

                Vector3 jointValue = new Vector3(15f, 10f, 5f);

                armsJoint[1].targetAngularVelocity = jointValue;

                JointDrive x1 = forearmsJoint[1].slerpDrive;
                x1.positionDamper = 100f;
                x1.positionSpring = 1000f;
                x1.maximumForce = 1000f;
                forearmsJoint[1].slerpDrive = x1;

                Vector3 jointValue1 = new Vector3(40f, -4f, 6f);

                forearmsJoint[1].targetAngularVelocity = jointValue1;
                rightpunch = false;
                rightpunchTimer = 0f;

            }
        }
    }
}
