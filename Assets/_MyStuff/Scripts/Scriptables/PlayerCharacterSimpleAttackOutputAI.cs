using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/Actitons/PlayerSimpleAttackOutput")]
    public class PlayerCharacterSimpleAttackOutputAI : CharacterAction
    {
        public override void OnFixedUpdate(CharacterThinker character)
        {
            AttackData currentAttack = character.currentAttack;

            float attackButtonClickTimer = character.attackButtonClickTimer;
            bool windUp = character.windUp;
            bool attack = character.attack;

            float attackPower = character.attackPower;
            float attackTimer = character.attackTimer;
            float windTimer = character.windTimer;
            Vector3 attackTarget = character.attackTarget;


            if (!character.health.alive || character.health.knockdown || character.isHurting)// if (character.health.knockdown) //if( !character.health.alive || character.health.knockdown || character.isHurting)
            {
                return;
            }

            if (currentAttack == null)
                return;


            Rigidbody chestBody = character.bpHolder.bodyParts[currentAttack.chestPart].BodyPartRb;
            CharacterFaceDirection hipFaceDirection = character.bpHolder.bodyParts[currentAttack.windupHipBodyPart].BodyPartFaceDirection;

            if (attack)
            {
                //EffectsController.PlayPunchSound(handsBody[0].position, handsBody[0].velocity.sqrMagnitude, handsBody[0].name);
                attackTimer += Time.deltaTime;
                if (attackTimer < currentAttack.attackSpeed * 0.2f)
                {

                    foreach (AttackPose windupPose in currentAttack.windupAttackPoses)
                    {
                        BodyPartMono bodyPart = character.bpHolder.bodyParts[windupPose.bodyPart];
                        JointDrive x = bodyPart.BodyPartConfigJoint.slerpDrive;
                        x.positionDamper = windupPose.jointDrive.x;
                        x.positionSpring = windupPose.jointDrive.y;
                        x.maximumForce = windupPose.jointDrive.z;
                        bodyPart.BodyPartConfigJoint.slerpDrive = x;

                        //Vector3 jointValue = new Vector3(20f, 0f, 0f);

                        bodyPart.BodyPartConfigJoint.targetAngularVelocity = windupPose.targetAngularVelocity;
                    }

                    chestBody.velocity *= -30f * Time.deltaTime;

                    hipFaceDirection.bodyForward.y = currentAttack.windUpHipForward;
                    //if (attackTimer < 0.2f && currentAttack.jumpOnWindUp)//check for jump in air as well
                    //{
                    //    chestBody.AddForce((currentAttack.jumpOnWindupForce) * Time.deltaTime, ForceMode.VelocityChange);
                    //}
                    character.simplewind = true;
                    character.simpleattack = false;
                }
                if (attackTimer >= currentAttack.attackSpeed * 0.4f && attackTimer < currentAttack.attackSpeed * 0.48f)
                {
                    attackTarget = (character.target - character.bpHolder.bodyParts[currentAttack.bodyPartToHitWith].BodyPartRb.transform.position).normalized;

                    foreach (AttackPose attackPose in currentAttack.attackingAttackPoses)
                    {
                        BodyPartMono bodyPart = character.bpHolder.bodyParts[attackPose.bodyPart];
                        JointDrive x = bodyPart.BodyPartConfigJoint.slerpDrive;
                        x.positionDamper = attackPose.jointDrive.x;
                        x.positionSpring = attackPose.jointDrive.y;
                        x.maximumForce = attackPose.jointDrive.z;
                        bodyPart.BodyPartConfigJoint.slerpDrive = x;

                        //Vector3 jointValue = new Vector3(20f, 0f, 0f);

                        bodyPart.BodyPartConfigJoint.targetAngularVelocity = attackPose.targetAngularVelocity;
                    }

                    if (character.target != null)
                    {

                        foreach (AttackForceRatio forceRatio in currentAttack.attackForceRatio)
                        {
                            character.bpHolder.bodyParts[forceRatio.bodyPart].BodyPartRb.AddForce(attackTarget * attackPower * forceRatio.forceRatio * Time.deltaTime, ForceMode.VelocityChange);
                        }

                        //if (attackTimer >= currentAttack.attackSpeed * (currentAttack.attackForceApplyPercent - 0.01f) && attackTimer < currentAttack.attackSpeed * currentAttack.attackForceApplyPercent)
                        //{
                        //    //print("Left Hand : feetBody[0].velocity.sqrMagnitude : " + feetBody[0].velocity.sqrMagnitude);
                        //    Debug.Log("attackPower : " + attackPower);
                        //    Debug.Log("attackButtonClickTimer : " + attackButtonClickTimer);
                        //    // EffectsController.PlayPunchSound(feetBody[0].position, feetBody[0].velocity.sqrMagnitude, feetBody[0].name);
                        //    //EffectsController.Shake(0.03f, 0.1f);
                        //}

                        // Debug.Log("attackPower : " + attackPower);

                    }

                    chestBody.velocity *= -100f * Time.deltaTime;
                    character.simplewind = false;
                    character.simpleattack = true;

                }
                if (attackTimer >= currentAttack.attackSpeed * 0.48f && attackTimer < currentAttack.attackSpeed * 1f)
                {

                }
                if (attackTimer >= currentAttack.attackSpeed * 1f)
                {

                    foreach (AttackPose afterAttackPose in currentAttack.afterAttackPoses)
                    {
                        BodyPartMono bodyPart = character.bpHolder.bodyParts[afterAttackPose.bodyPart];
                        JointDrive x = bodyPart.BodyPartConfigJoint.slerpDrive;
                        x.positionDamper = afterAttackPose.jointDrive.x;
                        x.positionSpring = afterAttackPose.jointDrive.y;
                        x.maximumForce = afterAttackPose.jointDrive.z;
                        bodyPart.BodyPartConfigJoint.slerpDrive = x;

                        //Vector3 jointValue = new Vector3(20f, 0f, 0f);

                        bodyPart.BodyPartConfigJoint.targetAngularVelocity = afterAttackPose.targetAngularVelocity;
                    }

                    attack = false;
                    attackTimer = 0f;

                    character.simplewind = false;
                    character.simpleattack = false;

                }
            }

            character.attackTarget = attackTarget;
            character.windTimer = windTimer;
            character.attackTimer = attackTimer;
            character.windUp = windUp;
            character.attack = attack;
        }

        public override void OnUpdate(CharacterThinker character)
        {
           
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}