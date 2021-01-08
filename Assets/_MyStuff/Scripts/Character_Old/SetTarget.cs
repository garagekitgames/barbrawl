using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTarget : MonoBehaviour {

    public PlayerController1 pContrl;

    public GameObject target;
    public bool touchInput = false;

    // Use this for initialization
    void Start () {

        pContrl = GetComponent<PlayerController1>();


    }

    Ray GenerateMouseRay()
    {
        Vector3 mousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

        Vector3 mousePosFarW = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosNearW = Camera.main.ScreenToWorldPoint(mousePosNear);

        Ray mouseRay = new Ray(mousePosNearW, mousePosFarW - mousePosNearW);

        return mouseRay;



    }
	
	// Update is called once per frame
	void Update () {
       // pContrl.target = target.transform.position;
        target.transform.position = pContrl.target;
        Touch[] myTouches = Input.touches;
        if (Input.touchCount == 1)
        {
            //for (int i = 0; i < Input.touchCount; i++)
            //{
                if (myTouches[0].phase == TouchPhase.Stationary || myTouches[0].phase == TouchPhase.Moved)
                {
                    Ray mouseRay = GenerateMouseRay();
                    RaycastHit hit;

                    if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
                    {
                        // GameObject temp = Instantiate(target, hit.point, Quaternion.identity);
                        pContrl.target = hit.point;
                    }

                }
            //}
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray mouseRay = GenerateMouseRay();
                RaycastHit hit;

                if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
                {
                    // GameObject temp = Instantiate(target, hit.point, Quaternion.identity);
                    pContrl.target = hit.point;
                }

            }
        }


    }
}
