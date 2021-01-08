using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/AttackConfig")]
    public class AttackData : ScriptableObject
    {
        public AttackType attackType;
        public List<AttackPose> windupAttackPoses;
        public List<AttackPose> attackingAttackPoses;
        public List<AttackPose> afterAttackPoses;

        public List<AttackForceRatio> attackForceRatio;

        //public string attackName;
        [Range(0, 1)]
        public float attackForceApplyPercent = 0.5f;// percentage of the punch duration the force is applied, shorter duration would mean less tracking
        [Range(0, 10)]
        public float attackPowerIncreaseRate = 2;//2 to 5 works good
        [Range(0f, 0.9f)]
        public float accuracyReduceFactor = 0f; //less value is better, percentage of 

        public float attackSpeed = 0.2f;
        //public float attackPower = 1300f;
        public float startAttackPower = 1300f;
        public float maxAttackPower = 3500f;
        public float minAttackPower = 1300f;

        public BodyPart chestPart;
        public BodyPart bodyPartToHitWith;

        public BodyPart windupHipBodyPart;
        public float windUpHipForward;
        public bool jumpOnWindUp;
        public Vector3 jumpOnWindupForce;
        public Vector3 attackHipForce;

    }
}