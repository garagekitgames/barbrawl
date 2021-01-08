using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace garagekitgames
{
    public class FootstepSound : MonoBehaviour
    {
        bool once;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            Transform collisiontransform = collision.transform;
            if (transform.root != collisiontransform.root && !once)
            {
                once = true;
                if(collisiontransform.name.Contains("Foot"))
                {

                    EffectsController.Instance.PlayFootStepSound(collision.contacts[0].point, collision.impulse.sqrMagnitude / 10, collision.collider.tag);
                    //AudioManager.instance.Play("WoodImpact" + Random.Range(1, 4));

                }
                if(collisiontransform.name.Contains("chest") || collisiontransform.name.Contains("hip") || collisiontransform.name.Contains("head"))
                {
                    EffectsController.Instance.CreateSmokeEffect(collision.contacts[0].point, collision.impulse.magnitude);
                    AudioManager.instance.Play("WoodImpact" + Random.Range(1, 4));
                    //AudioManager.instance.Play("WoodCrack" + Random.Range(1, 3));
                }
                /*if (collisiontransform.CompareTag("Cash") || collisiontransform.name.Contains("CashFinal"))
                {
                    //EffectsController.Instance.CreateSmokeEffect(collision.contacts[0].point, collision.impulse.magnitude);
                    AudioManager.instance.Play("CoinDrop1");
                }*/
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            Transform collisiontransform = collision.transform;
            if (transform.root != collisiontransform.root && once)
            {
                once = false;

            }
        }
    }

}
