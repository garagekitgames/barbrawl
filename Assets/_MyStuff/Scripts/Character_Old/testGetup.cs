using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGetup : MonoBehaviour
{
    private Rigidbody rb;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private IEnumerator FadeOut(float alphaStart, float alphaFinish, float time)
    {
        //if (bgTexture == null)
          //  yield return null;

        float elapsedTime = 0;
        //bgTexture.alpha = alphaStart;

        while (elapsedTime < time)
        {
            //bgTexture.alpha = ;
            transform.localEulerAngles = new Vector3(0, Mathf.Lerp(0, 90, (elapsedTime / time)), 0);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }


    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            var rot = transform.rotation;
            //transform.eulerAngles = new Vector3(-12f, 90f, -2f);

            //works 
            //transform.localEulerAngles = new Vector3(0, 90, 0);
            //StartCoroutine(FadeOut(0,90, 0.1f));
            //transform.rotation = rot * Quaternion.Euler(0, 90, 0); // this is 90 degrees around y axis
            //transform.rotation = Quaternion.Euler(0, 90, 0);

            transform.localEulerAngles = new Vector3(0, 90, 0);
           // transform.localEulerAngles.
            rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            //rb.constraints = (RigidbodyConstraints)90;

            //rb.constraints = RigidbodyConstraints.FreezeRotationZ;

        }

    }
}
