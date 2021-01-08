using UnityEngine;
using System.Collections;


public class CharacterInput : MonoBehaviour
{

    public bool left = false;
    public bool right = false;
    public bool up = false;
    public bool down = false;
    bool fire = false;
    bool special = false;
    bool jump = false;
    bool extra = false;


    //
    bool wasJump = false;
    bool wasFire = false;
    bool wasSpecial = false;
    bool wasUp = false;
    bool wasDown = false;
    bool wasLeft = false;
    bool wasRight = false;

    public bool leftPunch = false;
    public bool rightPunch = false;
    public bool wasLeftPunch = false;
    public bool wasRightPunch = false;

    public bool leftKick = false;
    public bool rightKick = false;
    public bool wasLeftKick = false;
    public bool wasRightKick = false;

    //
    bool Rleft = false;
    bool Rright = false;
    bool Rup = false;
    bool Rdown = false;

    bool drink = false;
    bool wasdrunk = false;
    //
    public int controllerID = 0;
    public float timerValue = 0.17f;
    //
    void Start()
    {

    }


    void Update()
    {
        //
        wasLeft = left;
        wasRight = right;
        //  
        wasFire = fire;
        wasSpecial = special;
        wasJump = jump;
        wasUp = up;
        wasDown = down;

        wasLeftPunch = leftPunch;
        wasRightPunch = rightPunch;

        wasLeftKick = leftKick;
        wasRightKick = rightKick;

        wasdrunk = drink;
        up = down = left = right = fire = jump = extra = special = leftPunch = rightPunch = drink = leftKick = rightKick = false;
        //
        Rup = Rdown = Rleft = Rright = false;
        if (controllerID < 5)
        {
            InputReader.GetInput(controllerID, ref up, ref down, ref left, ref right, ref fire, ref jump, ref extra, ref special, ref leftPunch, ref rightPunch, ref Rup, ref Rdown, ref Rleft, ref Rright, ref drink,  ref leftKick, ref rightKick, timerValue);
        }
    }
    //
    public bool Jump()
    {
        return (jump && !wasJump);
    }
    //
    public bool PressFire()
    {
        return fire && !wasFire;
    }
    //
    public bool ReleaseFire()
    {
        return !fire && wasFire;
    }
    //
    public bool HoldFire()
    {
        return fire;
    }
    //
    public bool PressSpecial()
    {
        return special && !wasSpecial;
    }
    //
    public bool ReleaseSpecial()
    {
        return !special && wasSpecial;
    }
    //
    public bool HoldSpecial()
    {
        return special;
    }
    public bool PressLeft()
    {
        return left && !wasLeft;
    }
    public bool PressRight()
    {
        return right && !wasRight;
    }
    public bool PressUp()
    {
        return up && !wasUp;
    }
    public bool PressDown()
    {
        return down && !wasDown;
    }
    public bool HoldLeft()
    {
        return left && !right;
    }
    //
    public bool HoldRight()
    {
        return right && !left;
    }
    //
    public bool HoldUp()
    {
        return up && !down;
    }
    //
    public bool HoldDown()
    {
        return down && !up;
    }
    //Punches 
    //Left
    public bool PressLeftPunch()
    {
        return leftPunch && !wasLeftPunch;
    }
    //
    public bool ReleaseLeftPunch()
    {
        return !leftPunch && wasLeftPunch;
    }
    //
    public bool HoldLeftPunch()
    {
        return leftPunch;
    }

    //Right
    public bool PressRightPunch()
    {
        return rightPunch && !wasRightPunch;
    }
    //
    public bool ReleaseRightPunch()
    {
        return !rightPunch && wasRightPunch;
    }
    //
    public bool HoldRightPunch()
    {
        return rightPunch;
    }

    //Kicks 
    //Left
    public bool PressLeftKick()
    {
        return leftKick && !wasLeftKick;
    }
    //
    public bool ReleaseLeftKick()
    {
        return !leftKick && wasLeftKick;
    }
    //
    public bool HoldLeftKick()
    {
        return leftKick;
    }

    //Right
    public bool PressRightKick()
    {
        return rightKick && !wasRightKick;
    }
    //
    public bool ReleaseRightKick()
    {
        return !rightKick && wasRightKick;
    }
    //
    public bool HoldRightKick()
    {
        return rightKick;
    }

    //Right Stick Inputs
    public bool RHoldLeft()
    {
        return Rleft && !Rright;
    }
    //
    public bool RHoldRight()
    {
        return Rright && !Rleft;
    }
    //
    public bool RHoldUp()
    {
        return Rup && !Rdown;
    }
    //
    public bool RHoldDown()
    {
        return Rdown && !Rup;
    }

    //
    public bool PressDrink()
    {
        return drink && !wasdrunk;
    }
    //
    public bool ReleaseDrink()
    {
        return !drink && wasdrunk;
    }
    //
    public bool HoldDrink()
    {
        return drink;
    }

}
