using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointFix : MonoBehaviour {

     Vector3 strtPos;
     ConfigurableJoint joint;
     Vector3 jointAnchor;
    Quaternion strtRot;
    // Use this for initialization

    bool started =  false;
    void Start () {
        strtPos = transform.localPosition;
        strtRot = transform.localRotation;
        joint = transform.GetComponent<ConfigurableJoint>();
        jointAnchor = joint.connectedAnchor;
        started = true;
    }

    void OnDisable()
    {
        //Debug.Log("PrintOnDisable: script was disabled");
    }

    

    private void OnEnable()
    {
        if (!started)
            return;
        //Debug.Log("PrintOnEnable: script was enabled");
        //if (strtPos == Vector3.zero) return;
        transform.localPosition = strtPos;
        transform.localRotation = strtRot;
        joint.connectedAnchor = jointAnchor;
        joint.anchor = Vector3.zero;
        joint.autoConfigureConnectedAnchor = false;
    }
    // Update is called once per frame
    void Update () {
        
    }
}
