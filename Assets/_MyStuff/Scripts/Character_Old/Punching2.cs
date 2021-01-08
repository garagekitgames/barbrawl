using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punching2 : MonoBehaviour
{

    public Rigidbody chestBody;

    public PlayerController1 pContrl;


    public Rigidbody[] armsBody = new Rigidbody[2];
    public ConfigurableJoint[] armsJoint = new ConfigurableJoint[2];

    public Rigidbody[] forearmsBody = new Rigidbody[2];
    public ConfigurableJoint[] forearmsJoint = new ConfigurableJoint[2];

    public Rigidbody[] handsBody = new Rigidbody[2];
    public ConfigurableJoint[] handsJoint = new ConfigurableJoint[2];

    public CharacterInput input;

    private float leftbuttonClickTimer = 0f;
    private float rightbuttonClickTimer = 0f;

    public bool leftBlock = false;
    public bool leftpunch = false;
    public bool leftThrow = false;

    public bool rightBlock = false;
    public bool rightpunch = false;
    public bool rightThrow = false;

    private float leftpunchTimer = 0.0f;
    private float rightpunchTimer = 0.0f;

    public float punchSpeed = 0.5f;
    public float punchPower = 1300f;

    public Transform target;

    public Vector3 leftPunchTarget = Vector3.zero;
    public Vector3 rightPunchTarget = Vector3.zero;

    // Use this for initialization
    void Start()
    {

        input = GetComponent<CharacterInput>();
        pContrl = GetComponent<PlayerController1>();

    }

    // Update is called once per frame
    void Update()
    {

        CheckInput();

        target.position = pContrl.target;



    }

    private void CheckInput()
    {
        //LEft and Right Punch Grab and Throw 
        if (input.PressLeftPunch())
        {
            // print("Just Pressed Key");

            leftbuttonClickTimer = 0f;
            leftBlock = false;

        }

        else if (input.HoldLeftPunch())
        {
            //print("Key Held");
            if (leftbuttonClickTimer < 1f) // as long as key is held down increase time, this records how long the key is held down
            {
                leftbuttonClickTimer += Time.deltaTime; //this records how long the key is held down
            }
            if (leftbuttonClickTimer > 0.2f && leftBlock == false)    // if the button is pressed for more than 0.2 seconds grab
            {
                leftBlock = true;


            }
            else    // else ready the arm, pull back for punch / grab
            {
                //call punch action readying
            }

        }

        else if (input.ReleaseLeftPunch())
        {
            //print("Key Released");
            if (leftbuttonClickTimer <= 0.2f && !leftpunch) // as long as key is held down increase time, this records how long the key is held down
            {
                leftpunch = true;

                leftbuttonClickTimer = 0f;
            }
            else
            {
                leftThrow = true;

            }

            leftBlock = false;
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
        }



        //////////////////

        if (input.PressRightPunch())
        {
            // print("Just Pressed Key");

            rightbuttonClickTimer = 0f;
            rightBlock = false;

        }

        else if (input.HoldRightPunch())
        {
            //print("Key Held");
            if (rightbuttonClickTimer < 1f) // as long as key is held down increase time, this records how long the key is held down
            {
                rightbuttonClickTimer += Time.deltaTime; //this records how long the key is held down
            }
            if (rightbuttonClickTimer > 0.2f && rightBlock == false)    // if the button is pressed for more than 0.2 seconds grab
            {
                rightBlock = true;


            }
            else    // else ready the arm, pull back for punch / grab
            {
                //call punch action readying
            }

        }

        else if (input.ReleaseRightPunch())
        {
            //print("Key Released");
            if (rightbuttonClickTimer <= 0.2f && !rightpunch) // as long as key is held down increase time, this records how long the key is held down
            {
                rightpunch = true;
                rightbuttonClickTimer = 0f;
            }
            else
            {
                rightThrow = true;

            }
            rightBlock = false;
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

        }


    }

    private void FixedUpdate()
    {
        if (leftBlock)
        {


            JointDrive x = armsJoint[0].slerpDrive;
            x.positionDamper = 100f;
            x.positionSpring = 1000f;
            x.maximumForce = 1000f;
            armsJoint[0].slerpDrive = x;

            Vector3 jointValue = new Vector3(-30f, -15f, 11f);

            armsJoint[0].targetAngularVelocity = jointValue;

            JointDrive x1 = forearmsJoint[0].slerpDrive;
            x1.positionDamper = 100f;
            x1.positionSpring = 1000f;
            x1.maximumForce = 1000f;
            forearmsJoint[0].slerpDrive = x1;

            Vector3 jointValue1 = new Vector3(-40f, 50f, 6f);

            forearmsJoint[0].targetAngularVelocity = jointValue1;


            //leftBlock = false;

        }
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

        if (rightBlock)
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




        }
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


        if (leftpunch)
        {
            //EffectsController.PlayPunchSound(handsBody[0].position, handsBody[0].velocity.sqrMagnitude, handsBody[0].name);
            leftpunchTimer += Time.deltaTime;
            if (leftpunchTimer < punchSpeed * 0.2f)
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

                chestBody.velocity *= -10f * Time.deltaTime;
            }
            if (leftpunchTimer >= punchSpeed * 0.2f && leftpunchTimer < punchSpeed * 0.4f)
            {
                leftPunchTarget = (target.transform.position - handsBody[0].transform.position).normalized;


            }
            if (leftpunchTimer >= punchSpeed * 0.4f && leftpunchTimer < punchSpeed * 0.6f)
            {
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
                    if (leftpunchTimer >= punchSpeed * 0.59f && leftpunchTimer < punchSpeed * 0.6f)
                    {
                        //print("Left Hand : handsBody[0].velocity.sqrMagnitude : " + handsBody[0].velocity.sqrMagnitude);

                        // EffectsController.PlayPunchSound(handsBody[0].position, handsBody[0].velocity.sqrMagnitude, handsBody[0].name);
                        //EffectsController.Shake(0.03f, 0.1f);
                    }

                }
                //if (leftpunch || rightpunch)
                // {
                chestBody.velocity *= -30f * Time.deltaTime;
                //}


            }
            if (leftpunchTimer >= punchSpeed * 0.6f && leftpunchTimer < punchSpeed * 1f)
            {


            }
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


        if (rightpunch)
        {
            rightpunchTimer += Time.deltaTime;
            if (rightpunchTimer < punchSpeed * 0.2f)
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

                chestBody.velocity *= -10f * Time.deltaTime;
            }
            if (rightpunchTimer >= punchSpeed * 0.2f && rightpunchTimer < punchSpeed * 0.4f)
            {
                rightPunchTarget = (target.transform.position - handsBody[1].transform.position).normalized;

            }
            if (rightpunchTimer >= punchSpeed * 0.4f && rightpunchTimer < punchSpeed * 0.6f)
            {
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

                    if (rightpunchTimer >= punchSpeed * 0.59f && rightpunchTimer < punchSpeed * 0.6f)
                    {
                        // EffectsController.PlayPunchSound(handsBody[1].position, handsBody[1].velocity.sqrMagnitude, handsBody[1].name);
                        //EffectsController.Shake(0.03f, 0.1f);
                        //print("Right Hand : handsBody[1].velocity.sqrMagnitude : " + handsBody[1].velocity.sqrMagnitude);
                    }
                    /*armsBody[1].AddForce(-(a * 20f), ForceMode.VelocityChange);
                    handsBody[1].AddForce(a * 30f, ForceMode.VelocityChange);*/

                }
                //if (leftpunch || rightpunch)
                //{
                chestBody.velocity *= -30f * Time.deltaTime;
                //}


            }
            if (rightpunchTimer >= punchSpeed * 0.6f && rightpunchTimer < punchSpeed * 1f)
            {

            }
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
