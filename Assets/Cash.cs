using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.Events;

namespace garagekitgames
{
    public class Cash : MonoBehaviour
    {
        public TransformVariable target;
        Vector3 velocity =  Vector3.zero;
        public RangedFloat timeModifier;
        bool isFollowing = false;
        Transform targetTransform;
        public int cashValue = 1;

        public IntVariable cashCollected;
        public IntVariable cashCollectedThisRound;
        public UnityEvent updateCoinsUI;
        //public float timeDuration;
        //public float speedModifier;
        // Use this for initialization

        private Rigidbody rb;
        void Start()
        {
            targetTransform = target.value;
        }

        public void AddCoins(int value)
        {
            cashCollected.Add(value);
            cashCollectedThisRound.Add(value);
        }

        public void OnCollisionEnter(Collision collision)
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {

            if (transform.root != other.transform.root)
            {

                CharacterThinker cT = other.transform.root.GetComponent<CharacterThinker>();

                if (cT)
                {
                    if (cT.amIMainPlayer)
                    {
                        if(isFollowing)
                        {
                            AudioManager.instance.Play("CoinDrop1");
                            AddCoins(cashValue);
                            updateCoinsUI.Invoke();
                            gameObject.SetActive(false);
                        }
                            
                    }
                }

                //Change to disable and use pooling 
            }
        }

        private void OnEnable()
        {
            targetTransform = target.value;
            rb = this.GetComponent<Rigidbody>();
            //Invoke("StartFollowing", Random.Range(timeModifier.minValue, timeModifier.maxValue));
            Invoke("StartFollowing", Random.Range(timeModifier.minValue, timeModifier.maxValue));
            isFollowing = false;
            rb.isKinematic = false;
        }

        private void OnDisable()
        {

            isFollowing = false;
            rb.isKinematic = false;
            //targetTransform = target.value;
            //rb = this.GetComponent<Rigidbody>();
            //Invoke("StartFollowing", Random.Range(timeModifier.minValue, timeModifier.maxValue));
        }

        public void Deactivate()
        {
            AddCoins(cashValue);
            updateCoinsUI.Invoke();
            this.gameObject.SetActive(false);
        }

        public void StartFollowing()
        {
            isFollowing = true;

        }
        // Update is called once per frame
        void Update()
        {
            if(isFollowing)
            {
                rb.isKinematic = true;
                this.transform.position = Vector3.SmoothDamp(this.transform.position, targetTransform.position, ref velocity, Time.deltaTime * Random.Range(7, 11));

                //rb.AddForce((targetTransform.position - transform.position).normalized * Random.Range(timeModifier.minValue, timeModifier.maxValue), ForceMode.Impulse);

            }
        }
    }
}

