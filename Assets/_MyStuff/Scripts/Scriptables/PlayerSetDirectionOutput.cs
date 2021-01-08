using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/Actitons/PlayerSetDirection")]
    public class PlayerSetDirectionOutput : CharacterAction
    {
        public BodyPart chest;
        public BodyPart hip;
        public BodyPart head;
        //public bool targetting;

        public bool useHeadToFaceTarget;
        public bool useChestToFaceTarget;
        public bool useHipToFaceTarget;

        public override void OnFixedUpdate(CharacterThinker character)
        {
            
        }

        public override void OnUpdate(CharacterThinker character)
        {
            Vector3 currentFacing = character.currentFacing;
            BodyPartMono chestPart = character.bpHolder.bodyParts[chest];
            BodyPartMono hipPart = character.bpHolder.bodyParts[hip];
            BodyPartMono headPart = character.bpHolder.bodyParts[head];
            Vector3 targetDirection = character.targetDirection;
            /*targetDirection = character.target - hipPart.BodyPartTransform.position; //target is input
            targetDirection.Normalize();*/



            if (character.walking)
            {
               // currentFacing = hipPart.BodyPartTransform.forward;
                currentFacing = hipPart.BodyPartFaceDirection.facingDirection;
                currentFacing.y = 0;
                currentFacing.Normalize();

                Vector3 inputDirection = character.inputDirection;

                if (!character.targetting)
                {
                    if (useChestToFaceTarget)
                        chestPart.BodyPartFaceDirection.facingDirection = inputDirection;
                    if (useHipToFaceTarget)
                        hipPart.BodyPartFaceDirection.facingDirection = inputDirection;
                    if (useHeadToFaceTarget)
                        headPart.BodyPartFaceDirection.facingDirection = inputDirection;
                }
                else
                {
                    if(useHeadToFaceTarget)
                        headPart.BodyPartFaceDirection.facingDirection = targetDirection;

                    if (useChestToFaceTarget)
                        chestPart.BodyPartFaceDirection.facingDirection = targetDirection;

                    if (useHipToFaceTarget)
                        hipPart.BodyPartFaceDirection.facingDirection = targetDirection;
                    
                }
            }
            else
            {
                if (!character.targetting)
                {

                    chestPart.BodyPartFaceDirection.facingDirection = currentFacing;
                    if (useHipToFaceTarget)
                        hipPart.BodyPartFaceDirection.facingDirection = currentFacing;
                    if (useHeadToFaceTarget)
                        headPart.BodyPartFaceDirection.facingDirection = currentFacing;
                }
                else
                {
                    if (useChestToFaceTarget)
                        chestPart.BodyPartFaceDirection.facingDirection = targetDirection;
                    if (useHipToFaceTarget)
                        hipPart.BodyPartFaceDirection.facingDirection = targetDirection;
                    if (useHeadToFaceTarget)
                        headPart.BodyPartFaceDirection.facingDirection = targetDirection;
                }
            }

            character.currentFacing = currentFacing;
            //character.Remember("targetDirection", targetDirection);
        }
    }
}