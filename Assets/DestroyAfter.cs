using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace garagekitgames
{
    public class DestroyAfter : MonoBehaviour
    {
        public FloatReference destroyTime;
        public Vector3 randomIntensity = new Vector3(0.5f, 0, 0);

        // Use this for initialization
        void Start()
        {
            Destroy(gameObject, destroyTime);
            transform.localPosition += new Vector3(Random.Range(-randomIntensity.x, randomIntensity.x), Random.Range(-randomIntensity.y, randomIntensity.y), Random.Range(-randomIntensity.z, randomIntensity.z));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

