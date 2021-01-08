using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace garagekitgames
{
    public class CharacterCollisionHandler : MonoBehaviour
    {

        public bool checkCollision = false;
        public bool checkDamage = false;
        public bool isAttackItem = false;


        private bool once = false;
        public Color damageIndicationColor;

        public CharacterThinker myBrain;

        public Rigidbody Rb;

        private void OnCollisionEnter(Collision collision)
        {

            Transform collisiontransform = collision.transform;
            if (transform.root != collisiontransform.root && !once)
            {
                if (checkDamage)
                {
                    //InteractableObject component = collision.transform.GetComponent<InteractableObject>();
                    Vector3 relativeVelocity = collision.relativeVelocity;
                    float magnitude = relativeVelocity.magnitude;
                    Rigidbody rigidbody = collision.rigidbody;
                    Collider collider = collision.collider;
                    ContactPoint[] contacts = collision.contacts;
                    //print("Impulse MAgnitude : " + collision.impulse.magnitude);
                    // float impulseMagnitude = collision.impulse.magnitude;
                    float impulseMagnitude = rigidbody.mass * magnitude;

                    /* print("**********************************************************************************");
                    // print("Punch Damage : " + num);
                     print("Punch Damage Magnitude : " + magnitude);
                     print("Punch Damage Velocity : " + relativeVelocity);
                     print("Punch Damage impulseMagnitude by 10 : " + impulseMagnitude);
                     */

                    DamageCheck(transform, rigidbody, collider, contacts, relativeVelocity, magnitude, impulseMagnitude);
                    once = true;
                }


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

        private void DamageCheck(Transform collisionTransform, Rigidbody collisionRigidbody, Collider collisionCollider, ContactPoint[] collisionContacts, Vector3 relativeVelocity, float velocityMagnitude, float impulseMagnitude)
        {
            //for (int i = 0; i < collisionContacts.Length; i++)
            // {
            ContactPoint contactPoint = collisionContacts[0];

            float num = 0f;
            // if (impulseMagnitude / 10 > 1)
            // {


            //  }
            //num = impulseMagnitude * collisionRigidbody.mass / rb.mass;
            num = impulseMagnitude;
            Rb.AddForce(contactPoint.normal * num, ForceMode.Impulse);
            //rb.AddExplosionForce(num * 400 , contactPoint.normal, 10000);


            if (num > 5)
            {

                /*print("**********************************************************************************");
                print("Punch Damage : " + num);
                print("Punch Damage Magnitude : " + velocityMagnitude);
                print("Punch Damage Velocity : " + relativeVelocity);
                print("Punch Damage impulseMagnitude : " + impulseMagnitude);*/
                EffectsController.Instance.PlayImpactSound(collisionContacts[0].point, num, collisionCollider.tag);
                //if (collisionRigidbody.transform.root.GetComponent<ForceTests>().ControlledBy == ForceTests.ControlledTypes.Human)
                //{
                EffectsController.Instance.Shake(num, 0.1f);

                FloatingTextController.CreateFloatingText("-" + Mathf.RoundToInt(num).ToString(), contactPoint.point, damageIndicationColor);
                //myBrain.healthHandler.AddDamage();
                contactPoint.thisCollider.attachedRigidbody.AddForce((contactPoint.normal + Vector3.up ) *  num * 2, ForceMode.VelocityChange);
            }

            // }

        }

        // Use this for initialization
        void Start()
        {
            myBrain = transform.root.gameObject.GetComponent<CharacterThinker>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}