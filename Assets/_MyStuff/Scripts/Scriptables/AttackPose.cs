using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/AttackPose")]
    public class AttackPose : ScriptableObject
    {

        public BodyPart bodyPart;
        public Vector3 jointDrive;
        public Vector3 targetAngularVelocity;
    }
}