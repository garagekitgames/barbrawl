using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace garagekitgames{

    
    public class BodyPartMono : MonoBehaviour
    {
        public BodyPart bodyPart;
        public Rigidbody bodyPartRb;
        public Transform bodyPartTransform;
        public Collider bodyPartCollider;
        public ConfigurableJoint bodyPartConfigJoint;
        public CharacterMaintainHeight bodyPartMaintainHeight;
        public CharacterFaceDirection bodyPartFaceDirection;
        public CharacterUpright bodyPartUpright;

        private JointDrive initialJointDrive = new JointDrive();
        private Vector3 initialTargetAngularVelocity = Vector3.zero;

        public JointDrive deathJointDrive = new JointDrive();
        public Vector3 deathTargetAngularVelocity = Vector3.zero;

        /*public bool checkCollision = false;
        public bool checkDamage = false;
        public bool isAttackItem = false;


        private bool once = false;
        public Color damageIndicationColor;
        */
        public CharacterThinker myBrain;
        public bool groundCheck;

        public bool partOnGround;

        public BodyPart BodyPart
        {
            get
            {
                return bodyPart;
            }

            set
            {
                bodyPart = value;
            }
        }

        public Rigidbody BodyPartRb
        {
            get
            {
                return bodyPartRb;
            }

            set
            {
                bodyPartRb = value;
            }
        }

        public Transform BodyPartTransform
        {
            get
            {
                return bodyPartTransform;
            }

            set
            {
                bodyPartTransform = value;
            }
        }

        public Collider BodyPartCollider
        {
            get
            {
                return bodyPartCollider;
            }

            set
            {
                bodyPartCollider = value;
            }
        }

        public ConfigurableJoint BodyPartConfigJoint
        {
            get
            {
                return bodyPartConfigJoint;
            }

            set
            {
                bodyPartConfigJoint = value;
            }
        }

        public CharacterMaintainHeight BodyPartMaintainHeight
        {
            get
            {
                return bodyPartMaintainHeight;
            }

            set
            {
                bodyPartMaintainHeight = value;
            }
        }

        public CharacterFaceDirection BodyPartFaceDirection
        {
            get
            {
                return bodyPartFaceDirection;
            }

            set
            {
                bodyPartFaceDirection = value;
            }
        }

        public CharacterUpright BodyPartUpright
        {
            get
            {
                return bodyPartUpright;
            }

            set
            {
                bodyPartUpright = value;
            }
        }

        private void Awake()
        {
            BodyPartRb = this.GetComponent<Rigidbody>();
            BodyPartTransform = this.GetComponent<Transform>();
            BodyPartConfigJoint = this.GetComponent<ConfigurableJoint>();
            BodyPartCollider = this.GetComponent<Collider>();
            BodyPartMaintainHeight = this.GetComponent<CharacterMaintainHeight>();
            BodyPartFaceDirection = this.GetComponent<CharacterFaceDirection>();
            BodyPartUpright = this.GetComponent<CharacterUpright>();

            transform.root.gameObject.GetComponent<CharacterBodyPartHolder>().bodyParts.Add(bodyPart, this);
            transform.root.gameObject.GetComponent<CharacterBodyPartHolder>().bodyPartsName.Add(transform.name, this);

            //Debug.Log(transform.root.name);

            if (BodyPartConfigJoint != null)
            {

                initialJointDrive = BodyPartConfigJoint.slerpDrive;
                initialTargetAngularVelocity = BodyPartConfigJoint.targetAngularVelocity;
               // BodyPartMono bodyPart = character.bpHolder.bodyParts[attackPose.bodyPart];
               // JointDrive x = BodyPartConfigJoint.slerpDrive;
               // x.positionDamper = attackPose.jointDrive.x;
               // x.positionSpring = attackPose.jointDrive.y;
               // x.maximumForce = attackPose.jointDrive.z;
               //bodyPart.BodyPartConfigJoint.slerpDrive = x;

                //Vector3 jointValue = new Vector3(20f, 0f, 0f);

                //bodyPart.BodyPartConfigJoint.targetAngularVelocity = attackPose.targetAngularVelocity;
            }

        }

        private void OnEnable()
        {
            if (BodyPartConfigJoint != null)
            {
                BodyPartConfigJoint.slerpDrive = initialJointDrive;
                BodyPartConfigJoint.targetAngularVelocity = initialTargetAngularVelocity;
            }
        }

        private void OnDisable()
        {
            if (BodyPartConfigJoint != null)
            {
                BodyPartConfigJoint.slerpDrive = initialJointDrive;
                BodyPartConfigJoint.targetAngularVelocity = initialTargetAngularVelocity;
            }
        }

        public void ResetJoint(bool isRagdoll)
        {
            if (BodyPartConfigJoint != null)
            {
                if(isRagdoll)
                {
                    BodyPartConfigJoint.slerpDrive = deathJointDrive;
                    BodyPartConfigJoint.targetAngularVelocity = deathTargetAngularVelocity;
                }
                else
                {
                    BodyPartConfigJoint.slerpDrive = initialJointDrive;
                    BodyPartConfigJoint.targetAngularVelocity = initialTargetAngularVelocity;
                }
                
            }
        }


        //private void OnCollisionEnter(Collision collision)
        //{

        //    Transform collisiontransform = collision.transform;
        //    if (transform.root != collisiontransform.root && !once)
        //    {
        //        if (checkDamage)
        //        {
        //            //InteractableObject component = collision.transform.GetComponent<InteractableObject>();
        //            Vector3 relativeVelocity = collision.relativeVelocity;
        //        float magnitude = relativeVelocity.magnitude;
        //        Rigidbody rigidbody = collision.rigidbody;
        //        Collider collider = collision.collider;
        //        ContactPoint[] contacts = collision.contacts;
        //        //print("Impulse MAgnitude : " + collision.impulse.magnitude);
        //        // float impulseMagnitude = collision.impulse.magnitude;
        //        float impulseMagnitude = rigidbody.mass * magnitude;

        //        /* print("**********************************************************************************");
        //        // print("Punch Damage : " + num);
        //         print("Punch Damage Magnitude : " + magnitude);
        //         print("Punch Damage Velocity : " + relativeVelocity);
        //         print("Punch Damage impulseMagnitude by 10 : " + impulseMagnitude);
        //         */

        //            DamageCheck(transform, rigidbody, collider, contacts, relativeVelocity, magnitude, impulseMagnitude);
        //            once = true;
        //        }


        //    }

        //}

        //private void OnCollisionExit(Collision collision)
        //{
        //    Transform collisiontransform = collision.transform;
        //    if (transform.root != collisiontransform.root && once)
        //    {
        //        once = false;

        //    }
        //}

        //private void DamageCheck(Transform collisionTransform, Rigidbody collisionRigidbody, Collider collisionCollider, ContactPoint[] collisionContacts, Vector3 relativeVelocity, float velocityMagnitude, float impulseMagnitude)
        //{
        //    //for (int i = 0; i < collisionContacts.Length; i++)
        //    // {
        //    ContactPoint contactPoint = collisionContacts[0];

        //    float num = 0f;
        //    // if (impulseMagnitude / 10 > 1)
        //    // {


        //    //  }
        //    //num = impulseMagnitude * collisionRigidbody.mass / rb.mass;
        //    num = impulseMagnitude;
        //    BodyPartRb.AddForce(contactPoint.normal * num, ForceMode.Impulse);
        //    //rb.AddExplosionForce(num * 400 , contactPoint.normal, 10000);


        //    if (num > 5)
        //    {

        //        /*print("**********************************************************************************");
        //        print("Punch Damage : " + num);
        //        print("Punch Damage Magnitude : " + velocityMagnitude);
        //        print("Punch Damage Velocity : " + relativeVelocity);
        //        print("Punch Damage impulseMagnitude : " + impulseMagnitude);*/
        //        EffectsController.PlayImpactSound(collisionContacts[0].point, num, collisionCollider.tag);
        //        //if (collisionRigidbody.transform.root.GetComponent<ForceTests>().ControlledBy == ForceTests.ControlledTypes.Human)
        //        //{
        //        EffectsController.Shake(num, 0.1f);

        //        FloatingTextController.CreateFloatingText("-" + Mathf.RoundToInt(num).ToString(), contactPoint.point, damageIndicationColor);
        //        //contactPoint.thisCollider.attachedRigidbody.AddForce((contactPoint.normal + Vector3.up ) *  num * 2, ForceMode.VelocityChange);
        //    }

        //    // }

        //}

        // Use this for initialization


        private void OnCollisionEnter(Collision collision)
        {



            if (this.transform != null)
            {
                Transform transform = collision.transform;
                if (transform.root != this.transform.root)
                {
                    //InteractableObject component = collision.transform.GetComponent<InteractableObject>();
                    Vector3 relativeVelocity = collision.relativeVelocity;
                    float magnitude = relativeVelocity.magnitude;
                    Rigidbody rigidbody = collision.rigidbody;
                    Collider collider = collision.collider;
                    ContactPoint[] contacts = collision.contacts;
                    //print("Impulse MAgnitude : " + collision.impulse.magnitude);
                    float impulseMagnitude = collision.impulse.magnitude;
                    /*if (this.grabCheck)
                    {
                        this.GrabCheck(component, transform, rigidbody);
                    }*/

                    if (this.groundCheck)
                    {
                        this.GroundCheck(transform, rigidbody, collider, contacts, relativeVelocity, magnitude);
                    }


                }
            }
        }

        /*private void OnCollisionStay(Collision collision)
        {
            
            Transform transform = collision.transform;
            if (transform.root != this.transform.root)
            {
                InteractableObject component = collision.transform.GetComponent<InteractableObject>();
                Vector3 relativeVelocity = collision.relativeVelocity;
                float magnitude = relativeVelocity.magnitude;
                Rigidbody rigidbody = collision.rigidbody;
                Collider collider = collision.collider;
                ContactPoint[] contacts = collision.contacts;
                
                if (this.groundCheck)
                {

                    this.GroundCheck(transform, rigidbody, collider, contacts, relativeVelocity, magnitude);
                }

                
            }
        }*/

        private void OnCollisionExit(Collision collision)
        {
            Transform coltransform = collision.transform;
            if (coltransform.root != this.transform.root)
            {
                if (this.groundCheck)
                {
                    this.partOnGround = false;
                }

            }
        }

        private void GroundCheck(Transform collisionTransform, Rigidbody collisionRigidbody, Collider collisionCollider, ContactPoint[] collisionContacts, Vector3 relativeVelocity, float velocityMagnitude)
        {
            int num = 0;
            for (int i = 0; i < collisionContacts.Length; i++)
            {
                ContactPoint contact = collisionContacts[i];
                if (collisionTransform.gameObject.layer == 8)
                {
                    //if (this.SurfaceWithinAngle(contact, Vector3.up, 90f))
                    //{
                    num++;
                    //}


                }
                /*else if (this.SurfaceWithinAngle(contact, Vector3.up, 50f))
                {
                    num++;
                }*/
                if (num > 0)
                {
                    this.partOnGround = true;
                }
                else
                {
                    this.partOnGround = false;
                }
            }
        }

        private bool SurfaceWithinAngle(ContactPoint contact, Vector3 direction, float angle)
        {
            bool result = false;
            float num = Vector3.Angle(contact.normal, direction);
            if (num <= angle)
            {
                result = true;
            }
            return result;
        }

        void Start()
        {
            myBrain = transform.root.gameObject.GetComponent<CharacterThinker>();


        }

        
    }
}

