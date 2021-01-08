using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/Actitons/PlayerLegsInput")]
    public class PlayerCharacterLegsInput : CharacterAction

    {


        public override void OnInitialize(CharacterThinker character)
        {
            base.OnInitialize(character);
            character.legs.feet[0] = character.bpHolder.bodyParts[character.legConfig.feet[0]].BodyPartRb;
            character.legs.feet[1] = character.bpHolder.bodyParts[character.legConfig.feet[1]].BodyPartRb;

            character.legs.legs[0] = character.bpHolder.bodyParts[character.legConfig.feet[0]].BodyPartRb;
            character.legs.legs[1] = character.bpHolder.bodyParts[character.legConfig.feet[1]].BodyPartRb;

            character.legs.shins[0] = character.bpHolder.bodyParts[character.legConfig.legs[0]].BodyPartRb;
            character.legs.shins[1] = character.bpHolder.bodyParts[character.legConfig.legs[1]].BodyPartRb;

            character.legs.thighs[0] = character.bpHolder.bodyParts[character.legConfig.thighs[0]].BodyPartRb;
            character.legs.thighs[1] = character.bpHolder.bodyParts[character.legConfig.thighs[1]].BodyPartRb;

            character.legs.legHeights[0] = character.bpHolder.bodyParts[character.legConfig.legs[0]].BodyPartMaintainHeight;
            character.legs.legHeights[1] = character.bpHolder.bodyParts[character.legConfig.legs[1]].BodyPartMaintainHeight;

            character.legs.chestBody = character.bpHolder.bodyParts[character.legConfig.chest].BodyPartRb;

            character.legs.legRate = character.legConfig.legRate;
            character.legs.legRateIncreaseByVelocity = character.legConfig.legRateIncreaseBy;
            character.legs.liftForce = character.legConfig.liftForce;
            character.legs.holdDownForce = character.legConfig.holdDownForce;
            character.legs.moveForwardForce = character.legConfig.moveForwardForce;
            character.legs.inFrontVelocityM = character.legConfig.inFrontVelocityM;
            character.legs.chestBendDownForce = character.legConfig.chestBendDownForce;

            character.legs.StopWalking();
            foreach (Rigidbody r in character.legs.legs) r.maxAngularVelocity = 40;
            foreach (Rigidbody r in character.legs.feet) r.maxAngularVelocity = 40;
            foreach (Rigidbody r in character.legs.thighs) r.maxAngularVelocity = 40;
            foreach (Rigidbody r in character.legs.shins) r.maxAngularVelocity = 40;

        }
        public override void OnFixedUpdate(CharacterThinker character)
        {
            //throw new System.NotImplementedException();
        }

        public override void OnUpdate(CharacterThinker character)
        {
            character.legs.inputDirection = character.inputDirection;
            if (character.walking)
            {
                if (character.legs)
                {
                    if (!character.legs.walking)
                    {
                        character.legs.StartWalking();
                    }
                }
            }
            else
            {
                if (character.legs)
                {
                    if (character.legs.walking)
                    {
                        character.legs.StopWalking();
                    }
                }

            }
        }

        
    }

}
