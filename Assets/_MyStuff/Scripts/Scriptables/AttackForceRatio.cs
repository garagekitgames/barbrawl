using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/AttackForceRatio")]
    public class AttackForceRatio : ScriptableObject
    {

        public BodyPart bodyPart;
        [Range(-5, 5)]
        public float forceRatio;
        
    }
}