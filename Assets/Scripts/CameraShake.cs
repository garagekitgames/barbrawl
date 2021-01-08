using UnityEngine;
using System.Collections;
namespace garagekitgames
{
    public class CameraShake : MonoBehaviour
    {
        // Transform of the camera to shake. Grabs the gameObject's transform
        // if null.
        public Camera camTransform;

        // How long the object should shake for.
        //public float shakeDuration = 0f;

        // Amplitude of the shake. A larger value shakes the camera harder.
        public float shakeAmount = 0.7f;
        //public float decreaseFactor = 1.0f;

        //Vector3 originalPos;

        void Awake()
        {
            if (camTransform == null)
            {
                camTransform = Camera.main;
            }
        }

        void Shake(float amt, float length)
        {
            shakeAmount = amt;
            InvokeRepeating("BeginShake", 0, 0.01f);
            Invoke("StopShake", length);
        }

        void BeginShake()
        {
            if (shakeAmount > 0)
            {
                Vector3 camPos = camTransform.transform.position;

                float shakeAmtX = Random.value * shakeAmount * 2 - shakeAmount;
                float shakeAmtY = Random.value * shakeAmount * 2 - shakeAmount;

                camPos.x += shakeAmtX;
                camPos.y += shakeAmtY;

                camTransform.transform.position = camPos;
            }
        }

        void StopShake()
        {
            CancelInvoke("BeginShake");
            camTransform.transform.localPosition = Vector3.zero;
        }



        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Shake(0.03f, 0.1f);
            }

        }
    }
}
