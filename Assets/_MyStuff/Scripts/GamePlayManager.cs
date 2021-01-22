using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace garagekitgames
{
    public class GamePlayManager : UnitySingleton<GamePlayManager>
    {
        ///This Guy Controls
        ///game start, game end, game running, raises events for game start, end and every other event that occurs in the game play
        ///Events : 
        ///1. Game start
        ///2. Game End
        ///3. Player Hit
        ///4. Player Died
        ///5. Enemy Hit
        ///6. Enemy Died
        ///7. Display Pause Menu
        ///8. Go To Main Menu
        public UnityEvent OnGameStart;
        public UnityEvent OnGameRestart;
        public UnityEvent OnGameEnd;
        public UnityEvent OnGameRoundEnd;
        public UnityEvent OnGamePause;
        public UnityEvent OnGameUnPause;
        public UnityEvent OnGameLaunch;
        public UnityEvent OnGameWon;
        public UnityEvent OnGameLost;

        public bool gameRestarted;
        public bool gameStarted;
        public bool gamePaused = false;
        public bool gameEnded;

        public Animator gameplayStateMachine;

        public BoolVariable showTutorialOnStart;

        public List<GameObject> tutorialTarget;
        public BoolVariable tutorialSuccess;
        public TestManager enemyManager;

        public IntVariable cashValue;
        public IntVariable resurrectionCost;

        public float timeLeft = 3.0f;
        //public bool gameStarted;
        public Text startText; // used for showing countdown from 3, 2, 1 
        public GameObject tapToStartGO;
        bool tapped;
        public bool useCountDown;
        // Use this for initialization
        private void Awake()
        {
            
            gameplayStateMachine = this.GetComponent<Animator>();
            tutorialTarget = new List<GameObject>(GameObject.FindGameObjectsWithTag("tutorialTarget"));
            enemyManager = GameObject.FindObjectOfType<TestManager>();
        }


        void Start()
        {
            startText.transform.gameObject.SetActive(false);
            AudioManager.instance.FadeInCaller("BGM1", 0.1f, 0.3f);
            AudioManager.instance.FadeOutCaller("BGM2", 0.1f);
            OnGameLaunch.Invoke();
            //gameStarted = true;
            //enemyManager.stopDoingShit = true;
            /*if(tutorialTarget[1] && showTutorialOnStart.value)
            {
                EffectsController.Instance.CreateFloatingTextEffectBasic(tutorialTarget[0].transform.position, "Tap on Target to attack", Color.white, tutorialTarget[0].transform, (Vector3.up * 2.5f), 0.5f);
            }*/

        }

        public IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(3);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0) && !tapped)
            {
                tapped = true; 
                gameStarted = true;
                tapToStartGO.SetActive(false);
            }

            if (gameStarted)
            {
                if(useCountDown)
                {
                    timeLeft -= Time.deltaTime;
                    startText.text = (timeLeft).ToString("0");
                    startText.transform.gameObject.SetActive(true);

                    if (timeLeft > 2 && timeLeft < 3)
                    {
                        //AudioManager.instance.Play("Two");
                    }
                    if (timeLeft > 1 && timeLeft < 2)
                    {
                        // AudioManager.instance.Play("One");
                    }

                    if (timeLeft > 0 && timeLeft < 1)
                    {
                        startText.text = "GO";
                        // AudioManager.instance.Play("Go");
                        //Do something useful or Load a new game scene depending on your use-case
                    }
                    if (timeLeft < 0)
                    {
                        startText.transform.gameObject.SetActive(false);
                        //OnCountdownEnd.Invoke();
                        OnGameStart.Invoke();
                        gameStarted = false;
                    }
                }
                else
                {
                    startText.transform.gameObject.SetActive(false);
                    //OnCountdownEnd.Invoke();
                    OnGameStart.Invoke();
                    gameStarted = false;
                }
                
            }
        }

        public BoolVariable IsTutorialSuccess()
        {
            
            return tutorialSuccess;
        }

        //Move to GamePlay Manager 
        public void Restart(string levelName)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //if(levelName.Contains("MainMenu"))
            //{

            //}
            //Application.LoadLevel(levelName);
        }

        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
            //Application.LoadLevel(levelName);
            //NotificationsManager.Instance.RequestRating();
        }

        public void OnTutorialSuccess()
        {
            //OnGameStart.Invoke();

            //enemyManager.stopDoingShit = false;
            /* tutorialSuccess.value = true;

             if (tutorialSuccess.value)
             {
                 EffectsController.Instance.CreateFloatingTextEffectBasic(tutorialTarget[1].transform.position, "Hold the tap longer for a stronger and accurate attack !", Color.white, tutorialTarget[1].transform, (Vector3.up * 2.5f), 0);
                 //Show dialogue box for showing tutorial on start
             }*/
        }

        public void OnGamePaused()
        {
            OnGamePause.Invoke();
            gamePaused = true;
        }

        public void OnGameUnPaused()
        {
            OnGameUnPause.Invoke();
            gamePaused = false;
        }
        public void OnPlayerDeath()
        {
            if (cashValue.value >= resurrectionCost.value)
            {
                //cashValue.value = cashValue.value - resurrectionCost.value;
                //updateCashUI.Invoke();
                // PersistableSO.Instance.Save();
               // gotEnoughCash.Invoke();

                OnGameEnd.Invoke();
            }
            else
            {
                //Display other cash buy options
                // notEnoughCash.Invoke();

                OnGameEnd.Invoke();
               // OnGameRoundEnd.Invoke();
            }

            
            AudioManager.instance.FadeOutCaller("BGM1", 0.1f);
            gameEnded = true;
            //uncomment this
            PersistableSO.Instance.Save();
        }

        public void OnPlayerSpawned()
        {

        }

        public void OnPlayerReSpawned()
        {
            AudioManager.instance.FadeInCaller("BGM1", 0.1f, 0.3f);
            gameEnded = false;
        }

        public void OnEnemyDeath()
        {

        }

        public void OnEnemySpawn()
        {

        }

        public void DisplayPauseMenu()
        {

        }

        public void DisplayGameOverMenu()
        {


        }
    }
}

