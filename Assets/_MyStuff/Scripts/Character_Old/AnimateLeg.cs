using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateLeg : MonoBehaviour {
    public float forward;
    public float sideways;

    public Animator anim;
    public PlayerController1 pcntrl;
    // Use this for initialization
    void Start () {

       // anim = GetComponent<Animator>();

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

         
        //print("Forward : " + forwardAmount);
       // print("Sideways : " + turnAmount);
        forward = forwardAmount;
        sideways = turnAmount;


        anim.SetBool("LockOn", true);
        anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("Sideways", turnAmount, 0.1f, Time.deltaTime);

    }

    // Update is called once per frame
    void Update () {

        ConvertMoveInputAndPassItToAnimator(pcntrl.inputDirection);
        

    }
}
