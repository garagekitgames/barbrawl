using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EZCameraShake;
using DamageEffect;
using System.Linq;
using SO;
using UnityEngine.Events;

namespace garagekitgames
{
    public class CharacterThinker : MonoBehaviour
    {
        //public IntVariable currentLevel;
        public CharacterRuntimeSet RuntimeSet;
        public CharacterBodyPartHolder bpHolder = new CharacterBodyPartHolder();

        public List<CharacterAction> actions = new List<CharacterAction>();

        private Dictionary<string, object> memory;

        public CharacterInput input;

        public CharacterLegsSimple1 legs; //this is too complex to refactor

        public LegsConfig legConfig;

        public float speed = 100;
        public List<AttackData> attacks = new List<AttackData>();

        public Vector3 target; //Later replace this with the set target action

        //----------------------------------Attack hit miss stuff

        
        public bool youHitMe;

        public int successHitCounter;
        public int missedHitCounter;
        public int attackCounter;

        public bool youMissed;

        public bool attacking =  false;


        public bool bleedNow = false;
        public bool breakFace = false;

        public bool hurt = false;
        public bool canBeHurt = true;
        public float nextHurtTime;
        public float timeBetweenGettingHurt = 0.2f;
        //----------------------------------------------------------

        public bool targetting = true;

        public bool stopDoingShit = false;

        public bool stopAttacking = false;
        public bool stopLooking = false;

        public bool enableDrag = true;

        public int teamID = 1;

        public bool amIMainPlayer =  false;

        public NavMeshAgent agent;
        //-----------------------------------------Action Attributes-----------------------------
        public Vector3 currentFacing;
        public Vector3 targetDirection;
        public bool walking;
        public Vector3 inputDirection;

        //attack data 
        public AttackData currentAttack;
        public bool windUp = false;
        public bool attack = false;

        public bool simplewind = false;
        public bool simpleattack = false;
        //public AttackData currentAttack;
        public float attackButtonClickTimer;
        public int attackPow; //
        public float attackPower;
        public float attackTimer;
        public float windTimer;
        public Vector3 attackTarget;

        private DamageEffectScript damageEffectScript;
        public CharacterHealth health;

        public bool canDamage = true;

        public List<DamageData> damages = new List<DamageData>();
        //public AttackData currentAttack;
        //public GameObject targetGO;


        //death cry
        public BodyPart cryBodyPart;
        public BodyPartMono cryPart;

        public float moveMagnitude;

        public bool isGrabbed;
        public bool isHurting;
        public bool grounded;

        public UnityEvent OnHitSomething;
        public int coinsToDrop = 4;
        public void DeathCry()
        {
            EffectsController.Instance.PlayDeathSound(cryPart.BodyPartTransform.position, 200, cryPart.BodyPartTransform.tag);
        }
        public void SetStopDoingShit(bool value)
        {
            if(value)
            {
                windUp = false;
                attack = false;
                windUp = false;
                simplewind = false;
                simpleattack = false;
                attackButtonClickTimer = 0;
                attackPower = 0;
                attackTimer = 0;
                windTimer = 0;
                legs.walking = false;
            }
            else
            {
                windUp = false;
                attack = false;
                simplewind = false;
                simpleattack = false;
                attackButtonClickTimer = 0;
                attackPower = 0;
                attackTimer = 0;
                windTimer = 0;
                legs.walking = false;
            }

            if(health.alive)
            {
                bpHolder.ResetJoints(false);
            }
            else
            {
                bpHolder.ResetJoints(true);
            }
            //bpHolder.ResetJoints(true);
            this.stopDoingShit = value;
            
            //walking = false;
            // windUp = false;
            // attack = false;
        }

        
        /*public T Remember<T>(string key)
        {
            object result;
            if (!memory.TryGetValue(key, out result))
                return default(T);
            return (T)result;
        }
        public void Remember<T>(string key, T value)
        {
            memory[key] = value;
        }*/

        public void SetTargettting(bool value)
        {
            this.targetting = value;
        }
        public void SetEnableDrag(bool value)
        {
            this.enableDrag = value;
        }
        

        public void Awake()
        {
            bpHolder = this.GetComponent<CharacterBodyPartHolder>();
            input = this.GetComponent<CharacterInput>();
            
            memory = new Dictionary<string, object>();

            legs = this.GetComponent<CharacterLegsSimple1>();

            damageEffectScript = transform.root.GetComponent<DamageEffectScript>();
            health = transform.root.GetComponent<CharacterHealth>();


        }

        void OnDisable()
        {
            bpHolder.ResetJoints(false);
            if(!amIMainPlayer)
            {
                RuntimeSet.Remove(this);
            }
            
            // Debug.Log("PrintOnDisable: script was disabled");
        }

        void OnEnable()
        {
            windUp = false;
            attack = false;
            simplewind = false;
            simpleattack = false;
            attackButtonClickTimer = 0;
            attackPower = 0;
            attackTimer = 0;
            windTimer = 0;
            legs.walking = false;
            bpHolder.ResetJoints(false);
            RuntimeSet.Add(this);
            //Debug.Log("PrintOnEnable: script was enabled");
        }

        
        // Use this for initialization
        void Start()
        {
            agent = transform.GetComponent<NavMeshAgent>();
           // Debug.Log(bpHolder.bodyParts.Keys.Count);
            foreach (CharacterAction action in actions)
            {
                action.OnInitialize(this);
            }
            cryPart = this.bpHolder.bodyPartsName["hip"];
              
            //legs.feet[0] = this.bpHolder.bodyParts[legConfig.feet[0]].BodyPartRb;
            //legs.feet[1] = this.bpHolder.bodyParts[legConfig.feet[1]].BodyPartRb;

            //legs.legs[0] = this.bpHolder.bodyParts[legConfig.feet[0]].BodyPartRb;
            //legs.legs[1] = this.bpHolder.bodyParts[legConfig.feet[1]].BodyPartRb;

            //legs.shins[0] = this.bpHolder.bodyParts[legConfig.legs[0]].BodyPartRb;
            //legs.shins[1] = this.bpHolder.bodyParts[legConfig.legs[1]].BodyPartRb;

            //legs.thighs[0] = this.bpHolder.bodyParts[legConfig.thighs[0]].BodyPartRb;
            //legs.thighs[1] = this.bpHolder.bodyParts[legConfig.thighs[1]].BodyPartRb;

            //legs.legHeights[0] = this.bpHolder.bodyParts[legConfig.legs[0]].BodyPartMaintainHeight;
            //legs.legHeights[1] = this.bpHolder.bodyParts[legConfig.legs[1]].BodyPartMaintainHeight;

            //legs.chestBody = this.bpHolder.bodyParts[legConfig.chest].BodyPartRb;

            //legs.legRate = legConfig.legRate;
            //legs.legRateIncreaseByVelocity = legConfig.legRateIncreaseBy;
            //legs.liftForce = legConfig.liftForce;
            //legs.holdDownForce = legConfig.holdDownForce;
            //legs.moveForwardForce = legConfig.moveForwardForce;
            //legs.inFrontVelocityM = legConfig.inFrontVelocityM;
            //legs.chestBendDownForce = legConfig.chestBendDownForce;

        }

        // Update is called once per frame
        void Update()
        {
            GroundCheck();
            if (stopDoingShit)
                return;

           /* foreach(CharacterAction action in actions)
            {
                action.OnUpdate(this);
            }*/

            foreach (var action in actions)
            {
                if(action is PlayerCharacterAttackInput)
                {
                    if(!stopAttacking)
                    {
                        action.OnUpdate(this);
                    }
                }
                else if(action is PlayerCharacterAttackOutput)
                {
                    if (!stopAttacking)
                    {
                        action.OnUpdate(this);
                    }
                }
                else if(action is PlayerCharacterSetTargetInput)
                {
                    if (!stopLooking)
                    {
                        action.OnUpdate(this);
                    }
                }
                else
                {

                    action.OnUpdate(this);
                }


            }

            if (hurt)
            {
                if (damages.Count >= 1)
                {
                    DamageData damage = damages.OrderByDescending(i => i.impulseMagnitude).FirstOrDefault<DamageData>();
                    DamageCheck(damage);
                    damages.Clear();
                }

            }

            if (hurt && canBeHurt)
            {
                canBeHurt = false;
                hurt = false;
                nextHurtTime = Time.time + timeBetweenGettingHurt;
            }

            if(Time.time >= nextHurtTime)
            {
                canBeHurt = true;
            }

            if (health.currentHP.Value <= 0.0f)
            {
                if (health.alive == true)
                {
                    DeathCry();
                    //alive = false;
                }




                //firstDeath = true;

            }


            // targetGO.transform.position = target;
            //target = this.Remember<Vector3>("inputDirection");
            /*  attack = this.Remember<bool>("attack");
              windUp = this.Remember<bool>("windUp");
              attackButtonClickTimer = this.Remember<float>("attackButtonClickTimer");
              attackPower = this.Remember<float>("attackPower");
               currentAttack = this.Remember<AttackData>("currentAttack");

             if(attack)
             {
                 //if (youHitMe)
                 //{

                 //    successHitCounter++;
                 //    youHitMe = false;

                 //}
                 //else
                 //{

                 //    youMissed = true;

                 //}

                 attacking = true;
             }

             if(!attack || windUp)
             {
                 if (attacking)
                 {
                     if(youHitMe)
                     {
                         successHitCounter++;
                         youHitMe = false;
                     }
                     else
                     {
                         missedHitCounter++;

                     }
                     attackCounter++;
                     attacking = false;
                 }

             }*/
            /*if(youMissed)
            {
                missedHitCounter++;
                youMissed = false;
            }*/
            canDamage = false;
        }

        public void checkDamage(DamageData damage)
        {
            damages.Add(damage);
        }

        public void DamageCheck(DamageData damage)
        {
           // if (!canDamage)
               // return;
            //for (int i = 0; i < collisionContacts.Length; i++)
            // {
            ContactPoint contactPoint = damage.collisionContacts[0];
            float value;
            float num = 0f;
            // if (impulseMagnitude / 10 > 1)
            // {


            //  }
            //num = impulseMagnitude * collisionRigidbody.mass / rb.mass;
            num = damage.impulseMagnitude * 2f;

            //rb.AddForce((contactPoint.normal + Vector3.up) * num * 2, ForceMode.Impulse);
            //rb.AddExplosionForce(num * 400 , contactPoint.normal, 10000);


            if (num > 15)
            {

                //contactPoint.thisCollider.attachedRigidbody.AddForce((contactPoint.normal + Vector3.up) * num, ForceMode.VelocityChange);
                //contactPoint.thisCollider.attachedRigidbody.AddForce((contactPoint.normal) * num, ForceMode.VelocityChange);



                //contactPoint.thisCollider.attachedRigidbody.AddForce((damage.relativeVelocity), ForceMode.VelocityChange);


                if (((health.currentHP.Value - Mathf.RoundToInt(1)) <= 0))
                {
                    if(amIMainPlayer)
                    {
                        contactPoint.thisCollider.attachedRigidbody.AddForce((damage.relativeVelocity.normalized) * num , ForceMode.VelocityChange);
                       // EffectsController.Instance.CreateFloatingTextEffect(damage.collisionContacts[0].point, num, this.transform, (int)num, amIMainPlayer);
                        bpHolder.ResetJoints(true);
                    }
                    else
                    {
                        contactPoint.thisCollider.attachedRigidbody.AddForce((damage.relativeVelocity.normalized) * num , ForceMode.VelocityChange);
                        bpHolder.ResetJoints(true);
                        EffectsController.Instance.CreateFloatingTextEffect(damage.collisionContacts[0].point, num, this.transform, (int)damage.damagePow, amIMainPlayer);
                    }
                    // EffectsController.Instance.CreateFloatingTextEffectForScore(damage.collisionContacts[0].point, num, this.transform, (int)num, amIMainPlayer);
                }
                else
                {
                    if(amIMainPlayer)
                    {
                        contactPoint.thisCollider.attachedRigidbody.AddForce((damage.relativeVelocity.normalized) * num , ForceMode.VelocityChange);
                        //EffectsController.Instance.CreateFloatingTextEffect(damage.collisionContacts[0].point, num, this.transform, (int)num, amIMainPlayer);
                    }
                    else
                    {
                        contactPoint.thisCollider.attachedRigidbody.AddForce((damage.relativeVelocity.normalized) * num, ForceMode.VelocityChange);
                        EffectsController.Instance.CreateFloatingTextEffect(damage.collisionContacts[0].point, num, this.transform, (int)damage.damagePow, amIMainPlayer);
                    }



                }

                
                /*********************************************************Use This *************************************/

                EffectsController.Instance.PlayImpactSound(damage.collisionContacts[0].point, num, damage.collisionCollider.name);


                EffectsController.Instance.CreateHitEffect(damage.collisionContacts[0].point, num, -damage.collisionContacts[0].normal);
                EffectsController.Instance.CreateCash(damage.collisionContacts[0].point, damage.cashToDrop);



                EffectsController.Instance.PlayGruntSound(damage.collisionContacts[0].point, num, damage.collisionCollider.tag);


               // health.applyDamage(Mathf.RoundToInt(num));
                health.applyDamage(Mathf.RoundToInt(damage.damagePow));




                if (num > 65)
                {
                    
                    this.bleedNow = true;
                    EffectsController.Instance.PlayBloodSpurtSound(damage.collisionContacts[0].point, num, damage.collisionCollider.tag);
                    EffectsController.Instance.PlayBreakSound(damage.collisionContacts[0].point, num, damage.collisionCollider.tag);
                    
                    damageEffectScript.Blink(0, 0.2f);

                    CameraShaker.Instance.ShakeOnce(num / 5, 10, 0.05f, 0.15f);
                    if (CameraShaker.Instance != null)
                    {
                        //CameraShaker.Instance.ShakeOnce(num / 5, 40, 0.05f, 0.1f);
                        CameraShaker.Instance.ShakeOnce(num / 5, 10, 0.05f, 0.15f);
                    }

                    OnHitSomething.Invoke();

                }
                else if ((num <= 65) && (num > 10))
                {
                    
                    EffectsController.Instance.PlayBloodSpurtSound(damage.collisionContacts[0].point, num, damage.collisionCollider.tag);
                    EffectsController.Instance.PlayBreakSound(damage.collisionContacts[0].point, num, damage.collisionCollider.tag);
                   
                    damageEffectScript.Blink(0, 0.4f);
                    CameraShaker.Instance.ShakeOnce(num, 5, 0.05f, 0.8f);
                    if (CameraShaker.Instance != null)
                    {
                        //CameraShaker.Instance.ShakeOnce(num / 5, 10, 0.05f, 0.4f);
                        CameraShaker.Instance.ShakeOnce(num, 5, 0.05f, 0.8f);

                    }
                    

                    OnHitSomething.Invoke();


                }
            }

            // }

        }

        public void GroundCheck()
        {
            /*foreach (var item in bpHolder.bodyParts.Values)
            {
                if()
            }*/

            var onGroundParts = bpHolder.bodyParts.Values.Where(t => t.partOnGround == true).ToList<BodyPartMono>();

            if (onGroundParts.Count > 0)
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }


        private void FixedUpdate()
        {
            if (stopDoingShit)
                return;


            foreach (CharacterAction action in actions)
            {
                action.OnFixedUpdate(this);
            }
        }
    }

}