using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InControl;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/Actitons/PlayerMovementActionInput")]
    public class PlayerCharacterMoveActionInput : CharacterAction
    {
        /*[Serializable]
        public class PlayerBodyPartMap
        {
            public string bodyPartName;
            public BodyPart bodyPart;
        }*/

        //public List<BodyPart> bodyParts = new List<BodyPart>();

        // public PlayerBodyPartMap chestMap = new PlayerBodyPartMap();

        public BodyPart chest;
        public BodyPart hip;
        public BodyPart head;
        //public float speed;
       // public bool enableDrag;

        public void Awake()
        {
            //chestMap = bodyParts.First(t => t.bodyPartName == "Chest");
            // chest = chestMap.bodyPart;
        }

        public override void OnFixedUpdate(CharacterThinker character)
        {

        }

        public override void OnUpdate(CharacterThinker character)
        {
            Vector3 inputDirection = Vector3.zero;
            float moveMagnitude;
            //character.Remember("inputDirection", inputDirection);
            //Vector3 currentFacing = Vector3.zero;

            // Debug.Log(character.Remember<Vector3>("inputDirection"));

            //var inputDevice = TouchManager.Device;

            //inputDevice.LeftStick.HasChanged
            if (character.input.HoldRight())
            {
                inputDirection += Vector3.right;


            }

            if (character.input.HoldLeft())
            {

                inputDirection += Vector3.left;

            }

            if (character.input.HoldUp())
            {
                inputDirection += Vector3.forward;

            }

            if (character.input.HoldDown())
            {
                inputDirection += Vector3.back;

            }

            //inputDirection = new Vector3(inputDevice.LeftStickX, 0, inputDevice.LeftStickY);
            moveMagnitude = inputDirection.magnitude;
            if (inputDirection != Vector3.zero)
            {
                //turn this to true to make him walk
                character.walking = true;

                //justStarted = true;

                inputDirection.Normalize();
                if (true)//camerabased movement
                {
                    inputDirection = Camera.main.transform.TransformDirection(inputDirection);
                    inputDirection.y = 0.0f;
                }

               
                /* BodyPartMono chestPart = character.bpHolder.bodyParts[chest];
                 if (character.bpHolder.bodyParts.TryGetValue(chest, out chestPart))
                 {

                 }*/
                /*currentFacing = chestPart.BodyPartRb.transform.forward;
                currentFacing.y = 0;
                currentFacing.Normalize();*/
                /*if (character.legs)
                {
                    if (!legs.walking)
                    {
                        legs.StartWalking();
                    }
                }
                if (!targetting)
                {

                    faceDirection.facingDirection = inputDirection;
                    hipFaceDirection.facingDirection = inputDirection;
                    headFaceDirection.facingDirection = inputDirection;
                }
                else
                {
                    faceDirection.facingDirection = targetDirection;
                    hipFaceDirection.facingDirection = targetDirection;
                    headFaceDirection.facingDirection = targetDirection;
                }*/



            }
            else
            {
                inputDirection = character.currentFacing;
                character.walking = false;
                /* if (legs)
                 {
                     if (legs.walking)
                     {
                         legs.StopWalking();
                     }
                 }

                 if (!targetting)
                 {

                     faceDirection.facingDirection = currentFacing;
                     hipFaceDirection.facingDirection = currentFacing;
                     headFaceDirection.facingDirection = currentFacing;
                 }
                 else
                 {
                     faceDirection.facingDirection = targetDirection;
                     hipFaceDirection.facingDirection = targetDirection;
                     headFaceDirection.facingDirection = targetDirection;
                 }*/



            }
            character.inputDirection = inputDirection;
            character.moveMagnitude = moveMagnitude;
        }

        private void ApplyStandingAndWalkingDrag(Vector3 inputDirection, Rigidbody rb)
        {
            // ***********  APPLY DRAGS! **
            //
            // THIS, along with the powerful facing direction forces, ACTUALLY MAKES THE CHARACTERS LESS INTERACTIBLE, BECAUSE THEY CAN'T PUSH EACH OTHER MUCH *****
            // SOFTER FORCES CAN BE BETTER, BUT THOSE NEED MORE TWEEKING, IDEALLY JUST ENOUGH FORCE TO ACHIEVE THE EFFECT WITHOUT BECOMING LOCKED INTO THAT POSITION OR DIRECTION ***
            //
            if (inputDirection == Vector3.zero)
            {
                // ***** WHEN STANDING STILL, APPLY A DRAG BASED ON HOW FAST THE TORSO IS TRAVELLING ***
                //
                Vector3 horizontalVelocity = rb.velocity;
                horizontalVelocity.y = 0;
                //
                float speed = horizontalVelocity.magnitude;
                //
                rb.velocity *= (1 - Mathf.Clamp(speed * 20f + 10, 0, 50) * Time.fixedDeltaTime);
            }
            else
            {
                // ***** APPLY A POWERFUL DRAG FORCE IF THE TORSO ISN'T TRAVELLING IN THE INPUT DIRECITON, ALLOWS FOR TIGHT TURNS ***
                //
                Vector3 horizontalVelocity = rb.velocity;
                horizontalVelocity.y = 0;
                float m = 1 - (1 + Vector3.Dot(horizontalVelocity.normalized, inputDirection)) / 2f;
                rb.velocity *= (1 - (m * 30) * Time.fixedDeltaTime);
                //faceDirection.facingDirection = targetDirection;


                //float m = 1 - (1 + Vector3.Dot(horizontalVelocity.normalized, inputDirection)) / 2f;

            }
            //
        }

    }

}
