using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
namespace garagekitgames
{
    public class PlayerManager : UnitySingleton<PlayerManager>
    {

        public LevelData levelData;
        public GameObject Player;
        public Vector3Variable playerSpawnPoint;
        public CharacterRuntimeSet mainPlayerCharacterSet;
        public CharacterThinker currentPlayer;


        private IEnumerator coroutine;

        public UnityEvent afterDeathEvent;

        public IntVariable cashValue;

        public IntVariable resurrectionCost;

        public UnityEvent updateCashUI;
        public UnityEvent notEnoughCash;

        public UnityEvent payToContinueWorked;
        // public bool destroy = false;
        public Slider healthSlider;
        public Slider powerSlider;
        public Text characterName;
        public TextMeshProUGUI nextButtonText;
        public TextMeshProUGUI homeButtonText;

        public Text healthLevel;
        public Text powerLevel;

        public TextMeshProUGUI uihealthLevel;
        public TextMeshProUGUI uipowerLevel;

        public string statLevelPrefix = "Lvl ";
        private void Awake()
        {
            Player = levelData.Player;
            playerSpawnPoint = levelData.spawnPoint;
            Spawn();
        }

        public void OnRoundOver()
        {
            UpdateSelectedLevel();
        }
        // Use this for initialization
        void Start()
        {

            currentPlayer.attackPow = levelData.currentPow;
            currentPlayer.health.currentHealth = levelData.currentHealth;
            currentPlayer.health.maxHealth = levelData.currentHealth;
            currentPlayer.health.currentHP.Value = levelData.currentHealth;
            currentPlayer.health.maxHP.Value = levelData.currentHealth;

            UpdateSelectedLevel();

        }

        public void UpdateSelectedLevel()
        {
            //AudioManager.instance.Play("NextButton");
            //var cT = new CharacterThinker();
            //Debug.Log("Level Updated Clicked");
            //currentLevel = levelData[index];
            //currentLevel.Enemies = levelData[index].Enemies;
            //currentLevel.Player = levelData[index].Player;
            //currentLevel.spawnPoint = levelData[index].spawnPoint;
            //currentLevel.enemyGroups = levelData[index].enemyGroups;
            //currentLevel.maxHealth = levels[index].maxHealth;
            //currentLevel.currentHealth = levels[index].currentHealth;
            //currentLevel.maxPow = levels[index].maxPow;
            //currentLevel.currentPow = levels[index].currentPow;
            //mainCam.target = levelCharacters[index].transform.GetComponent<CharacterThinker>().bpHolder.bodyParts[lookAtPart].BodyPartTransform;
            //buttonCashValueUI.text = levels[index].levelPrice.ToString();
            healthSlider.value = levelData.currentHealth;
            healthSlider.minValue = 0;
            healthSlider.maxValue = levelData.maxHealth;
            powerSlider.value = levelData.currentPow;
            powerSlider.minValue = 0;
            powerSlider.maxValue = levelData.maxPow;
            characterName.text = levelData.charName;

            healthLevel.text = statLevelPrefix + levelData.currentHealth;
            powerLevel.text = statLevelPrefix + levelData.currentPow;

            if (currentPlayer.health.alive)
            {
                nextButtonText.text = "Next";
            }
            else
            {
                nextButtonText.text = "Retry";
            }

            //homeButtonText
            //if (currentPlayer.health.alive)
            //{
            //    homeButtonText.text = "Change Hero";
            //}
            //else
            //{
            //    nextButtonText.text = "Retry";
            //}

            //if (levels[index].isUnlocked)
            //{
            //    playButton.SetActive(true);
            //    buyButton.SetActive(false);
            //    lockImage.SetActive(false);
            //}
            //else
            //{
            //    playButton.SetActive(false);
            //    buyButton.SetActive(true);
            //    lockImage.SetActive(true);
            //}

        }

        public void AttemptHealthBuy()
        {
            if (cashValue.value >= 100)
            {
                //levels[selectedIndex.value].currentHealth++;
                //updateCashUI.Invoke();

                if ((levelData.currentHealth + 1 <= levelData.maxHealth))
                {
                    cashValue.value = cashValue.value - 100;

                    levelData.currentHealth++;
                    GameManager.Instance.currentLevelInfo.currentHealth++;
                    updateCashUI.Invoke();

                    PersistableSO.Instance.Save();
                    AudioManager.instance.Play("ShopButton");
                    UpdateSelectedLevel();
                }
                else
                {
                    //Dont Upgrade, disable upgrade button
                }



            }
            else
            {
                //Display Buy Coins Panel
                notEnoughCash.Invoke();

            }

        }

        public void AttemptPowerBuy()
        {
            if (cashValue.value >= 100)
            {
                if ((levelData.currentPow + 1 <= levelData.maxPow))
                {
                    cashValue.value = cashValue.value - 100;

                    levelData.currentPow++;
                    GameManager.Instance.currentLevelInfo.currentPow++;
                    updateCashUI.Invoke();

                    PersistableSO.Instance.Save();
                    AudioManager.instance.Play("ShopButton");
                    UpdateSelectedLevel();
                }
                else
                {
                    //Dont Upgrade, disable upgrade button
                }




            }
            else
            {
                //Display Buy Coins Panel
                notEnoughCash.Invoke();

            }

        }

        public void SetStopDoingShit(bool value)
        {
            //this.stopDoingShit = value;
            //foreach (var enemy in RuntimeSet.Items)
            // {
            currentPlayer.SetStopDoingShit(value);
                //var AI = enemy.GetComponent<CharacterBasicAI>();
               // AI.SetStopDoingShit(value);
            //}
        }

        void Spawn()
        {
            // If the player has no health left...
            /*if (mainPlayerHealth.currentHP <= 0f)
            {
                // ... exit the function.
                return;
            }*/

            // Find a random index between zero and one less than the number of spawn points.
            //int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            //int enemyIndex = Random.Range(0, enemy.Length);

            //objectPools[enemyIndex].TryGetNextObject(spawnPoints[spawnPointIndex].transform.position, spawnPoints[spawnPointIndex].transform.rotation);
            //GameObject objectthatwasSpawned = pools[enemyIndex].InstantiateFromPool(spawnPoints[spawnPointIndex].transform.position);
            //poolArray[enemyIndex]
            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            Instantiate(Player, playerSpawnPoint.value, Quaternion.identity);

            foreach (var character in mainPlayerCharacterSet.Items)
            {
                currentPlayer = character;
                //enemy.SetStopDoingShit(value);
               // var AI = enemy.GetComponent<CharacterBasicAI>();
               // AI.SetStopDoingShit(value);
            }
            //print("Spawned : " + objectthatwasSpawned.name+" | "+ " From Pool : "+ pools[enemyIndex].pooledItems.Count + " | " + " Pools Size : " + pools.Count);
            //spawnCounter = 0;
        }

        public void RespawnCurrentPlayer()
        {
            //currentPlayer.gameObject.SetActive(false);
            if(cashValue.value >= resurrectionCost.value)
            {
                //Use the following two lines if the revive feature is based on coins
                //cashValue.value = cashValue.value - resurrectionCost.value;
                //updateCashUI.Invoke();
                PersistableSO.Instance.Save();
            }
            else
            {
                //Display other cash buy options
                // notEnoughCash.Invoke();
                PersistableSO.Instance.Save();
            }
            
            DisableEnableAfter(0.1f, false);
            ResurrectAfter(0.1f);
            DisableEnableAfter(0.1f, true);
            //currentPlayer.gameObject.SetActive(true);
        }

        public void RespawnCurrentPlayerPayToRevive()
        {
            //currentPlayer.gameObject.SetActive(false);
            if (cashValue.value >= resurrectionCost.value)
            {
                //Use the following two lines if the revive feature is based on coins
                cashValue.value = cashValue.value - resurrectionCost.value;
                //updateCashUI.Invoke();
                PersistableSO.Instance.Save();
                payToContinueWorked.Invoke();
            }
            else
            {
                //Display other cash buy options
                // notEnoughCash.Invoke();
                PersistableSO.Instance.Save();
            }

            /*DisableEnableAfter(0.1f, false);
            ResurrectAfter(0.1f);
            DisableEnableAfter(0.1f, true);*/
            //currentPlayer.gameObject.SetActive(true);
        }

        public void DisableEnableAfter(float sec, bool value)
        {
            coroutine = LateCall(value, sec);
            StartCoroutine(coroutine);
        }

        public void ResurrectAfter(float sec)
        {
            coroutine = LateCallResurrectEffect(sec);
            StartCoroutine(coroutine);
        }

        public void OnLevelUp()
        {
            IEnumerator coroutine2 = StopPlayerAndResumeAfter(1.5f);
            StartCoroutine(coroutine2);
        }

        //public void 
        IEnumerator LateCall(bool value, float sec)
        {

            yield return new WaitForSeconds(sec);
            afterDeathEvent.Invoke();

            
            
                currentPlayer.gameObject.SetActive(value);
            

            //Do Function here...
        }

        IEnumerator LateCallResurrectEffect(float sec)
        {

            yield return new WaitForSeconds(sec);

            EffectsController.Instance.CreateResurrectEffect(currentPlayer.gameObject.transform.position);


            // currentPlayer.gameObject.SetActive(value);


            //Do Function here...
        }

        IEnumerator StopPlayerAndResumeAfter(float sec)
        {
            SetStopDoingShit(true);
            yield return new WaitForSeconds(sec);
            SetStopDoingShit(false);


            //Do Function here...
        }

        // Update is called once per frame
        void Update()
        {
            uihealthLevel.text = statLevelPrefix + levelData.currentHealth;
            uipowerLevel.text = statLevelPrefix + levelData.currentPow;
        }
    }
}

