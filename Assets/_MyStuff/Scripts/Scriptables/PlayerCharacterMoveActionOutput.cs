using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/Actitons/PlayerMovementActionOutput")]
    public class PlayerCharacterMoveActionOutput : CharacterAction
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
        //public bool enableDrag;

        public void Awake()
        {
            //chestMap = bodyParts.First(t => t.bodyPartName == "Chest");
            // chest = chestMap.bodyPart;
        }

        public override void OnFixedUpdate(CharacterThinker character)
        {

            if (!character.health.alive || character.health.knockdown || character.isHurting)// if (character.health.knockdown) //if( !character.health.alive || character.health.knockdown || character.isHurting)
            {
                return;
            }

            Vector3 inputDirection = character.inputDirection;
            bool walking = character.walking;
            float speed = character.speed;
            BodyPartMono hipPart = character.bpHolder.bodyParts[hip];

            if (walking)
            {

               
                hipPart.BodyPartRb.AddForce(inputDirection * speed);

            }
            

           
            
            // Gizmos.DrawRay(hipPart.BodyPartTransform.position, inputDirection);

            if (character.enableDrag)
                ApplyStandingAndWalkingDrag(inputDirection, hipPart.BodyPartRb);



        }

        public override void OnUpdate(CharacterThinker character)
        {
           
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
