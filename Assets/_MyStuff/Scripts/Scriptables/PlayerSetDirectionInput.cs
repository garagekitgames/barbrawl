using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/Actitons/PlayerSetDirectionInput")]
    public class PlayerSetDirectionInput : CharacterAction
    {
        //public BodyPart chest;
        public BodyPart hip;
        //public BodyPart head;
        //public bool targetting;
        public bool useAgent;

        public override void OnFixedUpdate(CharacterThinker character)
        {
            
        }

        public override void OnUpdate(CharacterThinker character)
        {
            //Vector3 currentFacing = character.Remember<Vector3>("currentFacing");
            //BodyPartMono chestPart = character.bpHolder.bodyParts[chest];
            BodyPartMono hipPart = character.bpHolder.bodyParts[hip];
            //BodyPartMono headPart = character.bpHolder.bodyParts[head];
            Vector3 targetDirection = character.targetDirection;

            if(useAgent)
            {
                if(character.agent)
                {
                    targetDirection = character.agent.nextPosition - hipPart.BodyPartTransform.position;

                    targetDirection.Normalize();
                }
                
                
            }
            else
            {
                targetDirection = character.target - hipPart.BodyPartTransform.position; //target is input
                targetDirection.Normalize();
            }
            /*targetDirection = character.target - hipPart.BodyPartTransform.position; //target is input
            targetDirection.Normalize();*/



            /*if (character.Remember<bool>("walking"))
            {
               // currentFacing = hipPart.BodyPartTransform.forward;
                currentFacing = hipPart.BodyPartFaceDirection.facingDirection;
                currentFacing.y = 0;
                currentFacing.Normalize();

                Vector3 inputDirection = character.Remember<Vector3>("inputDirection");

                if (!character.targetting)
                {

                    chestPart.BodyPartFaceDirection.facingDirection = inputDirection;
                    hipPart.BodyPartFaceDirection.facingDirection = inputDirection;
                    headPart.BodyPartFaceDirection.facingDirection = inputDirection;
                }
                else
                {
                    chestPart.BodyPartFaceDirection.facingDirection = targetDirection;
                    hipPart.BodyPartFaceDirection.facingDirection = targetDirection;
                    headPart.BodyPartFaceDirection.facingDirection = targetDirection;
                }
            }
            else
            {
                if (!character.targetting)
                {

                    chestPart.BodyPartFaceDirection.facingDirection = currentFacing;
                    hipPart.BodyPartFaceDirection.facingDirection = currentFacing;
                    headPart.BodyPartFaceDirection.facingDirection = currentFacing;
                }
                else
                {
                    chestPart.BodyPartFaceDirection.facingDirection = targetDirection;
                    hipPart.BodyPartFaceDirection.facingDirection = targetDirection;
                    headPart.BodyPartFaceDirection.facingDirection = targetDirection;
                }
            }

            character.Remember("currentFacing", currentFacing);*/
            character.targetDirection = targetDirection;
        }
    }
}