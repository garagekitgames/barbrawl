using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCenterOfMass : MonoBehaviour {

    public Rigidbody myRb;
    public Transform myCoMIndicator;
    public Transform myCoM;
    public bool dontUse;
	// Use this for initialization
	void Start () {

        myRb = this.GetComponent<Rigidbody>();
        //myCoM = transform.GetComponentInChildren<Transform>();
        


    }
	
	// Update is called once per frame
	void Update () {

        

        if (myRb != null && myCoMIndicator != null)
        {
            myRb.centerOfMass = myCoM.localPosition;
            myCoMIndicator.localPosition = myRb.centerOfMass;
        }

        if (dontUse)
        {
            return;
        }

        if (myRb != null && myCoM != null)
        {
            myRb.centerOfMass = myCoM.localPosition;
            myCoMIndicator.localPosition = myRb.centerOfMass;
        }

    }
}
