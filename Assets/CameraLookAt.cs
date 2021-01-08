using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLookAt : MonoBehaviour {

    public Transform target;

    public int speed = 5;

    //public CinemachineStateDrivenCamera stateCamera;
    public CinemachineVirtualCamera virtCamera;

    private void Awake()
    {
        //stateCamera.LookAt = target;
        //stateCamera.Follow = target;

        virtCamera.LookAt = target;
        virtCamera.Follow = target;
    }
    // Update is called once per frame
    void Update()
    {
        
        //var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        //stateCamera.LookAt = target;
        //stateCamera.Follow = target;


        virtCamera.LookAt = target;
        virtCamera.Follow = target;
    }
}
