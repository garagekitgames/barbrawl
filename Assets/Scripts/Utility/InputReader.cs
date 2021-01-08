using UnityEngine;
//using UnityEngine.Random;
using System.Collections;
//using System;
using System.Reflection;

public class InputReader : MonoBehaviour
{
    //
    static InputReader instance;
    //
    static public KeyCode keyPlayer1Fire = KeyCode.Q;
    static public KeyCode keyPlayer2Fire = KeyCode.Comma;
    static public KeyCode keyPlayer1Special = KeyCode.E;
    static public KeyCode keyPlayer2Special = KeyCode.Period;
    static public KeyCode keyPlayer2Up = KeyCode.UpArrow;
    static public KeyCode keyPlayer1Up = KeyCode.W;
    static public KeyCode keyPlayer2Down = KeyCode.DownArrow;
    static public KeyCode keyPlayer1Down = KeyCode.S;
    static public KeyCode keyPlayer2Left = KeyCode.LeftArrow;
    static public KeyCode keyPlayer1Left = KeyCode.A;
    static public KeyCode keyPlayer2Right = KeyCode.RightArrow;
    static public KeyCode keyPlayer1Right = KeyCode.D;

    static public KeyCode keyPlayer1Lift = KeyCode.LeftShift;
    static public KeyCode keyPlayer1Drop = KeyCode.LeftControl;

    static public KeyCode keyPlayer1Jump = KeyCode.Space;

    static public KeyCode keyPlayer1Drink = KeyCode.R;

    static public float mouseY;

    static public float mouseX;
    static int b = 1;

    static public float tapClickTimer = 0;

    static public bool attack = false;
    static public bool canJump = false;
    static public bool beginTouch = false;
   // static public float timerValue = 0.2f;

    //
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            //
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    //
    private static bool IsBlocked
    {
        get
        {
            return false;
        }
    }
    //
    public static void GetInput(int controllerNum, ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool jump, ref bool dash, ref bool special, ref bool leftPunch, ref bool rightPunch, ref bool Rup, ref bool Rdown, ref bool Rleft, ref bool Rright, ref bool drink, ref bool leftKick, ref bool rightKick, float timerValue)
    {
        up = down = left = right = fire = jump = dash = special = leftPunch = rightPunch = drink = false;
        leftKick = rightKick = false;
        // 
        Rup = Rdown = Rleft = Rright = false;
        if (controllerNum == 0)
        {
           //GetMobileInput(ref up, ref down, ref left, ref right, ref fire, ref jump, ref dash, ref special, ref leftPunch, ref rightPunch, ref Rup, ref Rdown, ref Rleft, ref Rright, ref drink, ref leftKick, ref rightKick, timerValue);

            GetKeyboard1Input(ref up, ref down, ref left, ref right, ref fire, ref jump, ref dash, ref special, ref leftPunch, ref rightPunch, ref Rup, ref Rdown, ref Rleft, ref Rright, ref drink, ref  leftKick, ref  rightKick);
            //GetXBoxControllerInput("_1", ref up, ref down, ref left, ref right, ref fire, ref jump, ref dash, ref special, ref leftPunch, ref rightPunch, ref Rup, ref Rdown, ref Rleft, ref Rright, ref drink, ref  leftKick, ref  rightKick);
        }
        if (controllerNum == 1)
        {
            GetKeyboard2Input(ref up, ref down, ref left, ref right, ref fire, ref jump, ref dash, ref special);
           // GetXBoxControllerInput("_2", ref up, ref down, ref left, ref right, ref fire, ref jump, ref dash, ref special, ref leftPunch, ref rightPunch, ref Rup, ref Rdown, ref Rleft, ref Rright, ref drink, ref leftKick, ref rightKick);
        }
        // 
    }
    //
    public static bool GetMenuInputStandardKeys(ref bool up, ref bool down, ref bool left, ref bool right, ref bool accept, ref bool decline)
    {
        if (IsBlocked)
            return false;

        up = Input.GetKeyDown(KeyCode.UpArrow);
        down = Input.GetKeyDown(KeyCode.DownArrow);
        left = Input.GetKeyDown(KeyCode.LeftArrow);
        right = Input.GetKeyDown(KeyCode.RightArrow);
        accept = Input.GetKeyDown(KeyCode.Return);
        decline = Input.GetKeyDown(KeyCode.Escape);

        return up | down | left | right | accept | decline;
    }

    //Mobile Input 
    public static void GetMobileInput(ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool jump, ref bool dash, ref bool special, ref bool leftPunch, ref bool rightPunch, ref bool Rup, ref bool Rdown, ref bool Rleft, ref bool Rright, ref bool drink, ref bool leftKick, ref bool rightKick, float timerValue)
    {
        if (IsBlocked)
            return;
        //

        float tempTimer = 0.2f;
        mouseY = Input.GetAxis("Mouse Y") * 1f;

        mouseY = Mathf.Clamp(mouseY, -1, 1);

        mouseX = Input.GetAxis("Mouse X") * 1f;

        mouseX = Mathf.Clamp(mouseX, -1, 1);

        fire = fire || Input.GetKey(keyPlayer1Fire);
        special = special || Input.GetKey(keyPlayer1Special);
        jump = jump || Input.GetKeyDown(keyPlayer1Jump);
        up = up || Input.GetKey(keyPlayer1Up);


        // Touch myTouch = Input.GetTouch(0);

        Touch[] myTouches = Input.touches;

        //if (Input.touchCount >= 2)

        ////if(myTouches.Length > 0 )
        //{
        //    //for (int i = 0; i < Input.touchCount; i++)
        //    //{
        //    //Do something with the touches
        //    // Debug.Log("deltaPosition : " + myTouches[0].deltaPosition);

        //    // jump = jump || (myTouches[0].phase == TouchPhase.Began);
        //    attack = false;
        //    jump = jump || ( myTouches[1].deltaPosition.y > 10);
        //    //jump = jump || (myTouches[0].tapCount > 1);
        //    //}
        //}
        //else
        //{

        /*if(canJump)
        {
            tempTimer = 0.01f;
        }
        else
        {
            tempTimer = 0.2f;
        }*/
       // Debug.Log("timerValue : " + timerValue);
       // for (int i = 0; i < Input.touchCount; i++)
            //if (Input.touchCount == 1)
            //{
            //    //Do something with the touches
                

            //    // jump = jump || (myTouches[0].phase == TouchPhase.Began);

            ////if ((myTouches[0].tapCount > 1))
            ////{
            ////    jump = jump || (myTouches[0].tapCount > 1);
            ////}
            ////else
            ////{


            //    if (myTouches[0].phase == TouchPhase.Began)
            //    {
            //        //jump = jump || myTouches[0].deltaPosition.y > 200;
            //        tapClickTimer = 0f;
            //        attack = false;
            //        beginTouch = true;
            //    //canJump = false;
            //    }
            //    else if (myTouches[0].phase == TouchPhase.Stationary || myTouches[0].phase == TouchPhase.Moved)
            //    {
            //        beginTouch = false;
            //        if (tapClickTimer < 1f) // as long as key is held down increase time, this records how long the key is held down
            //        {
            //            tapClickTimer += Time.deltaTime; //this records how long the key is held down
            //        }
            //        if (tapClickTimer > timerValue && attack == false)    // if the button is pressed for more than 0.2 seconds grab
            //        {
            //            attack = true;


            //        }
            //        else    // else ready the arm, pull back for punch / grab
            //        {
            //            //call punch action readying
            //        }
            //    }
            //    else if (myTouches[0].phase == TouchPhase.Ended || myTouches[0].phase == TouchPhase.Canceled)
            //    {
            //        if (tapClickTimer <= timerValue) // as long as key is held down increase time, this records how long the key is held down
            //        {
            //            canJump = true;
            //            jump = jump || myTouches[0].deltaPosition.y > myTouches[0].deltaPosition.x;


            //            tapClickTimer = 0f;
            //        }
            //        attack = false;

            //    }
            //}
                


            //}
        //}


        

        //Mobile touch control stuff tewmporary solution please change this 
        //this is for randomized attack
        if (beginTouch)
             b = Random.Range(1, 5);

         // Debug.Log("b : "+b);

         switch (b)
         {
             case 1: // if a is an integer
                 leftPunch = leftPunch || attack;
                 // Debug.Log("leftPunch");
                 break;
             case 2: // if a is a string
                 rightPunch = rightPunch || attack;
                 // Debug.Log("rightPunch");
                 break;
             case 3: // if a is an integer
                 leftKick = leftKick || attack;
                 //Debug.Log("leftKick");
                 break;
             case 4: // if a is a string
                 rightKick = rightKick || attack;
                 // Debug.Log("rightKick");
                 break;

         }

        /*leftPunch = leftPunch || Input.GetMouseButton(0);
        rightPunch = rightPunch || Input.GetMouseButton(1);*/

        /* leftKick = leftKick || Input.GetMouseButton(0);
         rightKick = rightKick || Input.GetMouseButton(1);*/
        //Debug.Log("down");
        down = down || Input.GetKey(keyPlayer1Down);
        left = left || Input.GetKey(keyPlayer1Left);
        right = right || Input.GetKey(keyPlayer1Right);

        drink = drink || Input.GetKey(keyPlayer1Drink);
        //
        Rup = Rup || Input.GetKey(keyPlayer1Lift);
        Rdown = Rdown || Input.GetKey(keyPlayer1Drop);
        /* Rleft = Rleft || mouseX < -0.25f;
        Rright = Rright || mouseX > 0.25f;*/

        //
        /*Rup = Rup || mouseY > 0.25f;
        Rdown = Rdown || mouseY < -0.25f;
        Rleft = Rleft || mouseX < -0.25f;
        Rright = Rright || mouseX > 0.25f;*/



    }

    //
    public static void GetKeyboard1Input(ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool jump, ref bool dash, ref bool special, ref bool leftPunch, ref bool rightPunch, ref bool Rup, ref bool Rdown, ref bool Rleft, ref bool Rright, ref bool drink, ref bool leftKick, ref bool rightKick)
    {
        if (IsBlocked)
            return;
        //

        mouseY = Input.GetAxis("Mouse Y") * 1f;

        mouseY = Mathf.Clamp(mouseY, -1, 1);

        mouseX = Input.GetAxis("Mouse X") * 1f;

        mouseX = Mathf.Clamp(mouseX, -1, 1);

        fire = fire || Input.GetKey(keyPlayer1Fire);
        special = special || Input.GetKey(keyPlayer1Special);
        jump = jump || Input.GetKeyDown(keyPlayer1Jump);
        up = up || Input.GetKey(keyPlayer1Up);
        down = down || Input.GetKey(keyPlayer1Down);
        left = left || Input.GetKey(keyPlayer1Left);
        right = right || Input.GetKey(keyPlayer1Right);

        drink = drink || Input.GetKey(keyPlayer1Drink);
        //
        Rup = Rup || Input.GetKey(keyPlayer1Lift);
        Rdown = Rdown || Input.GetKey(keyPlayer1Drop);
        // Debug.Log("mouseX : " + mouseX + " | mouseY : " + mouseY);

        jump = jump || mouseY > 0.5;
        //Mobile touch control stuff tewmporary solution please change this 
        //this is for randomized attack
       // if (Input.GetMouseButtonDown(0))
       //      b = Random.Range(1, 5);

       //// Debug.Log("b : "+b);

       // switch (b)
       // {
       //     case 1: // if a is an integer
       //         leftPunch = leftPunch || Input.GetMouseButton(0);
       //        // Debug.Log("leftPunch");
       //         break;
       //     case 2: // if a is a string
       //         rightPunch = rightPunch || Input.GetMouseButton(0);
       //        // Debug.Log("rightPunch");
       //         break;
       //     case 3: // if a is an integer
       //         leftKick = leftKick || Input.GetMouseButton(0);
       //         //Debug.Log("leftKick");
       //         break;
       //     case 4: // if a is a string
       //         rightKick = rightKick || Input.GetMouseButton(0);
       //        // Debug.Log("rightKick");
       //         break;
            
       // }
        
        leftPunch = leftPunch || Input.GetMouseButton(0);
        rightPunch = rightPunch || Input.GetMouseButton(1);

        leftKick = leftKick || Input.GetMouseButton(0);
        rightKick = rightKick || Input.GetMouseButton(1);
        
       
        /* Rleft = Rleft || mouseX < -0.25f;
        Rright = Rright || mouseX > 0.25f;*/

        //
        /*Rup = Rup || mouseY > 0.25f;
        Rdown = Rdown || mouseY < -0.25f;
        Rleft = Rleft || mouseX < -0.25f;
        Rright = Rright || mouseX > 0.25f;*/



    }
    //
    public static void GetKeyboard2Input(ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool jump, ref bool dash, ref bool special)
    {

        if (IsBlocked)
            return;
        //
        fire = fire || Input.GetKey(keyPlayer2Fire);
        special = special || Input.GetKey(keyPlayer2Special);
        up = up || Input.GetKey(keyPlayer2Up);


        down = down || Input.GetKey(keyPlayer2Down);
        left = left || Input.GetKey(keyPlayer2Left);
        right = right || Input.GetKey(keyPlayer2Right);

    }
    //
    public static void GetXBoxControllerInput(string controllerIDString, ref bool up, ref bool down, ref bool left, ref bool right, ref bool fire, ref bool jump, ref bool dash, ref bool special, ref bool leftPunch, ref bool rightPunch, ref bool Rup, ref bool Rdown, ref bool Rleft, ref bool Rright, ref bool drink, ref bool leftKick, ref bool rightKick)
    {
        //
        //  *********************   controllerIDString  equals "_1" to "_4"
        //
        if (IsBlocked)
            return;
        //
        fire = fire || (Input.GetButton("X" + controllerIDString));
        jump = jump || Input.GetButtonDown("A" + controllerIDString);
        special = special || Input.GetButton("B" + controllerIDString);

        leftPunch = leftPunch || Input.GetButton("LB" + controllerIDString);
        rightPunch = rightPunch || Input.GetButton("RB" + controllerIDString);

        leftKick = leftKick || Input.GetAxisRaw("TriggersL" + controllerIDString) > 0.25f;
        rightKick = rightKick || Input.GetAxisRaw("TriggersR" + controllerIDString) > 0.25f;
        //
        up = up || Input.GetAxisRaw("DPad_YAxis" + controllerIDString) > 0.25f || Input.GetAxisRaw("L_YAxis" + controllerIDString) < -0.25f;
        down = down || Input.GetAxisRaw("DPad_YAxis" + controllerIDString) < -0.25f || Input.GetAxisRaw("L_YAxis" + controllerIDString) > 0.25f;
        left = left || Input.GetAxisRaw("DPad_XAxis" + controllerIDString) < -0.25f || Input.GetAxisRaw("L_XAxis" + controllerIDString) < -0.25f;
        right = right || Input.GetAxisRaw("DPad_XAxis" + controllerIDString) > 0.25f || Input.GetAxisRaw("L_XAxis" + controllerIDString) > 0.25f;
        //
        // dash = dash || Input.GetAxisRaw("TriggersR" + controllerIDString) > 0.25f || Input.GetAxisRaw("TriggersL" + controllerIDString) > 0.25f;

        drink = drink || Input.GetButton("Y" + controllerIDString);

        //
        Rup = Rup || Input.GetAxisRaw("R_YAxis" + controllerIDString) < -0.25f;
        Rdown = Rdown || Input.GetAxisRaw("R_YAxis" + controllerIDString) > 0.25f;
        Rleft = Rleft || Input.GetAxisRaw("R_XAxis" + controllerIDString) < -0.25f;
        Rright = Rright || Input.GetAxisRaw("R_XAxis" + controllerIDString) > 0.25f;

    }
    //
    public static int GetControllerPressingStart()
    {
        if (IsBlocked)
            return -1;
        //

        if (Input.GetButtonDown("Start_1"))
        {
            return 2;
        }
        if (Input.GetButtonDown("Start_2"))
        {
            return 3;
        }
        if (Input.GetButtonDown("Start_3"))
        {
            return 4;
        }
        if (Input.GetButtonDown("Start_4"))
        {
            return 5;
        }

        return -1;
    }
    //
    public static bool GetKeyDown(KeyCode key)
    {
        if (IsBlocked)
            return false;
        //
        //
        return Input.GetKeyDown(key);
    }
    //
    public static bool GetButtonDown(string button)
    {
        if (IsBlocked)
            return false;
        //
        return Input.GetButtonDown(button);
    }
    //

    
}