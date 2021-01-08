using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/Actitons/PlayerDirectionOutput")]
    public class PlayerDirectionOuput : CharacterAction
    {
        public BodyPart chest;
        public BodyPart hip;
        public BodyPart head;
        public bool targetting;
        public override void OnFixedUpdate(CharacterThinker character)
        {

        }

        public override void OnUpdate(CharacterThinker character)
        {
            Vector3 currentFacing = character.currentFacing;
            BodyPartMono chestPart = character.bpHolder.bodyParts[chest];
            BodyPartMono hipPart = character.bpHolder.bodyParts[hip];
            BodyPartMono headPart = character.bpHolder.bodyParts[head];
            Vector3 targetDirection = character.target - hipPart.BodyPartTransform.position; //target is input
            targetDirection.Normalize();



            if (character.walking)
            {
                currentFacing = hipPart.BodyPartTransform.forward;
                currentFacing.y = 0;
                currentFacing.Normalize();

                Vector3 inputDirection = character.inputDirection;

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

            character.currentFacing = currentFacing;
        }


    }
}
