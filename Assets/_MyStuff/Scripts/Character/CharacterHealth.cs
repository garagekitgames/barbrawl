using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace garagekitgames
{

    public class CharacterHealth : CharacterHealthManager
    {
        public CharacterThinker character;
        public bool alive = true;
        public UnityEvent OnEnableEvent;
        public UnityEvent OnDisableEvent;

        public UnityEvent OnHurtEvent;
        public UnityEvent OnRecoverEvent;


        public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
        //public Image m_FillImage;                           // The image component of the slider.
        //public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
        //public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.

        public bool isKilledByPlayer = false;
        public bool isHurting = false;


        [Header("Health, Knockdown etc...")]
        //HEalth and damage stuff
        public float invincibleTimer = -1;
        public bool canBeKnockedDown = true;
        public bool knockdown;
        public float maxHealth = 1000f;  // maximum health at starts
        public float currentHealth; // health at any given time 
        float healthDamage = 0f; // sum of damage to the life ; value to be subtracted from life Add damage to this, and this value will be subracted from currenthealth every udate cycle
        float regenerateHealth = 1f; // value by which life will be regenerated, every update cycle, set regenerate = regenerate/knockdown count
        public float reduceBasedOnLifeFactor = 0f; // currentHealth/maxHeaalth , this is done every update, this will be used to multiply with the forces, affects wakeup time, affects maxKnockcout

        public float maxWakeupEffort = 10; // max effort needed to wakeup
        public float currentWakeupeffort = 0; // effort at any given time ; if currentWakeupeffort > maxWakeupEffort then wake up the player; 
        float wakeupdepletionRate = 1f;// rate at which wakeup meter depletes; every update if(currentWakeupeffort >= 0)currentWakeupeffort -= Time.deltaTime * wakeupdepletionRate;
        float wakeupMultiplier = 1; // starts at one, when player is knocked down, every key press adds this amount to the currentWakeupeffort  = currentWakeupeffort + wakeupMultiplier * reduceBasedOnLifeFactor;  

        public float maxKnockoutForce = 50f;//amount based on how tough character is
        public float currentKnockoutForce = 0; // damage taken at any given time;  if currentKnockoutForce >= maxKnockoutForce 
        float knockoutDepletionRate = 10; //  every update if(currentWakeupeffort >= 0)currentWakeupeffort -= Time.deltaTime * knockoutDepletionRate;
        float maxKnockoutForceDecreaseRate = 0.1f; // this is used to reduce the max knocout force required everytime the player is knocked down,
        public int knockdownCount = 0;


        public float hurtDuration = 0.2f;
        public void SetHealthUI()
        {
            // Set the slider's value appropriately.
            if(m_Slider)
                m_Slider.value = currentHP.Value / maxHP.Value * 100;

            // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
            //if(m_FillImage)
              //  m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, currentHP.Value / maxHP.Value);
        }

        bool firstDeath =  false;

        private void OnEnable()
        {
            if (ResetHP)
                currentHP.Value = maxHP.Value;

            alive = true;

            if(firstDeath)
                OnEnableEvent.Invoke();

            character = transform.GetComponent<CharacterThinker>();


            if (GameObject.FindGameObjectWithTag("HealthSlider") && character.amIMainPlayer)
            {
                m_Slider = GameObject.FindGameObjectWithTag("HealthSlider").GetComponent<Slider>();
            }
            else
            {

            }
            

            SetHealthUI();
        }
        // Use this for initialization
        void Start()
        {
            
        }

        private void Update()
        {
            UpdateHealth();
            UpdateWakeup();
            if (knockdown)
            {
                if (!character.isHurting)//&& !character.isGrabbed)
                {
                    StartCoroutine(DoKnockDown(0.1f));
                }

                //myCharacterMovement.applyDrag = false;
                //myCharacterLegs.enabled = false;
                // myCharacterMovement.maintainHeight.enabled = false;
                // myCharacterMovement.canJump = false;
                // rightgrab = false;
                // rightGrabCheck.grabNow = false;
                //  leftgrab = false;
                // leftGrabCheck.grabNow = false;

                //main
                //if (character.player.GetButtonDown("PrimaryAttackLeft"))
                //{
                //    // print("Just Pressed Key");

                //    //buttonClickTimer = 0f;
                //    //grab = false;

                //    // inputDirection.y = 0.0f;

                //    currentWakeupeffort = currentWakeupeffort + wakeupMultiplier * reduceBasedOnLifeFactor;
                //    if (currentWakeupeffort > maxWakeupEffort)
                //    {

                //        knockdown = false;
                //        currentWakeupeffort = 0;
                //    }
                //    //wakeup();

                //}
                //end main
                return;

            }

            if(!character.amIMainPlayer)
            {
                SetHealthUI();
            }
            
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (currentHP.Value <= 0.0f)
            {
                if(alive == true)
                {
                    alive = false;
                    DeathEvent.Invoke();
                    
                }
                

                

                firstDeath = true;

            }
        }

        IEnumerator DoHurt(float sec)
        {
            OnHurtEvent.Invoke();
            character.isHurting = true;
            Debug.Log("isHurting " + character.isHurting);
            yield return new WaitForSeconds(sec);
            while (!character.grounded)
            {
                yield return new WaitForFixedUpdate();
            }
            //yield return new WaitForSeconds(sec);

            if (alive && !knockdown)
            {
                OnRecoverEvent.Invoke();
            }
            else
            {
                character.isHurting = false;
            }
            //isHurting = false;



        }

        IEnumerator DoKnockDown(float sec)
        {
            OnHurtEvent.Invoke();
            character.isHurting = true;

            yield return new WaitForSeconds(sec);
            while (knockdown)
            {
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(sec);

            if (alive)
            {
                OnRecoverEvent.Invoke();
            }
            //isHurting = false;



        }



        public void applyDamage(float value)
        {
            if ((currentHP.Value - value) > 0)
            {
                if (!character.isHurting)//&& !character.isGrabbed)
                {
                    StartCoroutine(DoHurt(hurtDuration));
                }
                /*else if(!character.isHurting && character.isGrabbed)
                {
                    character.isHurting = true;
                }*/
                

            }

            currentHP.Value = currentHP.Value - value;
            DamageEvent.Invoke();

        }

        public void AddDamage(float damage, Rigidbody causer)
        {
            //if (invincibleTimer < 0f)
            // {
            healthDamage += damage;
            currentKnockoutForce += damage;
            if (causer != null)
            {
            }
            //}
        }

        public void UpdateWakeup()
        {
            float num = currentWakeupeffort;
            if (currentWakeupeffort <= maxWakeupEffort && currentWakeupeffort >= 0)
            {
                currentWakeupeffort -= Time.deltaTime * wakeupdepletionRate;


            }

            /*if (currentWakeupeffort >= maxWakeupEffort)
            {
                currentWakeupeffort = maxWakeupEffort;


            }*/
            // healthDamage = 0f;

        }

        public void UpdateHealth()
        {
            float num = currentHealth;
            if (currentHealth < maxHealth)
            {
                if (knockdown)
                {
                    currentHealth += Time.deltaTime * regenerateHealth / 10;
                }
                else
                {
                    currentHealth += Time.deltaTime * regenerateHealth;
                }
            }
            if (knockdown)
            {
                if (currentHealth > 0f)
                {
                    currentHealth -= healthDamage / 4f;

                }
            }
            else if (currentHealth > 0f)
            {
                currentHealth -= healthDamage;
            }
            //float num2 = currentHealth - num;


            //currentHealth -= num;

            if (currentKnockoutForce >= 0)
            {
                currentKnockoutForce -= Time.deltaTime * knockoutDepletionRate;

            }

            if (currentKnockoutForce > maxKnockoutForce && !knockdown && canBeKnockedDown)
            {
                knockdown = true;
                knockdownCount++;
                currentKnockoutForce = maxKnockoutForce;
                //maxKnockoutForce = maxKnockoutForce -1;
            }


            reduceBasedOnLifeFactor = currentHealth / maxHealth;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            currentKnockoutForce = Mathf.Clamp(currentKnockoutForce, 0f, maxKnockoutForce);
            healthDamage = 0f;

        }

    }
}