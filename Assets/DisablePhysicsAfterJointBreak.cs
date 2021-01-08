using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePhysicsAfterJointBreak : MonoBehaviour {

    Rigidbody myRB;
    IEnumerator coroutine;
    // Use this for initialization
    void Start () {

        myRB = this.GetComponent<Rigidbody>();
		
	}

    void OnJointBreak(float breakForce)
    {
        //Debug.Log("A joint has just been broken!, force: " + breakForce);
        coroutine = LateCall(3);
        StartCoroutine(coroutine);
    }

    IEnumerator LateCall(float sec)
    {

        yield return new WaitForSeconds(sec);

        myRB.detectCollisions = false;
        myRB.isKinematic = true;
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());

        // currentPlayer.gameObject.SetActive(value);


        //Do Function here...
    }
    // Update is called once per frame
    void Update () {
		
	}
}
