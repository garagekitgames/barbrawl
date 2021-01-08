using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using SO;

namespace garagekitgames
{
    public class ScoreController : MonoBehaviour
    {

        public IntVariable currentScore;
        public IntVariable currentLevel;
        public IntVariable comboMultiplier;
        public IntVariable highScore;
        public IntVariable currentCoinsCollected;

        public int scoreAddedPerKill = 1;
        public int comboBreakerLimit = 2;

        private int missTracker;

        public UnityEvent updateScoreUI;
        public UnityEvent updateCoinsUI;
        public UnityEvent updateComboUI;
        public UnityEvent updateComboIncreaseUI;
        public UnityEvent updateComboDecreaseUI;
        public UnityEvent updateComboBrokenUI;

        public UnityEvent OnNewHighScoreUI;

        public bool updateScore = false;
        public bool playerHit = false;
        public int enemyDead = 0;

        public BoolVariable newBest;
        /*public UnityEvent hit;
        public UnityEvent OnDisableEvent;
        */
        // Use this for initialization


        void Awake()
        {
            currentScore.value = 0;
            newBest.value = false;
            comboMultiplier.value = 0;
            missTracker = 0;

            currentCoinsCollected.value = 0;


        }

        private void Start()
        {
            //updateScoreUI.Invoke();
            updateScoreUI.Invoke();
            updateCoinsUI.Invoke();
            // scoreAddedPerKill = currentLevel.value;
            currentCoinsCollected.value = 0;
        }

       
        public IEnumerator updateScoreUICoroutine()
        {
            yield return new WaitForSeconds(0.3f);
           // scoreAddedPerKill = currentLevel.value;
            currentScore.value += enemyDead * scoreAddedPerKill * comboMultiplier.value;
            enemyDead = 0;
           // Debug.Log("Enemy Dead" + comboMultiplier.value);
            if (currentScore.value > highScore.value)
            {

                highScore.value = currentScore.value;
                OnNewHighScoreUI.Invoke();
                newBest.value = true;
            }
            //scoreAddedPerKill *= comboMultiplier.value;
            //currentScore.Add(scoreAddedPerKill);
           // EffectsController.Instance.CreateFloatingTextWithDelayForScore(Vector3.zero, "+ "+ (enemyDead * scoreAddedPerKill * comboMultiplier.value), Color.white, this.transform, (Vector3.up * 2.5f), 0.1f);
            updateScoreUI.Invoke();
        }
        // Update is called once per frame
        void Update()
        {
           // scoreAddedPerKill = currentLevel.value;
            if (missTracker >= comboBreakerLimit)
            {
                comboMultiplier.value = 0;
                missTracker = 0;
                //updateComboUI.Invoke();
                updateComboBrokenUI.Invoke();
            }
            
            if(updateScore && playerHit)
            {
                updateScore = false;
                playerHit = false;
                IEnumerator coroutine = updateScoreUICoroutine();
                StartCoroutine(coroutine);
            }


        }

        public void PlayerHit()
        {
            playerHit = true;
            comboMultiplier.value++;
            missTracker = 0;
            updateComboUI.Invoke();
            updateComboIncreaseUI.Invoke();
            updateCoinsUI.Invoke();

            //Debug.Log("Player Hit" + comboMultiplier.value);

        }

        public void PlayerMiss()
        {

            missTracker++;
            

           

            /*if (missTracker >= comboBreakerLimit)
            {
                comboMultiplier.value = 0;
                missTracker = 0;
                //updateComboUI.Invoke();
                updateComboBrokenUI.Invoke();
            }
            else
            {
                updateComboUI.Invoke();
                updateComboDecreaseUI.Invoke();
            }*/

            if (!(missTracker >= comboBreakerLimit) && (comboMultiplier.value >= 1))
            {
                //comboMultiplier.value = 0;
                // missTracker = 0;
                //updateComboUI.Invoke();
                //updateComboBrokenUI.Invoke();

                updateComboUI.Invoke();
                updateComboDecreaseUI.Invoke();
            }
            else
            {
                //updateComboUI.Invoke();
                //updateComboBrokenUI.Invoke();
            }



        }

        public void PlayerGotHit()
        {
            missTracker++;
            

            if (!(missTracker >= comboBreakerLimit) && (comboMultiplier.value >= 1))
            {
                //comboMultiplier.value = 0;
                // missTracker = 0;
                //updateComboUI.Invoke();
                //updateComboBrokenUI.Invoke();

                updateComboUI.Invoke();
                updateComboDecreaseUI.Invoke();
            }
            else
            {
               // updateComboUI.Invoke();
               // updateComboBrokenUI.Invoke();
            }

        }

        public void PlayerDead()
        {
            comboMultiplier.value = 0;
            missTracker = 0;
            //updateComboUI.Invoke();

            

        }

        public void EnemyDead()
        {
            updateScore = true;
            enemyDead++;
            /*currentScore.value += scoreAddedPerKill * comboMultiplier.value;
            if (currentScore.value > highScore.value)
            {

                highScore.value = currentScore.value;
            }
            //scoreAddedPerKill *= comboMultiplier.value;
            //currentScore.Add(scoreAddedPerKill);

            updateScoreUI.Invoke();*/
        }


    }
}

