
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XftWeapon;


namespace garagekitgames
{
    public class CharacterSimpleHitMissTracker : MonoBehaviour
    {

        public CharacterThinker character;
        public bool attack;
        public bool attacking;
        public bool windUp;
        public int successHitCounter;
        public int missedHitCounter;
        public int attackCounter;

        public UnityEvent playerHitEvent;
        public UnityEvent playerMissEvent;

        private bool playWoosh;
        bool canWoosh = false;

        private bool playWindup;
        bool canWindup = false;
        // Use this for initialization
        public GameObject windUpPrefab;
        public GameObject windUpAlertPrefab;
        public ParticleSystem windUpParticle;

        public ParticleSystem windUpAlertParticle;

        public GameObject currentWindupAttack;
        public GameObject currentWindupAlert;

        // public GameObject windUpAlertPrefab;

        public AudioClip[] windupclips;

        public XWeaponTrail xWT;

        public TrailRenderer trail;

        void Start()
        {
            character = transform.GetComponent<CharacterThinker>();
            //windUpParticle = windUpPrefab.GetComponent<ParticleSystem>();
            // var main = windUpParticle.main;
            // main.loop = false;
            windUpAlertPrefab = EffectsController.Instance.alertPrefab;


        }

        private void OnEnable()
        {
            character = transform.GetComponent<CharacterThinker>();
        }
        // Update is called once per frame
        void Update()
        {

            checkHitOrMiss();

            wooshSound();

            windupSound();

            playTrail();

        }

        public void playTrail()
        {
            attack = character.simpleattack;
            if (attack)
            {
                AttackData currentAttack = character.currentAttack;
                BodyPartMono attackPart = character.bpHolder.bodyParts[currentAttack.bodyPartToHitWith];

                trail = attackPart.GetComponent<TrailRenderer>();

                if(trail)
                {
                    trail.emitting = true;
                }
            }
            else if(!attack)
            {
                if (trail)
                {
                    trail.emitting = false;
                }
            }
            

        }

        private void wooshSound()
        {
            attack = character.simpleattack;
            AttackData currentAttack = character.currentAttack;
            //playWoosh = character.Remember<bool>("playWoosh");

            
            if (currentAttack == null)
                return;
            BodyPartMono soundPart = character.bpHolder.bodyParts[currentAttack.bodyPartToHitWith];
            BodyPartMono yellPart = character.bpHolder.bodyParts[currentAttack.chestPart];
            xWT = soundPart.GetComponentInChildren<XWeaponTrail>();
            if (attack)
            {
                playWoosh = true;

                
                //canWoosh = true;
                //if (youHitMe)
                //{

                //    successHitCounter++;
                //    youHitMe = false;

                //}
                //else
                //{

                //    youMissed = true;

                //}
                /*if(xWT)
                {
                    xWT.StopSmoothly(0.2f);
                }*/


            }
            else
            {
                //playWoosh = false;
                playWoosh = false;
                canWoosh = false;
            }

            if (playWoosh && !canWoosh)
            {
                EffectsController.Instance.PlayWooshSound(soundPart.BodyPartTransform.position, (character.attackPower / currentAttack.maxAttackPower) * 100, soundPart.BodyPartTransform.name);
                EffectsController.Instance.PlayYellSound(soundPart.BodyPartTransform.position, (character.attackPower / currentAttack.maxAttackPower) * 100, soundPart.BodyPartTransform.name);
                canWoosh = playWoosh;
                /*if (xWT)
                {
                    xWT.Activate();
                    xWT.StopSmoothly(1.5f);

                }*/


                //playWoosh = false;

            }
        }


        

        private void windupSound()
        {
            windUp = character.simplewind;
            AttackData currentAttack = character.currentAttack;
            //playWoosh = character.Remember<bool>("playWoosh");

            if (currentAttack == null)
                return;
            BodyPartMono soundPart = character.bpHolder.bodyParts[currentAttack.bodyPartToHitWith];
            BodyPartMono yellPart = character.bpHolder.bodyParts[currentAttack.chestPart];


            if (windUp)
            {
                playWindup = true;
                //canWoosh = true;
                //if (youHitMe)
                //{

                //    successHitCounter++;
                //    youHitMe = false;

                //}
                //else
                //{

                //    youMissed = true;

                //}


            }
            else
            {
                //playWoosh = false;
                playWindup = false;
                canWindup = false;
            }

            if (playWindup && !canWindup)
            {

                EffectsController.Instance.PlayWindupSound(yellPart.BodyPartTransform.position , 100, soundPart.BodyPartTransform.name);
                //EffectsController.Instance.CreateAlert(soundPart.BodyPartTransform.position + (Vector3.up * 2));

                //EffectsController.Instance.PlayEnemyWindupAlertSound(soundPart.BodyPartTransform.position, 100, soundPart.BodyPartTransform.name);
                //EnemyAlert
                AudioManager.instance.Play("EnemyAlert");
                //EffectsController.Instance.CreateWindupEffect(soundPart.BodyPartTransform.position, soundPart.BodyPartTransform);

                currentWindupAttack = Instantiate(windUpPrefab, soundPart.BodyPartTransform.position, Quaternion.identity, soundPart.BodyPartTransform);
                windUpParticle = currentWindupAttack.GetComponent<ParticleSystem>();
                //var main = windUpParticle.main;
                //main.loop = true;

                windUpParticle.Play(true);


                currentWindupAlert = Instantiate(windUpAlertPrefab, yellPart.BodyPartTransform.position + (Vector3.up), Quaternion.identity, yellPart.BodyPartTransform);
                windUpAlertParticle = currentWindupAlert.GetComponent<ParticleSystem>();
                //var main = windUpParticle.main;
                //main.loop = true;

                windUpAlertParticle.Play(true);


                //
                /* GameObject gO = new GameObject("OneShotAudio");
                 gO.transform.position = soundPart.BodyPartTransform.position;
                 AudioSource source = gO.AddComponent<AudioSource>();
                 source.loop = true;
                 source.dopplerLevel = 0.1f;
                 source.volume = 0.3f;
                 source.pitch = Random.Range(0.8f,1);
                 source.rolloffMode = AudioRolloffMode.Linear;
                 source.minDistance = 2000;
                 source.maxDistance = 2010;
                 source.spatialBlend = 0.5f;
                 //source.outputAudioMixerGroup = Instance.mixerGroup;
                 int clipIndex = Random.Range(0, windupclips.Length);  // Mathf.FloorToInt(m * 0.999f * Instance.playerYellClips.Length); //Random.Range(0, Instance.playerYellClips.Length);//
                 if (clipIndex < windupclips.Length && clipIndex >= 0)
                 {
                     source.clip = windupclips[clipIndex]; // **** INDEX 0 to 1 less than number of clips ***
                   //  Debug.Log("clipIndex " + clipIndex + "  clips.Length " + windupclips.Length);
                 }
                 else
                 {
                   //  Debug.LogError("clipIndex " + clipIndex + "  clips.Length " + windupclips.Length);
                 }
                 source.Play();
                 */


                //IEnumerator coroutine = EmitChargeParticle(currentWindupAttack, windUpParticle, source, gO);

                IEnumerator coroutine = EmitChargeParticle(currentWindupAttack, windUpParticle, currentAttack, windUpAlertParticle, currentWindupAlert);

                StartCoroutine(coroutine);

                //playWoosh = false;


                //EffectsController.Instance.PlayWindupSound(soundPart.BodyPartTransform.position, (character.attackPower / currentAttack.maxAttackPower) * 100, soundPart.BodyPartTransform.name);



                canWindup = playWindup;

            }
        }



        public void PlayYellSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
        {
            
                //
                
               
                //
                
                //
            
        }


        //public IEnumerator EmitChargeParticle(GameObject parent, ParticleSystem parentParticle, AudioSource source, GameObject gO)


        public IEnumerator EmitChargeParticle(GameObject parent, ParticleSystem parentParticle, AttackData currentAttack, ParticleSystem parentAlertParticle, GameObject parentAlert)
        {
            //Vector3 camPos = camTransform.transform.position;
            //Vector3 velocity = Vector3.zero;
            //float elapsed = 0.0f;
            //ParticleSystem parentParticle = parent.GetComponent<ParticleSystem>();
            ParticleSystem[] windUpParticle = parent.GetComponentsInChildren<ParticleSystem>();
            ParticleSystem[] windUpAlertParticle = parentAlert.GetComponentsInChildren<ParticleSystem>();

            //var main = windUpParticle.main;
            var parentMain = parentParticle.main;
            //parentMain.startLifetime = currentAttack.attackSpeed;
            //main.
            while (windUp)
            {

                foreach (var item in windUpParticle)
                {
                    var main = item.main;
                    if (main.loop == true)
                    {
                        main.loop = true;
                    }
                }

                foreach (var item in windUpAlertParticle)
                {
                    var main = item.main;
                    if (main.loop == true)
                    {
                        main.loop = true;
                    }
                }
                //source.loop = true;
                yield return new WaitForEndOfFrame();
            }
            foreach (var item in windUpParticle)
            {
                var main = item.main;
               // main.loop = false;
                item.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            foreach (var item in windUpParticle)
            {
                var main = item.main;
                // main.loop = false;
                item.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            //source.loop = false;

            //Destroy(gO);
            //windUpParticle.Play(true);
            parentAlertParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            parentParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            //windUpParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            //camTransform.transform.localPosition = Vector3.zero;

        }

        public void checkHitOrMiss()
        {
            attack = character.simpleattack;
            windUp = character.simplewind;
            /*attackButtonClickTimer = character.Remember<float>("attackButtonClickTimer");
            attackPower = character.Remember<float>("attackPower");
            currentAttack = character.Remember<AttackData>("currentAttack");*/

            if (attack)
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

            if (!attack || windUp)
            {
                if (attacking)
                {
                    if (character.youHitMe)
                    {
                        successHitCounter++;
                        character.youHitMe = false;

                        //if (playerHitEvent != null)
                            playerHitEvent.Invoke();
                    }
                    else
                    {
                        missedHitCounter++;
                        //if (playerMissEvent != null)
                            playerMissEvent.Invoke();

                    }
                    attackCounter++;
                    attacking = false;
                }

            }
        }
    }
}

