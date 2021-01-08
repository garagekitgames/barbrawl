using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using garagekitgames;
using System.Linq;
using UnityEngine.AI;

namespace garagekitgames
{
    public class CharacterBasicAI : MonoBehaviour
    {
        public bool stopDoingShit;

        public TransformVariable target;//used

        public FloatVariable difficultyMultiplier;

        public Transform targetTransform;//used

        public CharacterThinker character;//used

        public Vector3 targetDirection;//used

        public float targetDistance;//used

        public BodyPart rootBodyPart;//used

        public float attackDistance = 1.2f; //used

        public float attackSpeed = 0.5f; //used but same as chargeTimePerDistance

        public float attackPower; //used

        public bool windUp; //used

        public bool attack; //used

        public float nextShotAllowedAfter = 0; //npot used

        public float fireAt; //not used

        public bool justPressed, onHold, released = false; //not used

        public float attackCounter = 0; //using
        public float attackDelay = 0.4f; //using

        public NavMeshAgent agent;

        public bool stopAttack = false;


        [MinMaxRange(3f, 9f)]
        public RangedFloat moveNavigationSpeedVariable; //used

        [MinMaxRange(100f, 800f)]
        public RangedFloat moveForceSpeedVariable; //used

        [MinMaxRange(0.2f, 3f)]
        public RangedFloat chargeTimePerDistance; //used

        [MinMaxRange(0.2f, 2)]
        public RangedFloat timeBetweenShots; //not used
        // Use this for initialization
        

        public void SetStopDoingShit(bool value)
        {
            
            this.stopDoingShit = value;
            
        }
        void Start()
        {
            agent = transform.GetComponent<NavMeshAgent>();
            character = this.GetComponent<CharacterThinker>();
            //attackDelay = Mathf.Clamp(timeBetweenShots.minValue / difficultyMultiplier.value, timeBetweenShots.minValue, timeBetweenShots.maxValue);
           // attackSpeed = Mathf.Clamp(chargeTimePerDistance.minValue / difficultyMultiplier.value, chargeTimePerDistance.minValue, chargeTimePerDistance.maxValue);
            targetTransform = target.value;
            character.target = targetTransform.position;
            if(agent.isActiveAndEnabled)
            {
             //   agent.speed = Mathf.Clamp(moveNavigationSpeedVariable.maxValue * difficultyMultiplier.value, moveNavigationSpeedVariable.minValue, moveNavigationSpeedVariable.maxValue);
            }
           // character.speed = Mathf.Clamp(moveForceSpeedVariable.maxValue * difficultyMultiplier.value, moveForceSpeedVariable.minValue, moveForceSpeedVariable.maxValue);
        }

        private void PickAnAttack(CharacterThinker character)
        {
            AttackData currentAttack = character.attacks[UnityEngine.Random.Range(0, character.attacks.Count)];
            character.currentAttack = currentAttack;
        }

        public void SetStopAttackFor(float sec)
        {
            StartCoroutine(StopAttackFor(sec));
        }

        IEnumerator StopAttackFor(float sec)
        {
            this.stopAttack = true;
            yield return new WaitForSeconds(sec);
            this.stopAttack = false;
        }

        private void OnEnable()
        {
            character = this.GetComponent<CharacterThinker>();
            
            //attackDelay = Mathf.Clamp(timeBetweenShots.minValue / difficultyMultiplier.value, timeBetweenShots.minValue, timeBetweenShots.maxValue);
            
           // attackSpeed = Mathf.Clamp(chargeTimePerDistance.minValue / difficultyMultiplier.value, chargeTimePerDistance.minValue, chargeTimePerDistance.maxValue);
            agent = transform.GetComponent<NavMeshAgent>();
            if (agent.isActiveAndEnabled)
            {
             //   agent.speed = Mathf.Clamp(moveNavigationSpeedVariable.maxValue * difficultyMultiplier.value, moveNavigationSpeedVariable.minValue, moveNavigationSpeedVariable.maxValue);
            }
            //character.speed = Mathf.Clamp(moveForceSpeedVariable.maxValue * difficultyMultiplier.value, moveForceSpeedVariable.minValue, moveForceSpeedVariable.maxValue);
        }

        public void OnLevelUp()
        {
            //attackDelay = Mathf.Clamp(timeBetweenShots.minValue / difficultyMultiplier.value, timeBetweenShots.minValue, timeBetweenShots.maxValue);

            //attackSpeed = Mathf.Clamp(chargeTimePerDistance.minValue / difficultyMultiplier.value, chargeTimePerDistance.minValue, chargeTimePerDistance.maxValue);
            agent = transform.GetComponent<NavMeshAgent>();
            if (agent.isActiveAndEnabled)
            {
             //   agent.speed = Mathf.Clamp(moveNavigationSpeedVariable.maxValue * difficultyMultiplier.value, moveNavigationSpeedVariable.minValue, moveNavigationSpeedVariable.maxValue);
            }
            //character.speed = Mathf.Clamp(moveForceSpeedVariable.maxValue * difficultyMultiplier.value, moveForceSpeedVariable.minValue, moveForceSpeedVariable.maxValue);

        }
        // Update is called once per frame
        void FixedUpdate()
        {
            if (stopDoingShit)
                return;


            if (!character.health.alive || character.health.knockdown || character.isHurting)// if (character.health.knockdown) //if( !character.health.alive || character.health.knockdown || character.isHurting)
            {
                //return;
            }

            if( !character.health.alive || character.health.knockdown || character.isHurting)
            {
                character.windUp = false;
                character.attack = false;
                character.simplewind = false;
                character.simpleattack = false;
                character.attackButtonClickTimer = 0;
                character.attackPower = 0;
                character.attackTimer = 0;
                character.windTimer = 0;

                windUp = false;
                attack = false;
                attackCounter = 0;
               //attackButtonClickTimer = 0;

                //return;
            }


            targetTransform = target.value;
            character.target = targetTransform.position;


            
            if (agent.isActiveAndEnabled)
            {
                targetDirection = agent.nextPosition - character.bpHolder.bodyParts[rootBodyPart].bodyPartTransform.position;
                
                targetDirection.Normalize();
                targetDirection.y = 0;
               // print("Agent Target : " + targetDirection);
               // character.Remember("targetDirection", targetDirection);
                targetDistance = Vector3.Distance(character.target, character.bpHolder.bodyParts[rootBodyPart].bodyPartTransform.position);
            }
            else
            {
                targetDirection = character.target - character.bpHolder.bodyParts[rootBodyPart].bodyPartTransform.position;
                
                targetDirection.Normalize();
                //targetDirection.y = 0;
               // print("Non Agent Target : " + targetDirection);

                targetDistance = Vector3.Distance(character.target, character.bpHolder.bodyParts[rootBodyPart].bodyPartTransform.position);

            }

            /*windUp = character.Remember<bool>("windUp");
            attack = character.Remember<bool>("attack");*/

            if (targetDistance >= attackDistance)
            {
                character.inputDirection = targetDirection;
                character.walking = true;
                windUp = false;
                attack = false;
                attackCounter = 0;

            }
            else if(targetDistance < attackDistance)
            {
                //if (!character.health.alive || character.health.knockdown || character.isHurting || character.attacking)// if (character.health.knockdown) //if( !character.health.alive || character.health.knockdown || character.isHurting)
                //{
                //    return;
                //}

                // PickAnAttack(character);
                //character.Remember("nextShotAllowedAfter", Time.time + Random.Range(timeBetweenShots.minValue, timeBetweenShots.maxValue));
                character.walking = false;

                if (stopAttack)
                    return;
                //if (windUp == false && !attack)    // if the button is pressed for more than 0.2 seconds grab
                //{
                //    windUp = true;
                //    //character.Remember("windUp", windUp);

                //}

                //if(windUp)
                //{

                //}

                //nextShotAllowedAfter = Time.time;
                //if (!windUp)
                //{
                //    if (Time.time > nextShotAllowedAfter)
                //    {
                //        //float distanceToTarget = Vector3.Distance(target.transform.position, tank.transform.position);
                //        float timeToCharge = Random.Range(chargeTimePerDistance.minValue, chargeTimePerDistance.maxValue);
                //        //character.Remember("fireAt", Time.time + timeToCharge);
                //        fireAt = Time.time + timeToCharge;
                //        windUp = true;
                //        character.Remember("windUp", windUp);
                //    }
                //}
                //else
                //{
                //    //float fireAt = character.Remember<float>("fireAt");
                //    if (Time.time > fireAt)
                //    {
                //        attack = true;
                //        character.Remember("attack", attack);
                //        nextShotAllowedAfter = Time.time + Random.Range(timeBetweenShots.minValue, timeBetweenShots.maxValue);
                //        // character.Remember("nextShotAllowedAfter", Time.time + Random.Range(timeBetweenShots.minValue, timeBetweenShots.maxValue));
                //    }
                //}


                float attackButtonClickTimer = character.attackButtonClickTimer;
                bool windUp = character.windUp;
                bool attack = character.attack;

                attackCounter += Time.deltaTime;


                if (!character.health.alive || character.health.knockdown || character.isHurting)
                {
                    character.windUp = false;
                    character.attack = false;
                    character.simplewind = false;
                    character.simpleattack = false;
                    character.attackButtonClickTimer = 0;
                    character.attackPower = 0;
                    character.attackTimer = 0;
                    character.windTimer = 0;

                    windUp = false;
                    attack = false;
                    attackCounter = 0;
                    //attackButtonClickTimer = 0;

                    //return;
                }



                if (attackCounter > attackDelay * 0.75f)
                {
                    //print("Key Released");
                    if (!attack) // as long as key is held down increase time, this records how long the key is held down
                    {
                        PickAnAttack(character);
                        //float attackPower = Mathf.Clamp(currentAttack.startAttackPower * (1 + attackButtonClickTimer * currentAttack.attackPowerIncreaseRate), currentAttack.minAttackPower, currentAttack.maxAttackPower);
                        AttackData currentAttack = character.currentAttack;
                        currentAttack.attackSpeed = attackSpeed; //right now its random, this has to increase in speed, or decrease in value based on time, this affects difficulty 
                        //float attackButtonClickTimer = character.Remember<float>("attackButtonClickTimer");
                        float attackPower = Mathf.Clamp(currentAttack.startAttackPower * (1 + attackButtonClickTimer * currentAttack.attackPowerIncreaseRate), currentAttack.minAttackPower, currentAttack.maxAttackPower);
                        character.attackPower = attackPower;

                        attack = true;
                        attackButtonClickTimer = 0f;
                        attackCounter = 0;
                    }

                    windUp = false;



                }

                

                else if (attackCounter > attackDelay * 0.25f)
                {
                    attackButtonClickTimer += Time.deltaTime;
                    if (windUp == false && !attack)    // if the button is pressed for more than 0.2 seconds grab
                    {
                        windUp = true;
                        //character.Remember("windUp", windUp);

                    }
                    /* Debug.Log("windUp : " + windUp);
                     Debug.Log("attackButtonClickTimer : " + attackButtonClickTimer);*/

                }

                else
                {
                    attackButtonClickTimer = 0f;

                    windUp = false;

                   /* if (!attack)
                        PickAnAttack(character);*/

                }


                character.attackButtonClickTimer = attackButtonClickTimer;
                character.windUp = windUp;
                character.attack = attack;

            }

            //character.Remember("windUp", windUp);
           // character.Remember("attack", attack);

        }
    }
}