using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kicking2 : MonoBehaviour
{

    public Rigidbody chestBody;

    public PlayerController1 pContrl;


    public Rigidbody[] thighsBody = new Rigidbody[2];
    public ConfigurableJoint[] thighsJoint = new ConfigurableJoint[2];

    public Rigidbody[] legsBody = new Rigidbody[2];
    public ConfigurableJoint[] legsJoint = new ConfigurableJoint[2];

    public Rigidbody[] feetBody = new Rigidbody[2];
    public ConfigurableJoint[] feetJoint = new ConfigurableJoint[2];

    public CharacterInput input;

    private float leftbuttonClickTimer = 0f;
    private float rightbuttonClickTimer = 0f;

    // public bool leftBlock = false;
    public bool leftKick = false;
    // public bool leftThrow = false;
    public bool leftWindUp = false;


    //public bool rightBlock = false;
    public bool rightKick = false;
    //public bool rightThrow = false;
    public bool rightWindUp = false;

    public float leftkickTimer = 0.0f;
    public float leftwindTimer = 0.0f;

    private float rightwindTimer = 0.0f;

    private float rightkickTimer = 0.0f;

    [Range(0, 1)]
    public float kickForceApplyPercent = 0.5f;// percentage of the punch duration the force is applied, shorter duration would mean less tracking
    [Range(0, 5)]
    public float kickPowerIncreaseRate = 2;//2 to 5 works good
    [Range(0f, 0.9f)]
    public float accuracyReduceFactor = 1f; //less value is better, percentage of 

    public float kickSpeed = 0.5f;
    public float kickPower = 1300f;
    public float startPunchPower = 0f;
    public float maxPunchPower = 3500;
    public float minPunchPower = 1300;

    public Vector3 target;

    public Vector3 leftKickTarget = Vector3.zero;
    public Vector3 rightKickTarget = Vector3.zero;

    public Jump jump;
    public bool windupDrag = true;

    // Use this for initialization
    void Start()
    {

        startPunchPower = kickPower;
        input = GetComponent<CharacterInput>();
        pContrl = GetComponent<PlayerController1>();
        jump = GetComponent<Jump>();

    }

    // Update is called once per frame
    void Update()
    {

        CheckInput();

        target = pContrl.target;

    }

    private void CheckInput()
    {
        //LEft and Right Punch Grab and Throw 
        if (input.PressLeftKick())
        {
            leftbuttonClickTimer = 0f;
            leftWindUp = false;

        }

        else if (input.HoldLeftKick())
        {
            

            //print("Key Held");
            if (leftbuttonClickTimer < 1f) // as long as key is held down increase time, this records how long the key is held down
            {
                leftbuttonClickTimer += Time.deltaTime; //this records how long the key is held down
            }
            if (leftbuttonClickTimer > 0.2f && leftWindUp == false)    // if the button is pressed for more than 0.2 seconds grab
            {
                leftWindUp = true;


            }
            else    // else ready the arm, pull back for punch / grab
            {
                //call punch action readying
            }


        }

        else if (input.ReleaseLeftKick())
        {
            //print("Key Released");
           /* if (!leftKick) // as long as key is held down increase time, this records how long the key is held down
            {
                kickPower = Mathf.Clamp(startPunchPower * (1 + leftbuttonClickTimer * kickPowerIncreaseRate), minPunchPower, maxPunchPower);

                leftKick = true;
                leftbuttonClickTimer = 0f;
            }

            leftWindUp = false;*/


            //print("Key Released");
            if (leftbuttonClickTimer <= 0.2f && !leftKick) // as long as key is held down increase time, this records how long the key is held down
            {
                leftKick = true;

                leftbuttonClickTimer = 0f;
            }
            else
            {
                //leftThrow = true;

            }

            leftWindUp = false;
            JointDrive x = thighsJoint[0].slerpDrive;
            x.positionDamper = 0f;
            x.positionSpring = 0f;
            x.maximumForce = 0f;
            thighsJoint[0].slerpDrive = x;

            Vector3 jointValue = new Vector3(20f, 0f, 0f);

            thighsJoint[0].targetAngularVelocity = jointValue;

            JointDrive x1 = legsJoint[0].slerpDrive;
            x1.positionDamper = 0f;
            x1.positionSpring = 0f;
            x1.maximumForce = 0f;
            legsJoint[0].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(0f, 0f, 0f);

            legsJoint[0].targetAngularVelocity = jointValue1;
            /* JointDrive x = thighsJoint[0].slerpDrive;
             x.positionDamper = 100f;
             x.positionSpring = 1000f;
             x.maximumForce = 1000f;
             thighsJoint[0].slerpDrive = x;

             Vector3 jointValue = new Vector3(-15f, -10f, 5f);

             thighsJoint[0].targetAngularVelocity = jointValue;

             JointDrive x1 = forethighsJoint[0].slerpDrive;
             x1.positionDamper = 100f;
             x1.positionSpring = 1000f;
             x1.maximumForce = 1000f;
             forethighsJoint[0].slerpDrive = x1;

             Vector3 jointValue1 = new Vector3(-40f, 4f, 6f);

             forethighsJoint[0].targetAngularVelocity = jointValue1;*/

        }



        //////////////////

        if (input.PressRightKick())
        {
            // print("Just Pressed Key");

            rightbuttonClickTimer = 0f;
            rightWindUp = false;

        }

        else if (input.HoldRightKick())
        {

           /* rightbuttonClickTimer += Time.deltaTime;
            if (rightWindUp == false && !rightKick)    // if the button is pressed for more than 0.2 seconds grab
            {
                rightWindUp = true;

            }*/


            if (rightbuttonClickTimer < 1f) // as long as key is held down increase time, this records how long the key is held down
            {
                rightbuttonClickTimer += Time.deltaTime; //this records how long the key is held down
            }
            if (rightbuttonClickTimer > 0.2f && rightWindUp == false)    // if the button is pressed for more than 0.2 seconds grab
            {
                rightWindUp = true;


            }
            else    // else ready the arm, pull back for punch / grab
            {
                //call punch action readying
            }


        }

        else if (input.ReleaseRightKick())
        {

            /*if (!rightKick) // as long as key is held down increase time, this records how long the key is held down
            {
                kickPower = Mathf.Clamp(startPunchPower * (1 + rightbuttonClickTimer * kickPowerIncreaseRate), minPunchPower, maxPunchPower);

                rightKick = true;
                rightbuttonClickTimer = 0f;
            }

            rightWindUp = false;
            */

            if (rightbuttonClickTimer <= 0.2f && !rightKick) // as long as key is held down increase time, this records how long the key is held down
            {
                rightKick = true;
                rightbuttonClickTimer = 0f;
            }
            else
            {
                //rightThrow = true;

            }
            rightWindUp = false;

            JointDrive x = thighsJoint[1].slerpDrive;
            x.positionDamper = 0f;
            x.positionSpring = 0f;
            x.maximumForce = 0f;
            thighsJoint[1].slerpDrive = x;

            Vector3 jointValue = new Vector3(20f, 0f, 0f);

            thighsJoint[1].targetAngularVelocity = jointValue;

            JointDrive x1 = legsJoint[1].slerpDrive;
            x1.positionDamper = 0f;
            x1.positionSpring = 0f;
            x1.maximumForce = 0f;
            legsJoint[1].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(0f, 0f, 0f);

            legsJoint[1].targetAngularVelocity = jointValue1;

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
        if (leftWindUp)
        {
           // leftwindTimer += Time.deltaTime;
            JointDrive x = thighsJoint[0].slerpDrive;
            x.positionDamper = 100f;
            x.positionSpring = 1000f;
            x.maximumForce = 1000f;
            thighsJoint[0].slerpDrive = x;

            Vector3 jointValue = new Vector3(40, 0f, 0f);

            thighsJoint[0].targetAngularVelocity = jointValue;

            JointDrive x1 = legsJoint[0].slerpDrive;
            x1.positionDamper = 100f;
            x1.positionSpring = 1000f;
            x1.maximumForce = 1000f;
            legsJoint[0].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(-40, 0f, 0f);

            legsJoint[0].targetAngularVelocity = jointValue1;

            //if (windupDrag)
               // chestBody.velocity *= -30f * Time.deltaTime;

            /*if (leftwindTimer < 0.2f && !jump.inAir)
            {
                chestBody.AddForce((Vector3.up * 800) * Time.deltaTime, ForceMode.VelocityChange);
            }*/



            //chestBody.AddForce((Vector3.up * 500 + pContrl.inputDirection * 500) * Time.deltaTime, ForceMode.VelocityChange);

        }

        //if (leftKick)
        //{
        //    leftwindTimer = 0;
        //    //EffectsController.PlayPunchSound(feetBody[0].position, feetBody[0].velocity.sqrMagnitude, feetBody[0].name);
        //    leftkickTimer += Time.deltaTime;
        //    if (leftkickTimer < kickSpeed * kickForceApplyPercent)
        //    {
        //        if (leftkickTimer < kickSpeed * (kickForceApplyPercent - kickForceApplyPercent * accuracyReduceFactor))
        //        {
        //            leftKickTarget = (target.transform.position - feetBody[0].transform.position).normalized;

        //        }
        //        JointDrive x = thighsJoint[0].slerpDrive;
        //        x.positionDamper = 0f;
        //        x.positionSpring = 0f;
        //        x.maximumForce = 0f;
        //        thighsJoint[0].slerpDrive = x;

        //        Vector3 jointValue = new Vector3(20f, 0f, 0f);

        //        thighsJoint[0].targetAngularVelocity = jointValue;

        //        JointDrive x1 = legsJoint[0].slerpDrive;
        //        x1.positionDamper = 0f;
        //        x1.positionSpring = 0f;
        //        x1.maximumForce = 0f;
        //        legsJoint[0].slerpDrive = x1;

        //        Vector3 jointValue1 = new Vector3(0f, 0f, 0f);

        //        legsJoint[0].targetAngularVelocity = jointValue1;
        //        //feetBody[0].AddForceAtPosition((feetBody[0].transform.forward ) * 10, feetBody[0].transform.TransformPoint(Vector3.back * 0.2f), ForceMode.Impulse);

        //        //feetBody[0].AddForce((chestBody.transform.position + chestBody.transform.forward + (chestBody.transform.forward * -1) / 2f - feetBody[0].transform.position).normalized * 1000 * Time.deltaTime, ForceMode.VelocityChange);

        //        if (target != null)
        //        {


        //            /*Vector3 a = (target.transform.position - feetBody[0].transform.position).normalized;
        //            thighsBody[0].AddForce(-(a * punchPower * 0.34f) * Time.deltaTime, ForceMode.VelocityChange);
        //            feetBody[0].AddForce(a * punchPower * 1f * Time.deltaTime, ForceMode.VelocityChange);
        //            */

        //            //leftPunchTarget

        //            thighsBody[0].AddForce(-(leftKickTarget * kickPower * 0.5f) * Time.deltaTime, ForceMode.Impulse);
        //            // legsBody[0].AddForce(-(leftKickTarget * kickPower * 0.5f) * Time.deltaTime, ForceMode.Impulse);
        //            feetBody[0].AddForce(leftKickTarget * kickPower * 1f * Time.deltaTime, ForceMode.Impulse);

        //            // chestBody.AddForce(leftKickTarget * kickPower * 0.1f * Time.deltaTime, ForceMode.VelocityChange);
        //            /* thighsBody[0].AddForce(-(a * 20f), ForceMode.VelocityChange);
        //             feetBody[0].AddForce(a * 30f , ForceMode.VelocityChange);*/
        //            if (leftkickTimer >= kickSpeed * (kickForceApplyPercent - 0.01f) && leftkickTimer < kickSpeed * kickForceApplyPercent)
        //            {
        //                //print("Left Hand : feetBody[0].velocity.sqrMagnitude : " + feetBody[0].velocity.sqrMagnitude);

        //                // EffectsController.PlayPunchSound(feetBody[0].position, feetBody[0].velocity.sqrMagnitude, feetBody[0].name);
        //                //EffectsController.Shake(0.03f, 0.1f);
        //            }

        //        }
        //        //if (leftpunch || rightKick)
        //        // {
        //        chestBody.velocity *= -100f * Time.deltaTime;
        //        //}

        //    }
        //    /*if (leftkickTimer >= kickSpeed * 0.2f && leftkickTimer < kickSpeed * 0.4f)
        //    {
                


        //    }
        //    if (leftkickTimer >= kickSpeed * 0.4f && leftkickTimer < kickSpeed * 0.6f)
        //    {
                

        //    }
        //    if (leftkickTimer >= kickSpeed * 0.6f && leftkickTimer < kickSpeed * 1f)
        //    {
                

        //    }*/
        //    if (leftkickTimer >= kickSpeed * 1f)
        //    {
        //        JointDrive x = thighsJoint[0].slerpDrive;
        //        x.positionDamper = 0;
        //        x.positionSpring = 0;
        //        x.maximumForce = 0;
        //        thighsJoint[0].slerpDrive = x;

        //        Vector3 jointValue = new Vector3(0, 0, 0);

        //        thighsJoint[0].targetAngularVelocity = jointValue;

        //        JointDrive x1 = legsJoint[0].slerpDrive;
        //        x1.positionDamper = 0;
        //        x1.positionSpring = 0;
        //        x1.maximumForce = 0;
        //        legsJoint[0].slerpDrive = x1;

        //        Vector3 jointValue1 = new Vector3(0, 0, 0);

        //        legsJoint[0].targetAngularVelocity = jointValue1;
        //        leftKick = false;
        //        leftkickTimer = 0f;

        //    }
        //}


        if (leftKick)
        {
            //EffectsController.PlayPunchSound(handsBody[0].position, handsBody[0].velocity.sqrMagnitude, handsBody[0].name);
            leftkickTimer += Time.deltaTime;
            if (leftkickTimer < kickSpeed * 0.2f)
            {
                leftwindTimer += Time.deltaTime;
                JointDrive x = thighsJoint[0].slerpDrive;
                x.positionDamper = 100f;
                x.positionSpring = 1000f;
                x.maximumForce = 1000f;
                thighsJoint[0].slerpDrive = x;

                Vector3 jointValue = new Vector3(40, 0f, 0f);

                thighsJoint[0].targetAngularVelocity = jointValue;

                JointDrive x1 = legsJoint[0].slerpDrive;
                x1.positionDamper = 100f;
                x1.positionSpring = 1000f;
                x1.maximumForce = 1000f;
                legsJoint[0].slerpDrive = x1;

                Vector3 jointValue1 = new Vector3(-40, 0f, 0f);

                legsJoint[0].targetAngularVelocity = jointValue1;

                //if (windupDrag)
                   // chestBody.velocity *= -30f * Time.deltaTime;

                /*if (leftwindTimer < 0.2f && !jump.inAir)
                {
                    chestBody.AddForce((Vector3.up * 800) * Time.deltaTime, ForceMode.VelocityChange);
                }
                */
                //chestBody.velocity *= -10f * Time.deltaTime;
            }
            if (leftkickTimer >= kickSpeed * 0.2f && leftkickTimer < kickSpeed * 0.4f)
            {
                leftKickTarget = (target - feetBody[0].transform.position).normalized;


            }
            if (leftkickTimer >= kickSpeed * 0.4f && leftkickTimer < kickSpeed * 0.6f)
            {
                JointDrive x = thighsJoint[0].slerpDrive;
                x.positionDamper = 0f;
                x.positionSpring = 0f;
                x.maximumForce = 0f;
                thighsJoint[0].slerpDrive = x;

                Vector3 jointValue = new Vector3(20f, 0f, 0f);

                thighsJoint[0].targetAngularVelocity = jointValue;

                JointDrive x1 = legsJoint[0].slerpDrive;
                x1.positionDamper = 0f;
                x1.positionSpring = 0f;
                x1.maximumForce = 0f;
                legsJoint[0].slerpDrive = x1;

                Vector3 jointValue1 = new Vector3(0f, 0f, 0f);

                legsJoint[0].targetAngularVelocity = jointValue1;
                //handsBody[0].AddForceAtPosition((handsBody[0].transform.forward ) * 10, handsBody[0].transform.TransformPoint(Vector3.back * 0.2f), ForceMode.Impulse);

                //handsBody[0].AddForce((chestBody.transform.position + chestBody.transform.forward + (chestBody.transform.forward * -1) / 2f - handsBody[0].transform.position).normalized * 1000 * Time.deltaTime, ForceMode.VelocityChange);

                if (target != null)
                {


                    /*Vector3 a = (target.transform.position - handsBody[0].transform.position).normalized;
                    armsBody[0].AddForce(-(a * punchPower * 0.34f) * Time.deltaTime, ForceMode.VelocityChange);
                    handsBody[0].AddForce(a * punchPower * 1f * Time.deltaTime, ForceMode.VelocityChange);
                    */

                    //leftPunchTarget

                    thighsBody[0].AddForce(-(leftKickTarget * kickPower * 0.5f) * Time.deltaTime, ForceMode.Impulse);
                    // legsBody[0].AddForce(-(leftKickTarget * kickPower * 0.5f) * Time.deltaTime, ForceMode.Impulse);
                    feetBody[0].AddForce(leftKickTarget * kickPower * 1f * Time.deltaTime, ForceMode.Impulse);
                    /* armsBody[0].AddForce(-(a * 20f), ForceMode.VelocityChange);
                     handsBody[0].AddForce(a * 30f , ForceMode.VelocityChange);*/
                    if (leftkickTimer >= kickSpeed * 0.59f && leftkickTimer < kickSpeed * 0.6f)
                    {
                        //print("Left Hand : handsBody[0].velocity.sqrMagnitude : " + handsBody[0].velocity.sqrMagnitude);

                        // EffectsController.PlayPunchSound(handsBody[0].position, handsBody[0].velocity.sqrMagnitude, handsBody[0].name);
                        //EffectsController.Shake(0.03f, 0.1f);
                    }

                }
                //if (leftpunch || rightKick)
                // {
                chestBody.velocity *= -100f * Time.deltaTime;
                //}


            }
            if (leftkickTimer >= kickSpeed * 0.6f && leftkickTimer < kickSpeed * 1f)
            {


            }
            if (leftkickTimer >= kickSpeed * 1f)
            {
                JointDrive x = thighsJoint[0].slerpDrive;
                x.positionDamper = 0;
                x.positionSpring = 0;
                x.maximumForce = 0;
                thighsJoint[0].slerpDrive = x;

                Vector3 jointValue = new Vector3(0, 0, 0);

                thighsJoint[0].targetAngularVelocity = jointValue;

                JointDrive x1 = legsJoint[0].slerpDrive;
                x1.positionDamper = 0;
                x1.positionSpring = 0;
                x1.maximumForce = 0;
                legsJoint[0].slerpDrive = x1;

                Vector3 jointValue1 = new Vector3(0, 0, 0);

                legsJoint[0].targetAngularVelocity = jointValue1;
                leftKick = false;
                leftkickTimer = 0f;

            }
        }

        if (rightWindUp)
        {
            rightwindTimer += Time.deltaTime;
            JointDrive x = thighsJoint[1].slerpDrive;
            x.positionDamper = 100f;
            x.positionSpring = 1000f;
            x.maximumForce = 1000f;
            thighsJoint[1].slerpDrive = x;

            Vector3 jointValue = new Vector3(40, 0f, 0f);

            thighsJoint[1].targetAngularVelocity = jointValue;

            JointDrive x1 = legsJoint[1].slerpDrive;
            x1.positionDamper = 100f;
            x1.positionSpring = 1000f;
            x1.maximumForce = 1000f;
            legsJoint[1].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(-40, 0f, 0f);

            legsJoint[1].targetAngularVelocity = jointValue1;

            //if (windupDrag)
               // chestBody.velocity *= -30f * Time.deltaTime;

           /* if (rightwindTimer < 0.2f && !jump.inAir)
            {
                chestBody.AddForce((Vector3.up * 800) * Time.deltaTime, ForceMode.VelocityChange);
            }*/
            //chestBody.AddForce((Vector3.up * 500 + pContrl.inputDirection * 500) * Time.deltaTime, ForceMode.VelocityChange);

        }

        //if (rightKick)
        //{
        //    rightwindTimer = 0;
        //    //EffectsController.PlayPunchSound(feetBody[0].position, feetBody[0].velocity.sqrMagnitude, feetBody[0].name);
        //    rightkickTimer += Time.deltaTime;
        //    if (rightkickTimer < kickSpeed * kickForceApplyPercent)
        //    {
        //        if (rightkickTimer < kickSpeed * (kickForceApplyPercent - kickForceApplyPercent * accuracyReduceFactor))
        //        {
        //            rightKickTarget = (target.transform.position - feetBody[1].transform.position).normalized;

        //        }
        //        JointDrive x = thighsJoint[1].slerpDrive;
        //        x.positionDamper = 0f;
        //        x.positionSpring = 0f;
        //        x.maximumForce = 0f;
        //        thighsJoint[1].slerpDrive = x;

        //        Vector3 jointValue = new Vector3(20f, 0f, 0f);

        //        thighsJoint[1].targetAngularVelocity = jointValue;

        //        JointDrive x1 = legsJoint[1].slerpDrive;
        //        x1.positionDamper = 0f;
        //        x1.positionSpring = 0f;
        //        x1.maximumForce = 0f;
        //        legsJoint[1].slerpDrive = x1;

        //        Vector3 jointValue1 = new Vector3(0f, 0f, 0f);

        //        legsJoint[1].targetAngularVelocity = jointValue1;
        //        //feetBody[0].AddForceAtPosition((feetBody[0].transform.forward ) * 10, feetBody[0].transform.TransformPoint(Vector3.back * 0.2f), ForceMode.Impulse);

        //        //feetBody[0].AddForce((chestBody.transform.position + chestBody.transform.forward + (chestBody.transform.forward * -1) / 2f - feetBody[0].transform.position).normalized * 1000 * Time.deltaTime, ForceMode.VelocityChange);

        //        if (target != null)
        //        {


        //            /*Vector3 a = (target.transform.position - feetBody[0].transform.position).normalized;
        //            thighsBody[0].AddForce(-(a * punchPower * 0.34f) * Time.deltaTime, ForceMode.VelocityChange);
        //            feetBody[0].AddForce(a * punchPower * 1f * Time.deltaTime, ForceMode.VelocityChange);
        //            */

        //            //leftPunchTarget

        //            thighsBody[1].AddForce(-(rightKickTarget * kickPower * 0.5f) * Time.deltaTime, ForceMode.VelocityChange);
        //            feetBody[1].AddForce(rightKickTarget * kickPower * 1f * Time.deltaTime, ForceMode.VelocityChange);

        //            /* thighsBody[0].AddForce(-(a * 20f), ForceMode.VelocityChange);
        //             feetBody[0].AddForce(a * 30f , ForceMode.VelocityChange);*/
        //            if (leftkickTimer >= kickSpeed * (kickForceApplyPercent - 0.01f) && leftkickTimer < kickSpeed * kickForceApplyPercent)
        //            {
        //                //print("Left Hand : feetBody[0].velocity.sqrMagnitude : " + feetBody[0].velocity.sqrMagnitude);

        //                // EffectsController.PlayPunchSound(feetBody[0].position, feetBody[0].velocity.sqrMagnitude, feetBody[0].name);
        //                //EffectsController.Shake(0.03f, 0.1f);
        //            }

        //        }
        //        //if (leftpunch || rightKick)
        //        // {
        //        chestBody.velocity *= -100 * Time.deltaTime;
        //        //}

        //    }
        //    /*if (leftkickTimer >= kickSpeed * 0.2f && leftkickTimer < kickSpeed * 0.4f)
        //    {
                


        //    }
        //    if (leftkickTimer >= kickSpeed * 0.4f && leftkickTimer < kickSpeed * 0.6f)
        //    {
                

        //    }
        //    if (leftkickTimer >= kickSpeed * 0.6f && leftkickTimer < kickSpeed * 1f)
        //    {
                

        //    }*/
        //    if (rightkickTimer >= kickSpeed * 1f)
        //    {
        //        JointDrive x = thighsJoint[1].slerpDrive;
        //        x.positionDamper = 0;
        //        x.positionSpring = 0;
        //        x.maximumForce = 0;
        //        thighsJoint[1].slerpDrive = x;

        //        Vector3 jointValue = new Vector3(0, 0, 0);

        //        thighsJoint[1].targetAngularVelocity = jointValue;

        //        JointDrive x1 = legsJoint[1].slerpDrive;
        //        x1.positionDamper = 0;
        //        x1.positionSpring = 0;
        //        x1.maximumForce = 0;
        //        legsJoint[1].slerpDrive = x1;

        //        Vector3 jointValue1 = new Vector3(0, 0, 0);

        //        legsJoint[1].targetAngularVelocity = jointValue1;
        //        rightKick = false;
        //        rightkickTimer = 0f;

        //    }
        //}

        if (rightKick)
        {
            rightkickTimer += Time.deltaTime;
            if (rightkickTimer < kickSpeed * 0.2f)
            {
                rightwindTimer += Time.deltaTime;
                JointDrive x = thighsJoint[1].slerpDrive;
                x.positionDamper = 100f;
                x.positionSpring = 1000f;
                x.maximumForce = 1000f;
                thighsJoint[1].slerpDrive = x;

                Vector3 jointValue = new Vector3(40, 0f, 0f);

                thighsJoint[1].targetAngularVelocity = jointValue;

                JointDrive x1 = legsJoint[1].slerpDrive;
                x1.positionDamper = 100f;
                x1.positionSpring = 1000f;
                x1.maximumForce = 1000f;
                legsJoint[1].slerpDrive = x1;

                Vector3 jointValue1 = new Vector3(-40, 0f, 0f);

                legsJoint[1].targetAngularVelocity = jointValue1;

               // if (windupDrag)
                    //chestBody.velocity *= -30f * Time.deltaTime;

               // chestBody.velocity *= -10f * Time.deltaTime;
            }
            if (rightkickTimer >= kickSpeed * 0.2f && rightkickTimer < kickSpeed * 0.4f)
            {
                rightKickTarget = (target - feetBody[1].transform.position).normalized;

            }
            if (rightkickTimer >= kickSpeed * 0.4f && rightkickTimer < kickSpeed * 0.6f)
            {
                JointDrive x = thighsJoint[1].slerpDrive;
                x.positionDamper = 0f;
                x.positionSpring = 0f;
                x.maximumForce = 0f;
                thighsJoint[1].slerpDrive = x;

                Vector3 jointValue = new Vector3(20f, 0f, 0f);

                thighsJoint[1].targetAngularVelocity = jointValue;

                JointDrive x1 = legsJoint[1].slerpDrive;
                x1.positionDamper = 0f;
                x1.positionSpring = 0f;
                x1.maximumForce = 0f;
                legsJoint[1].slerpDrive = x1;

                Vector3 jointValue1 = new Vector3(0f, 0f, 0f);

                legsJoint[1].targetAngularVelocity = jointValue1;
                //handsBody[0].AddForceAtPosition((handsBody[0].transform.forward ) * 10, handsBody[0].transform.TransformPoint(Vector3.back * 0.2f), ForceMode.Impulse);

                //handsBody[0].AddForce((chestBody.transform.position + chestBody.transform.forward + (chestBody.transform.forward * -1) / 2f - handsBody[0].transform.position).normalized * 1000 * Time.deltaTime, ForceMode.VelocityChange);

                if (target != null)
                {

                    /*Vector3 a = (target.transform.position - handsBody[1].transform.position).normalized;
                    armsBody[1].AddForce(-(a * punchPower * 0.34f) * Time.deltaTime, ForceMode.VelocityChange);
                    handsBody[1].AddForce(a * punchPower * 1f * Time.deltaTime, ForceMode.VelocityChange);
                    */
                    thighsBody[1].AddForce(-(rightKickTarget * kickPower * 0.5f) * Time.deltaTime, ForceMode.VelocityChange);
                    feetBody[1].AddForce(rightKickTarget * kickPower * 1f * Time.deltaTime, ForceMode.VelocityChange);


                    if (rightkickTimer >= kickSpeed * 0.59f && rightkickTimer < kickSpeed * 0.6f)
                    {
                        // EffectsController.PlayPunchSound(handsBody[1].position, handsBody[1].velocity.sqrMagnitude, handsBody[1].name);
                        //EffectsController.Shake(0.03f, 0.1f);
                        //print("Right Hand : handsBody[1].velocity.sqrMagnitude : " + handsBody[1].velocity.sqrMagnitude);
                    }
                    /*armsBody[1].AddForce(-(a * 20f), ForceMode.VelocityChange);
                    handsBody[1].AddForce(a * 30f, ForceMode.VelocityChange);*/

                }
                //if (leftpunch || rightKick)
                //{
                chestBody.velocity *= -100 * Time.deltaTime;
                //}


            }
            if (rightkickTimer >= kickSpeed * 0.6f && rightkickTimer < kickSpeed * 1f)
            {

            }
            if (rightkickTimer >= kickSpeed * 1f)
            {
                JointDrive x = thighsJoint[1].slerpDrive;
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
                rightKick = false;
                rightkickTimer = 0f;

            }
        }
    }
}
