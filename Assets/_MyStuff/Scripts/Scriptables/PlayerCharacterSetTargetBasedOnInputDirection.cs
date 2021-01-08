using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/Actitons/PlayerSetTargetInput_UsingInputDirection")]
    public class PlayerCharacterSetTargetBasedOnInputDirection : CharacterAction
    {
        public BodyPart chest;
        public BodyPart hip;
        public BodyPart head;
        public Vector3 targetOffset;

        public override void OnInitialize(CharacterThinker character)
        {
            character.targetting = false;
        }
        public override void OnFixedUpdate(CharacterThinker character)
        {
           
        }

        public override void OnUpdate(CharacterThinker character)
        {
            if(!(character.targetting == false))
            {
                character.targetting = false;
            }
            //Remember to set targetting as false;

            BodyPartMono hipPart = character.bpHolder.bodyParts[hip];
            
            BodyPartMono chestPart = character.bpHolder.bodyParts[chest];
            character.target = hipPart.BodyPartTransform.position + hipPart.BodyPartFaceDirection.facingDirection * 5;
        }
    }
}