using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter3DCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    public GameObject charA, charB;
    public float distance = 1f;
    public float upDistance = 1f;
    public float snappyness = 5f;

    public Transform camTransform;

    public float maxZoomOut = 6f;
    // Update is called once per frame
    void Update()
    {
        Vector3 middle = (charA.transform.position + charB.transform.position) / 2f;

        Vector3 betweenVector = charB.transform.position - charA.transform.position;
        Vector3 perpendicular = Vector3.Cross(betweenVector, Vector3.up);

        // perpendicular.Normalize();

        //print("Camera Perp : " + perpendicular);
        Vector3 tempPerp = perpendicular;

        if (Mathf.Abs(perpendicular.x) > maxZoomOut)
            tempPerp.x = Mathf.Sign(perpendicular.x) * maxZoomOut;

        if (Mathf.Abs(perpendicular.z) > maxZoomOut)
            tempPerp.z = Mathf.Sign(perpendicular.z) * maxZoomOut;

       // print("Camera Perp : " + perpendicular);
      //  print("Camera Temp Perp : " + tempPerp);

        Vector3 targetCamPosition = middle + tempPerp * distance + Vector3.up * upDistance;
        //Vector3 targetCamPosition = middle + tempPerp * distance + Vector3.up * upDistance;

        camTransform.position = Vector3.Lerp(camTransform.position, targetCamPosition, Time.deltaTime * snappyness);
        camTransform.LookAt(middle);
    }
}
