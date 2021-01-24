using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using EZObjectPools;
using System;
using Random = UnityEngine.Random;
using SO;
using UnityEngine.Events;

using TMPro;
namespace garagekitgames
{
    [System.Serializable]
    public class Wave
    {
        public string name;
        //public Transform enemy;
        public int count;
        public float rate;
        public EnemyGroup enemyGroup;

        public Wave(string name, int count, float rate, EnemyGroup enemyGroup)
        {
            this.name = name;
            this.count = count;
            this.rate = rate;
            this.enemyGroup = enemyGroup;
        }
    }
    public class EnemyWaveSpawner : MonoBehaviour
    {
        public enum SpawnState { Spawning, Waiting, Counting, Done};

        public enum RoundType { Normal, Bonus, Boss };

        public List<Wave> waves;
        public int nextWave = 0;
        public float timeBetweenWaves = 5;
        public float waveCountDown;
        // Start is called before the first frame update
        public SpawnState state = SpawnState.Counting;
        public RoundType roundType;


        public CharacterRuntimeSet enemyRuntimeSet;

        public GameObject[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
        //private List<EZObjectPool> newObjectPools;
        public List<GameObject[]> enemyList;

        private List<List<EZObjectPool>> levelBasedObjectPool;


        [Header("Seed Settings")]
        public int seed = 1;

        private int enemyCount = 0;

        public List<EnemyGroup> enemyGroups;
        public List<EnemyGroup> bossEnemyGroups;
        public List<EnemyGroup> bonusEnemyGroups;

        public IntVariable currentLevel;
        public IntVariable nextLevel;

        public bool stopDoingShit = false;

        public UnityEvent OnWaveComplete;
        public UnityEvent OnAllWavesCompleted;

        public StringVariable waveName;
        public IntVariable waveNumber;
        public IntVariable enemiesLeft;
        public IntVariable totalEnemies;
        public IntVariable enemiesKilled;

        public RangedInt enemyHealth;
        public RangedInt enemyPow;

        public StringVariable RoundName;

        public UnityEvent OnRoundBegin;

        public CharacterThinker currentPlayer;
        //public LevelData currentLevel
        public StringVariable alertString;

        public string waveCleared = "Wave Cleared !";
        public string roundCleared = "Level Cleared !";
        public string bossRoundName = "Boss Round !";
        public string bonusRound = "$$ BONUS ROUND $$";
        public string roundBegin = "Get Ready !";

        public UnityEvent OnRoundBeginMessage;
        public UnityEvent OnAlertStatusEvent;

        public TextMeshProUGUI enemyHealthLevel;
        public TextMeshProUGUI enemyPowerLevel;
        public TextMeshProUGUI enemyKillCount;

        public GameObject[] levels;
        private void Awake()
        {
            Random.InitState(currentLevel.value);
            enemiesKilled.value = 0;
            enemiesLeft.value = 0;
            totalEnemies.value = 0;
            SetupLevel();

        }

        void Start()
        {

            
            SetupWaves();
            waveCountDown = timeBetweenWaves;
            spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
            //newObjectPools = new List<EZObjectPool>();
            enemyList = new List<GameObject[]>();
            levelBasedObjectPool = new List<List<EZObjectPool>>();
            foreach (var wave in waves)
            {
                var tempEnemyArray = wave.enemyGroup.Enemies.ToArray();

                // var newObjectPools = new List<EZObjectPool>();
                // foreach (var item2 in newEnemy)
                // {
                //     EZObjectPool objectPool = EZObjectPool.CreateObjectPool(item2, item2.name, poolsize, true, true, false);

                //      newObjectPools.Add(objectPool);
                //  }
                enemyList.Add(tempEnemyArray);
                //  levelBasedObjectPool.Add(newObjectPools);
            }

            int enemyCounts = 0;
            foreach (var wave in waves)
            {
                var newObjectPools2 = new List<EZObjectPool>();
                foreach (var enemy in wave.enemyGroup.Enemies)
                {
                    EZObjectPool objectPool = EZObjectPool.CreateObjectPool(enemy, wave.name+"_"+enemy.name, 3, true, true, false);
                    //ObjectPool pool = new ObjectPool(item, poolsize, true);
                    //newObjectPools.Add(objectPool);
                    newObjectPools2.Add(objectPool);
                    //enemyCount++;
                }
                levelBasedObjectPool.Add(newObjectPools2);
                enemyCounts += wave.count;
            }

            totalEnemies.value = enemyCounts;

            currentPlayer = PlayerManager.Instance.currentPlayer;

            Invoke("RoundBeginMessage", 0.5f);
        }

        private void SetupLevel()
        {
            Random.InitState(currentLevel.value);

            if(currentLevel.value == 1)
            {
                int selectedLevel = 0;

                levels[selectedLevel].SetActive(true);
            }
            else
            {
                int selectedLevel = Random.Range(0, levels.Length);

                levels[selectedLevel].SetActive(true);
            }

            
        }

        public void RoundBeginMessage()
        {
            if(roundType == RoundType.Normal)
            {
                alertString.value = roundBegin;

            }
            else if (roundType == RoundType.Boss)
            {
                alertString.value = bossRoundName;

            }
            else if (roundType == RoundType.Bonus)
            {
                alertString.value = bonusRound;

            }
            OnRoundBeginMessage.Invoke();
        }

        public void SetupWaves()
        {
            if((currentLevel.value % 5) == 0)
            {
                roundType = RoundType.Boss;
                //every fifth round
                Random.InitState(currentLevel.value);
                int maxWaves = 1;
                int noOfWaves = 1;

                if (waves.Count <= 0)
                {
                    waves = new List<Wave>();

                    for (int i = 0; i < noOfWaves; i++)
                    {
                        var enemyGroup = bossEnemyGroups[Random.Range(0, bossEnemyGroups.Count)];
                        int count = Mathf.Clamp(Random.Range(1, (currentLevel.value / 2) + 1), 1, 6);
                        waves.Add(new Wave(enemyGroup.groupName, count, 4, enemyGroup));
                    }

                }
            }
            else if((currentLevel.value % 4) == 0)
            {
                roundType = RoundType.Bonus;
                Random.InitState(currentLevel.value);
                int maxWaves = 1;
                int noOfWaves = 1;

                if (waves.Count <= 0)
                {
                    waves = new List<Wave>();

                    for (int i = 0; i < noOfWaves; i++)
                    {
                        var enemyGroup = bonusEnemyGroups[Random.Range(0, bonusEnemyGroups.Count)];
                        int count = Random.Range(6, 10);
                        waves.Add(new Wave(enemyGroup.groupName, count, 2, enemyGroup));
                    }

                }
            }
            else
            {
                roundType = RoundType.Normal;
                Random.InitState(currentLevel.value);
                int maxWaves = currentLevel.value;
                int noOfWaves = Mathf.Clamp(maxWaves, 1, 3);

                if (waves.Count <= 0)
                {
                    waves = new List<Wave>();

                    for (int i = 0; i < noOfWaves; i++)
                    {
                        var enemyGroup = enemyGroups[Random.Range(0, enemyGroups.Count)];
                        int count = Mathf.Clamp(Random.Range(1, (currentLevel.value / 2) + 1), 1, 6);
                        waves.Add(new Wave(enemyGroup.groupName, count, 1, enemyGroup));
                    }

                }
            }
            


            
        }
        // Update is called once per frame
        void Update()
        {
            if (stopDoingShit)
                return;

            nextLevel.value = currentLevel.value + 1;
            enemiesLeft.value = totalEnemies.value - enemiesKilled.value;
            waveNumber.value = nextWave + 1;
            waveName.value = waves[nextWave].name;

            if (roundType == RoundType.Normal)
            {
                //if (currentLevel.value <= 20)
                //{
                //    enemyThinker.attackPow = Mathf.Clamp((currentLevel.value / 6) + 1, 1, 200);//Random.Range((int)enemyPow.minValue,(int)enemyPow.maxValue);
                //    enemyThinker.health.currentHP.Value = Mathf.Clamp((currentLevel.value / 3) + 1, 1, 200);//Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                //    enemyThinker.health.maxHP.Value = Mathf.Clamp((currentLevel.value / 3) + 1, 1, 200);// Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);

                //}
                //else
                //{

                //    enemyThinker.attackPow = Mathf.Clamp((currentLevel.value / 4) + 1, 1, 200);//Random.Range((int)enemyPow.minValue,(int)enemyPow.maxValue);
                //    enemyThinker.health.currentHP.Value = Mathf.Clamp((currentLevel.value / 2) + 1, 1, 200);//Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                //    enemyThinker.health.maxHP.Value = Mathf.Clamp((currentLevel.value / 2) + 1, 1, 200);// Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);

                //}

                if (currentLevel.value <= 20)
                {
                    enemyHealthLevel.text = "Lvl " + Mathf.Clamp((currentLevel.value / 3) + 1, 1, 200).ToString();
                    enemyPowerLevel.text = "Lvl " + Mathf.Clamp((currentLevel.value / 6) + 1, 1, 200).ToString();
                }
                else
                {
                    enemyHealthLevel.text = "Lvl " + Mathf.Clamp((currentLevel.value / 2) + 1, 1, 200).ToString();
                    enemyPowerLevel.text = "Lvl " + Mathf.Clamp((currentLevel.value / 4) + 1, 1, 200).ToString();
                }

                    
                

            }
            else if (roundType == RoundType.Boss)
            {

                //if (currentLevel.value <= 20)
                //{
                //    enemyThinker.attackPow = Mathf.Clamp((currentLevel.value / 5) + 1, 1, 200);//Random.Range((int)enemyPow.minValue,(int)enemyPow.maxValue);
                //    enemyThinker.health.currentHP.Value = currentPlayer.attackPow * 4;//Mathf.Clamp((currentLevel.value / 5) + 1, 1, 20);//Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                //    enemyThinker.health.maxHP.Value = currentPlayer.attackPow * 4;// Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);

                //}
                //else
                //{
                //    enemyThinker.attackPow = Mathf.Clamp((currentLevel.value / 5) + 1, 1, 200);//Random.Range((int)enemyPow.minValue,(int)enemyPow.maxValue);
                //    enemyThinker.health.currentHP.Value = currentPlayer.attackPow * 10;//Mathf.Clamp((currentLevel.value / 5) + 1, 1, 20);//Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                //    enemyThinker.health.maxHP.Value = currentPlayer.attackPow * 10;// Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);

                //}


                if (currentLevel.value <= 20)
                {
                    //enemyThinker.attackPow = Mathf.Clamp((currentLevel.value / 5) + 1, 1, 200);//Random.Range((int)enemyPow.minValue,(int)enemyPow.maxValue);
                    //enemyThinker.health.currentHP.Value = currentPlayer.attackPow * 4;//Mathf.Clamp((currentLevel.value / 5) + 1, 1, 20);//Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                    //enemyThinker.health.maxHP.Value = currentPlayer.attackPow * 4;// Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);

                    enemyHealthLevel.text = "Lvl " + (currentPlayer.attackPow * 4).ToString();
                    enemyPowerLevel.text = "Lvl " + Mathf.Clamp((currentLevel.value / 5) + 1, 1, 200).ToString();
                }
                else
                {
                    //enemyThinker.attackPow = Mathf.Clamp((currentLevel.value / 5) + 1, 1, 200);//Random.Range((int)enemyPow.minValue,(int)enemyPow.maxValue);
                    //enemyThinker.health.currentHP.Value = currentPlayer.attackPow * 10;//Mathf.Clamp((currentLevel.value / 5) + 1, 1, 20);//Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                    //enemyThinker.health.maxHP.Value = currentPlayer.attackPow * 10;// Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);

                    enemyHealthLevel.text = "Lvl " + (currentPlayer.attackPow * 10).ToString();
                    enemyPowerLevel.text = "Lvl " + Mathf.Clamp((currentLevel.value / 5) + 1, 1, 200).ToString();
                }

                //enemyHealthLevel.text =  "Lvl "+(currentPlayer.attackPow * 10).ToString();
                //enemyPowerLevel.text = "Lvl " + Mathf.Clamp((currentLevel.value / 5) + 1, 1, 200).ToString();
            }
            else if (roundType == RoundType.Bonus)
            {
                enemyHealthLevel.text = "Lvl " + "1";
                enemyPowerLevel.text = "Lvl " + "1";
            }

            enemyKillCount.text = enemiesKilled.value + "/" + totalEnemies.value;

            if (state == SpawnState.Done)
            {
                return;
            }

            if(state == SpawnState.Waiting)
            {
                // check if enemies are alive 
                if(!IsEnemyAlive())
                {
                    //begin new round 
                    WaveCompleted();
                    
                   
                }
                else
                {
                    return;
                }
            }
            if(waveCountDown <= 0)
            {
                if(state != SpawnState.Spawning)
                {
                    //start spawning
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
            else
            {
                waveCountDown -= Time.deltaTime;
            }
        }


        public bool IsEnemyAlive()
        {
            return (enemyRuntimeSet.Items.Count > 0);
        }


        public void WaveCompleted()
        {
            Debug.Log("WaveCompleted!");

            state = SpawnState.Counting;
            waveCountDown = timeBetweenWaves;

            if (nextWave + 1 > waves.Count - 1)
            {
                state = SpawnState.Done;
                //nextWave = 0;
                Debug.Log("All Waves Completed");
                alertString.value = roundCleared;
                OnAlertStatusEvent.Invoke();
                OnAllWavesCompleted.Invoke();
                Invoke("IncreaseLevel", 1);
                // set nextWave = 0 to loop else return and show round over
                //nextWave = 0;
                return;
            }
            else
            {
                //alertString.value = "Wave "+ (nextWave + 1) + " Cleared !";
                Debug.Log("WaveCompleted!");
                //alertString.value = waveCleared;
                 OnWaveComplete.Invoke();
                //state = SpawnState.Counting;
                //waveCountDown = timeBetweenWaves;
                nextWave++;
                alertString.value = "Next Wave : " + waves[nextWave].name;
                OnAlertStatusEvent.Invoke();

                //Invoke("DisplayNextWave", 0.5f);
            }

        }

        public void IncreaseLevel()
        {
            currentLevel.value++;

            PersistableSO.Instance.Save();
        }

        public void DisplayNextWave()
        {
            alertString.value = "Next Wave : " + waves[nextWave].name;
           // Debug.Log("Next Wave!");
            //alertString.value = waveCleared;
            OnAlertStatusEvent.Invoke();

        }

        IEnumerator SpawnWave(Wave _wave)
        {
            

            Debug.Log("Spawning Wav : " + _wave.name);
            state = SpawnState.Spawning;

            //Spawn

            for (int i = 0; i < _wave.count; i++)
            {
                while (stopDoingShit)
                {
                    yield return new WaitForEndOfFrame();
                }
                //Spawn
                SpawnEnemy(_wave);

                yield return new WaitForSeconds(1f / _wave.rate);
            }

            state = SpawnState.Waiting;

            yield break;
        }


        //leveling methods
        public void OnEnemyDeath(int e)
        {
            if (stopDoingShit)
                return;

            enemiesKilled.value += e;
            //if (enemiesKilled.value >= expLeft.value)
            //{
            //    LvlUp();
            //}
        }

        public void KillAfter(float sec)
        {
            IEnumerator coroutine = LateCall(sec);
            StartCoroutine(coroutine);
        }

        //public void 
        IEnumerator LateCall(float sec)
        {

            yield return new WaitForSeconds(sec);
            foreach (var enemy in enemyRuntimeSet.Items)
            {
                enemy.SetStopDoingShit(true);
                var AI = enemy.GetComponent<CharacterBasicAI>();
                var AIHealth = enemy.GetComponent<CharacterHealth>();
                AIHealth.applyDamage(1000);

                AI.SetStopDoingShit(true);
            }

            //Do Function here...
        }

        IEnumerator StartStopDoingShit(float sec)
        {
            SetStopDoingShit(true);
            yield return new WaitForSeconds(sec);
            SetStopDoingShit(false);

            //Do Function here...
        }

        IEnumerator GiveLevelUpReward(float sec)
        {
            //SetStopDoingShit(true);
            yield return new WaitForSeconds(sec);
            //EffectsController.Instance.CreateCash(Vector3.up * 2 + Random.insideUnitSphere, currentLevel.value * 2);
            //AddCoins(currentLevel.value * 2);


            //Do Function here...
        }

        /*public void AddCoins(int value)
        {
            cashCollected.Add(value);
            cashCollectedThisRound.Add(value);
        }*/

        public void SetStopDoingShit(bool value)
        {
            // Debug.Log("Stop Doing Shit Value : " + value);
            stopDoingShit = value;
            foreach (var enemy in enemyRuntimeSet.Items)
            {
                enemy.SetStopDoingShit(value);
                var AI = enemy.GetComponent<CharacterBasicAI>();

                AI.SetStopDoingShit(value);
            }
        }

        public void SetStopAttacking(bool value)
        {
            if(roundType == RoundType.Normal || roundType == RoundType.Boss)
            {
                // Debug.Log("Stop Doing Shit Value : " + value);
                //stopDoingShit = value;
                foreach (var enemy in enemyRuntimeSet.Items)
                {
                    //enemy.stopAttacking = value;
                    var AI = enemy.GetComponent<CharacterBasicAI>();

                    //AI.stopAttack = value;

                    AI.SetStopAttackFor(5f);
                }
            }
            
        }


        void SpawnEnemy(Wave _enemyWave)
        {

            


            
            
            //// spawn enemy
            //int newSpawnPointIndex = Random.Range(0, spawnPoints.Length); //Random.seed 

            //int newEnemyIndex = Random.Range(0, enemyList[waves.IndexOf(_enemyWave)].Length);

            ////Debug.Log("enemyList.Count  : " + enemyList.Count);
            //GameObject enemyGO;
            //levelBasedObjectPool[waves.IndexOf(_enemyWave)][newEnemyIndex].TryGetNextObject(spawnPoints[newSpawnPointIndex].transform.position, spawnPoints[newSpawnPointIndex].transform.rotation, out enemyGO);

            //var enemyThinker = enemyGO.GetComponent<CharacterThinker>();
            //var enemyAI = enemyGO.GetComponent<CharacterBasicAI>();

            if(roundType == RoundType.Normal)
            {
                Random.InitState(currentLevel.value + waves.IndexOf(_enemyWave) + enemyCount);
                // spawn enemy
                int newSpawnPointIndex = Random.Range(0, spawnPoints.Length); //Random.seed 

                int newEnemyIndex = Random.Range(0, enemyList[waves.IndexOf(_enemyWave)].Length);

                //Debug.Log("enemyList.Count  : " + enemyList.Count);
                GameObject enemyGO;
                levelBasedObjectPool[waves.IndexOf(_enemyWave)][newEnemyIndex].TryGetNextObject(spawnPoints[newSpawnPointIndex].transform.position, spawnPoints[newSpawnPointIndex].transform.rotation, out enemyGO);

                var enemyThinker = enemyGO.GetComponent<CharacterThinker>();
                var enemyAI = enemyGO.GetComponent<CharacterBasicAI>();


                if(currentLevel.value <= 20)
                {
                    enemyThinker.attackPow = Mathf.Clamp((currentLevel.value / 6) + 1, 1, 200);//Random.Range((int)enemyPow.minValue,(int)enemyPow.maxValue);
                    enemyThinker.health.currentHP.Value = Mathf.Clamp((currentLevel.value / 3) + 1, 1, 200);//Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                    enemyThinker.health.maxHP.Value = Mathf.Clamp((currentLevel.value / 3) + 1, 1, 200);// Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);

                }
                else
                {

                    enemyThinker.attackPow = Mathf.Clamp((currentLevel.value / 4) + 1, 1, 200);//Random.Range((int)enemyPow.minValue,(int)enemyPow.maxValue);
                    enemyThinker.health.currentHP.Value = Mathf.Clamp((currentLevel.value / 2) + 1, 1, 200);//Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                    enemyThinker.health.maxHP.Value = Mathf.Clamp((currentLevel.value / 2) + 1, 1, 200);// Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);

                }

                enemyThinker.coinsToDrop = 3;

                enemyAI.attackDelay = enemyAI.timeBetweenShots.maxValue;//Mathf.Clamp(timeBetweenShots.minValue / difficultyMultiplier.value, timeBetweenShots.minValue, timeBetweenShots.maxValue);
                enemyAI.attackSpeed = enemyAI.chargeTimePerDistance.maxValue;//Mathf.Clamp(chargeTimePerDistance.minValue / difficultyMultiplier.value, chargeTimePerDistance.minValue, chargeTimePerDistance.maxValue);
                if (enemyAI.agent.isActiveAndEnabled)
                {
                    enemyAI.agent.speed = Random.Range(enemyAI.moveNavigationSpeedVariable.minValue, enemyAI.moveNavigationSpeedVariable.maxValue);// enemyAI.moveNavigationSpeedVariable.maxValue;//Mathf.Clamp(moveNavigationSpeedVariable.maxValue * difficultyMultiplier.value, moveNavigationSpeedVariable.minValue, moveNavigationSpeedVariable.maxValue);
                }
                enemyThinker.speed = Random.Range(enemyAI.moveForceSpeedVariable.minValue, enemyAI.moveForceSpeedVariable.maxValue); //enemyAI.moveForceSpeedVariable.maxValue;//Mathf.Clamp(moveForceSpeedVariable.maxValue * difficultyMultiplier.value, moveForceSpeedVariable.minValue, moveForceSpeedVariable.maxValue);

                //new new
                //newObjectPools[newEnemyIndex].TryGetNextObject(spawnPoints[newSpawnPointIndex].transform.position, spawnPoints[newSpawnPointIndex].transform.rotation);

                Debug.Log("Spawning Enemy : " + _enemyWave.name + "_" + newEnemyIndex);
                Debug.Log("newSpawnPointIndex : " + newSpawnPointIndex);
                Debug.Log("newEnemyIndex : " + newEnemyIndex);
                enemyCount++;
            }
            else if(roundType == RoundType.Boss)
            {
                Random.InitState(currentLevel.value + waves.IndexOf(_enemyWave) + enemyCount);

                if(enemyCount == 0)
                {
                    // spawn enemy
                    int newSpawnPointIndex = Random.Range(0, spawnPoints.Length); //Random.seed 

                    int newEnemyIndex = 0;

                    //Debug.Log("enemyList.Count  : " + enemyList.Count);
                    GameObject enemyGO;
                    levelBasedObjectPool[waves.IndexOf(_enemyWave)][newEnemyIndex].TryGetNextObject(spawnPoints[newSpawnPointIndex].transform.position, spawnPoints[newSpawnPointIndex].transform.rotation, out enemyGO);

                    var enemyThinker = enemyGO.GetComponent<CharacterThinker>();
                    var enemyAI = enemyGO.GetComponent<CharacterBasicAI>();

                    
                    if (currentLevel.value <= 20)
                    {
                        enemyThinker.attackPow = Mathf.Clamp((currentLevel.value / 5) + 1, 1, 200);//Random.Range((int)enemyPow.minValue,(int)enemyPow.maxValue);
                        enemyThinker.health.currentHP.Value = currentPlayer.attackPow * 4;//Mathf.Clamp((currentLevel.value / 5) + 1, 1, 20);//Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                        enemyThinker.health.maxHP.Value = currentPlayer.attackPow * 4;// Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);

                    }
                    else
                    {
                        enemyThinker.attackPow = Mathf.Clamp((currentLevel.value / 5) + 1, 1, 200);//Random.Range((int)enemyPow.minValue,(int)enemyPow.maxValue);
                        enemyThinker.health.currentHP.Value = currentPlayer.attackPow * 10;//Mathf.Clamp((currentLevel.value / 5) + 1, 1, 20);//Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                        enemyThinker.health.maxHP.Value = currentPlayer.attackPow * 10;// Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);

                    }

                    enemyThinker.coinsToDrop = 5;

                    enemyAI.attackDelay = 0.5f;//enemyAI.timeBetweenShots.maxValue;//Mathf.Clamp(timeBetweenShots.minValue / difficultyMultiplier.value, timeBetweenShots.minValue, timeBetweenShots.maxValue);
                    enemyAI.attackSpeed = 2.3f;//enemyAI.chargeTimePerDistance.maxValue;//Mathf.Clamp(chargeTimePerDistance.minValue / difficultyMultiplier.value, chargeTimePerDistance.minValue, chargeTimePerDistance.maxValue);
                    if (enemyAI.agent.isActiveAndEnabled)
                    {
                        enemyAI.agent.speed = enemyAI.moveNavigationSpeedVariable.minValue;//Mathf.Clamp(moveNavigationSpeedVariable.maxValue * difficultyMultiplier.value, moveNavigationSpeedVariable.minValue, moveNavigationSpeedVariable.maxValue);
                    }
                    enemyThinker.speed = enemyAI.moveForceSpeedVariable.minValue;//Mathf.Clamp(moveForceSpeedVariable.maxValue * difficultyMultiplier.value, moveForceSpeedVariable.minValue, moveForceSpeedVariable.maxValue);

                    //new new
                    //newObjectPools[newEnemyIndex].TryGetNextObject(spawnPoints[newSpawnPointIndex].transform.position, spawnPoints[newSpawnPointIndex].transform.rotation);

                    Debug.Log("Spawning Boss : " + _enemyWave.name + "_" + newEnemyIndex);
                    Debug.Log("newSpawnPointIndex : " + newSpawnPointIndex);
                    Debug.Log("newEnemyIndex : " + newEnemyIndex);
                    enemyCount++;
                }
                else
                {
                    int newSpawnPointIndex = Random.Range(0, spawnPoints.Length); //Random.seed 

                    int newEnemyIndex = Random.Range(1, enemyList[waves.IndexOf(_enemyWave)].Length);

                    //Debug.Log("enemyList.Count  : " + enemyList.Count);
                    GameObject enemyGO;
                    levelBasedObjectPool[waves.IndexOf(_enemyWave)][newEnemyIndex].TryGetNextObject(spawnPoints[newSpawnPointIndex].transform.position, spawnPoints[newSpawnPointIndex].transform.rotation, out enemyGO);

                    var enemyThinker = enemyGO.GetComponent<CharacterThinker>();
                    var enemyAI = enemyGO.GetComponent<CharacterBasicAI>();

                    if(currentLevel.value <= 20)
                    {
                        enemyThinker.attackPow = 1;//Random.Range((int)enemyPow.minValue,(int)enemyPow.maxValue);
                        enemyThinker.health.currentHP.Value = Mathf.Clamp((currentLevel.value / 3) + 1, 1, 200);//Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                        enemyThinker.health.maxHP.Value = Mathf.Clamp((currentLevel.value / 3) + 1, 1, 200);// Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);

                    }
                    else
                    {
                        enemyThinker.attackPow = 1;//Random.Range((int)enemyPow.minValue,(int)enemyPow.maxValue);
                        enemyThinker.health.currentHP.Value = Mathf.Clamp((currentLevel.value / 2) + 1, 1, 200);//Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                        enemyThinker.health.maxHP.Value = Mathf.Clamp((currentLevel.value / 2) + 1, 1, 200);// Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);

                    }
                                                                                                       //enemyThinker.GetCo

                    if(currentLevel.value <=20)
                    {
                        enemyAI.stopAttack = true;
                    }
                    
                    enemyThinker.coinsToDrop = 1;


                    enemyAI.attackDelay = 2;//Mathf.Clamp(timeBetweenShots.minValue / difficultyMultiplier.value, timeBetweenShots.minValue, timeBetweenShots.maxValue);
                    enemyAI.attackSpeed = enemyAI.chargeTimePerDistance.maxValue;//Mathf.Clamp(chargeTimePerDistance.minValue / difficultyMultiplier.value, chargeTimePerDistance.minValue, chargeTimePerDistance.maxValue);
                    if (enemyAI.agent.isActiveAndEnabled)
                    {
                        enemyAI.agent.speed = Random.Range(enemyAI.moveNavigationSpeedVariable.minValue, enemyAI.moveNavigationSpeedVariable.maxValue);// enemyAI.moveNavigationSpeedVariable.maxValue;//Mathf.Clamp(moveNavigationSpeedVariable.maxValue * difficultyMultiplier.value, moveNavigationSpeedVariable.minValue, moveNavigationSpeedVariable.maxValue);
                    }
                    enemyThinker.speed = Random.Range(enemyAI.moveForceSpeedVariable.minValue, enemyAI.moveForceSpeedVariable.maxValue); //enemyAI.moveForceSpeedVariable.maxValue;//Mathf.Clamp(moveForceSpeedVariable.maxValue * difficultyMultiplier.value, moveForceSpeedVariable.minValue, moveForceSpeedVariable.maxValue);

                    //new new
                    //newObjectPools[newEnemyIndex].TryGetNextObject(spawnPoints[newSpawnPointIndex].transform.position, spawnPoints[newSpawnPointIndex].transform.rotation);

                    Debug.Log("Spawning Minions : " + _enemyWave.name + "_" + newEnemyIndex);
                    Debug.Log("newSpawnPointIndex : " + newSpawnPointIndex);
                    Debug.Log("newEnemyIndex : " + newEnemyIndex);
                    enemyCount++;
                }
                
            }
            else if(roundType == RoundType.Bonus)
            {
                Random.InitState(currentLevel.value + waves.IndexOf(_enemyWave) + enemyCount);
                // spawn enemy
                int newSpawnPointIndex = Random.Range(0, spawnPoints.Length); //Random.seed 

                int newEnemyIndex = Random.Range(0, enemyList[waves.IndexOf(_enemyWave)].Length);

                //Debug.Log("enemyList.Count  : " + enemyList.Count);
                GameObject enemyGO;
                levelBasedObjectPool[waves.IndexOf(_enemyWave)][newEnemyIndex].TryGetNextObject(spawnPoints[newSpawnPointIndex].transform.position, spawnPoints[newSpawnPointIndex].transform.rotation, out enemyGO);

                var enemyThinker = enemyGO.GetComponent<CharacterThinker>();
                var enemyAI = enemyGO.GetComponent<CharacterBasicAI>();

                enemyThinker.attackPow = 1;//Random.Range((int)enemyPow.minValue,(int)enemyPow.maxValue);
                enemyThinker.health.currentHP.Value = 1;//Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                enemyThinker.health.maxHP.Value = 1;// Random.Range((int)enemyHealth.minValue, (int)enemyHealth.maxValue);
                //enemyThinker.GetCo
                enemyAI.stopAttack = true;
                enemyThinker.coinsToDrop = 10;

                enemyAI.attackDelay = enemyAI.timeBetweenShots.maxValue;//Mathf.Clamp(timeBetweenShots.minValue / difficultyMultiplier.value, timeBetweenShots.minValue, timeBetweenShots.maxValue);
                enemyAI.attackSpeed = enemyAI.chargeTimePerDistance.maxValue;//Mathf.Clamp(chargeTimePerDistance.minValue / difficultyMultiplier.value, chargeTimePerDistance.minValue, chargeTimePerDistance.maxValue);
                if (enemyAI.agent.isActiveAndEnabled)
                {
                    enemyAI.agent.speed = enemyAI.moveNavigationSpeedVariable.maxValue;//Mathf.Clamp(moveNavigationSpeedVariable.maxValue * difficultyMultiplier.value, moveNavigationSpeedVariable.minValue, moveNavigationSpeedVariable.maxValue);
                }
                enemyThinker.speed = enemyAI.moveForceSpeedVariable.maxValue;//Mathf.Clamp(moveForceSpeedVariable.maxValue * difficultyMultiplier.value, moveForceSpeedVariable.minValue, moveForceSpeedVariable.maxValue);

                Debug.Log("Spawning Enemy : " + _enemyWave.name + "_" + newEnemyIndex);
                Debug.Log("newSpawnPointIndex : " + newSpawnPointIndex);
                Debug.Log("newEnemyIndex : " + newEnemyIndex);
                enemyCount++;
            }
            ////new new
            ////newObjectPools[newEnemyIndex].TryGetNextObject(spawnPoints[newSpawnPointIndex].transform.position, spawnPoints[newSpawnPointIndex].transform.rotation);

            //Debug.Log("Spawning Enemy : "+ _enemyWave.name+"_"+ newEnemyIndex);
            //Debug.Log("newSpawnPointIndex : " + newSpawnPointIndex);
            //Debug.Log("newEnemyIndex : " + newEnemyIndex);
            //enemyCount++;
        }
    }

}
