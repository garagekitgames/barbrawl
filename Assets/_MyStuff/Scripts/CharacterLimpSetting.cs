using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames
{
    public class CharacterLimpSetting : MonoBehaviour
    {
        public List<BodyPart> bodyParts;

        public bool resetHipRotation = true; // use this for stiff active ragdoll, which has constraints on the hip rigid body;

        public BodyPart bodyPartToFixConstraints;

        public CharacterThinker character;

        public bool looseCharacter = false;


        public bool isLimp = false;
        public bool isLimping = false;

        public Quaternion targetRotation;


        // Use this for initialization
        void Awake()
        {
            character = transform.GetComponent<CharacterThinker>();
        }

        private void Start()
        {
            targetRotation = character.bpHolder.bodyParts[bodyPartToFixConstraints].transform.rotation;

        }
        // Update is called once per frame
        void Update()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {

            //Go limp as soon as grabbed
            if (character.isGrabbed && !isLimp)
            {


                if (!character.isHurting)
                {
                    StartCoroutine(DoGrabHurt(0.5f));
                    isLimp = true;
                }


            }





            /*if (!character.isGrabbed && isLimp)
            {
                
                // cachedRigidbody.useGravity = true;
                if (!isLimping)
                {
                    isLimp = false;
                    StartCoroutine(DoHurt(0.5f));
                }
                    
               // newResetLimp();
                
                //limpSetting.resetHurtLimp();
            }*/
        }

        IEnumerator DoGrabHurt(float sec)
        {
            goLimp();
            character.isHurting = true;
            //goLimp();
            //isLimping = true;
            yield return new WaitForSeconds(sec);
            while (character.isGrabbed)
            {
                yield return new WaitForFixedUpdate();
            }
            while (!character.grounded)
            {
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(sec);

            if (character.health.alive && !character.health.knockdown)
            {
                newResetLimp();

            }
            else
            {
                character.isHurting = false;
            }
            isLimp = false;
            //isHurting = false;



        }


        public void hurtLimp()
        {
            targetRotation = character.bpHolder.bodyParts[bodyPartToFixConstraints].transform.rotation;

            character.enableDrag = false;
            character.legs.enabled = false;





            foreach (BodyPart bodyPart in bodyParts)
            {
                BodyPartMono bodyPartMono = character.bpHolder.bodyParts[bodyPart];

                if (bodyPartMono.BodyPartMaintainHeight)
                {
                    CharacterMaintainHeight bpMH = bodyPartMono.BodyPartMaintainHeight;
                    bpMH.enabled = false;
                }

                if (bodyPartMono.BodyPartFaceDirection)
                {
                    CharacterFaceDirection bpFD = bodyPartMono.BodyPartFaceDirection;
                    bpFD.enabled = false;
                }

            }
        }


        public void resetHurtLimp()
        {
            character.enableDrag = true;
            character.legs.enabled = true;
            // isLimp = false;




            foreach (BodyPart bodyPart in bodyParts)
            {
                BodyPartMono bodyPartMono = character.bpHolder.bodyParts[bodyPart];

                if (bodyPartMono.BodyPartMaintainHeight)
                {
                    CharacterMaintainHeight bpMH = bodyPartMono.BodyPartMaintainHeight;
                    bpMH.enabled = true;
                }

                if (bodyPartMono.BodyPartFaceDirection)
                {
                    CharacterFaceDirection bpFD = bodyPartMono.BodyPartFaceDirection;
                    bpFD.enabled = true;
                }

            }
        }



        public void goLimp()
        {
            targetRotation = character.bpHolder.bodyParts[bodyPartToFixConstraints].transform.rotation;
            if (!looseCharacter)
            {
                BodyPartMono bodyPartMono2 = character.bpHolder.bodyParts[bodyPartToFixConstraints];
                bodyPartMono2.BodyPartRb.constraints = RigidbodyConstraints.None;

                
            }
            //if (resetHipRotation)
            //{

            //}

            BodyPartMono bodyPartMono1 = character.bpHolder.bodyParts[bodyPartToFixConstraints];

            if (bodyPartMono1)
            {
                QuadraticDrag qD = bodyPartMono1.GetComponent<QuadraticDrag>();
                if (qD)
                {
                    qD.enabled = false;
                }

            }

            character.enableDrag = false;
            character.legs.enabled = false;

            

            

            foreach (BodyPart bodyPart in bodyParts)
            {
                BodyPartMono bodyPartMono = character.bpHolder.bodyParts[bodyPart];

                if(bodyPartMono.BodyPartMaintainHeight)
                {
                    CharacterMaintainHeight bpMH = bodyPartMono.BodyPartMaintainHeight;
                    bpMH.enabled = false;
                }

                if (bodyPartMono.BodyPartFaceDirection)
                {
                    CharacterFaceDirection bpFD = bodyPartMono.BodyPartFaceDirection;
                    bpFD.enabled = false;
                }

            }
           
        }

        public IEnumerator LerpToAngle()
        {
            BodyPartMono bodyPartMono2 = character.bpHolder.bodyParts[bodyPartToFixConstraints];
            //bodyPartMono.BodyPartRb.constraints = RigidbodyConstraints.None;
            bodyPartMono2.BodyPartRb.velocity = Vector3.zero;
            bodyPartMono2.BodyPartRb.angularVelocity = Vector3.zero;
            bodyPartMono2.BodyPartRb.isKinematic = true;
            bodyPartMono2.BodyPartRb.detectCollisions = false;

            if (!looseCharacter)
            {
                Quaternion currentYAngle = bodyPartMono2.bodyPartTransform.rotation;
                Quaternion _targetRotation = targetRotation;// Quaternion.Euler(new Vector3(90, 0, 0));
                while (!(currentYAngle == targetRotation))
                {

                    //float yAngle = Mathf.SmoothDampAngle(bodyPartMono2.bodyPartTransform.localEulerAngles.y, 90f, ref yVelocity, smooth);

                    // float angle = Mathf.LerpAngle(bodyPartMono2.bodyPartTransform.localEulerAngles.y, 90f, Time.time);
                    // bodyPartMono2.bodyPartTransform.eulerAngles = new Vector3(bodyPartMono2.bodyPartTransform.localRotation.x, angle, 0);


                    bodyPartMono2.bodyPartTransform.rotation = Quaternion.RotateTowards(bodyPartMono2.bodyPartTransform.rotation, _targetRotation, 360f * Time.deltaTime);
                    currentYAngle = bodyPartMono2.bodyPartTransform.rotation;
                    //bodyPartMono2.bodyPartTransform.localEulerAngles.Set(bodyPartMono2.bodyPartTransform.localEulerAngles.x, yAngle, bodyPartMono2.bodyPartTransform.localEulerAngles.z);
                    yield return new WaitForFixedUpdate();
                }

                bodyPartMono2.BodyPartRb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }

            bodyPartMono2.BodyPartRb.isKinematic = false;
            bodyPartMono2.BodyPartRb.detectCollisions = true;

            BodyPartMono bodyPartMono1 = character.bpHolder.bodyParts[bodyPartToFixConstraints];
            if (bodyPartMono1)
            {
                QuadraticDrag qD = bodyPartMono1.GetComponent<QuadraticDrag>();
                if (qD)
                {
                    qD.enabled = true;
                }

            }

            character.enableDrag = true;
            character.legs.enabled = true;





            foreach (BodyPart bodyPart in bodyParts)
            {
                BodyPartMono bodyPartMono = character.bpHolder.bodyParts[bodyPart];

                if (bodyPartMono.BodyPartMaintainHeight)
                {
                    CharacterMaintainHeight bpMH = bodyPartMono.BodyPartMaintainHeight;
                    bpMH.enabled = true;
                }

                if (bodyPartMono.BodyPartFaceDirection)
                {
                    CharacterFaceDirection bpFD = bodyPartMono.BodyPartFaceDirection;
                    bpFD.enabled = true;
                }

            }

            if (character.isHurting)
            {
                character.isHurting = false;
            }

            /*if (isLimping)
            {
                isLimping = false;
            }*/

        }


        public void newResetLimp()
        {
            StartCoroutine(LerpToAngle());
        }


        public void resetLimp()
        {
            BodyPartMono bodyPartMono2 = character.bpHolder.bodyParts[bodyPartToFixConstraints];
            //bodyPartMono.BodyPartRb.constraints = RigidbodyConstraints.None;
            bodyPartMono2.BodyPartRb.velocity = Vector3.zero;
            bodyPartMono2.BodyPartRb.angularVelocity = Vector3.zero;
            bodyPartMono2.BodyPartRb.isKinematic = true;
            bodyPartMono2.BodyPartRb.detectCollisions = false;
            //if (resetHipRotation)
            //{
            if (!looseCharacter)
            {
                bodyPartMono2.bodyPartTransform.localEulerAngles = new Vector3(0, 90, 0);
                // transform.localEulerAngles.
                bodyPartMono2.BodyPartRb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                

                
            }
               

            
            //}
            bodyPartMono2.BodyPartRb.isKinematic = false;
            bodyPartMono2.BodyPartRb.detectCollisions = true;

            BodyPartMono bodyPartMono1 = character.bpHolder.bodyParts[bodyPartToFixConstraints];
            if (bodyPartMono1)
            {
                QuadraticDrag qD = bodyPartMono1.GetComponent<QuadraticDrag>();
                if (qD)
                {
                    qD.enabled = true;
                }

            }

            character.enableDrag = true;
            character.legs.enabled = true;

            

            

            foreach (BodyPart bodyPart in bodyParts)
            {
                BodyPartMono bodyPartMono = character.bpHolder.bodyParts[bodyPart];

                if (bodyPartMono.BodyPartMaintainHeight)
                {
                    CharacterMaintainHeight bpMH = bodyPartMono.BodyPartMaintainHeight;
                    bpMH.enabled = true;
                }

                if (bodyPartMono.BodyPartFaceDirection)
                {
                    CharacterFaceDirection bpFD = bodyPartMono.BodyPartFaceDirection;
                    bpFD.enabled = true;
                }

            }
        }
    }

}
