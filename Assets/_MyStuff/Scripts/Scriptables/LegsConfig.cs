using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/LegsConfig")]

    public class LegsConfig : ScriptableObject
    {

        public List<BodyPart> feet;
        public List<BodyPart> legs;
        public List<BodyPart> thighs;
        public BodyPart chest;
        public float legRate = 0.4f;
        public float legRateIncreaseBy = 0.1f;
        public float liftForce = 80;
        public float holdDownForce = 500f;
        public float moveForwardForce = 150f;
        public float inFrontVelocityM = 0.4f;
        public float chestBendDownForce = 0;


    }

}