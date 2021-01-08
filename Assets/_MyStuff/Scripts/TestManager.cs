using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SO;
using MyCompany.GameFramework.Pooling;
using EZObjectPools;
using System.Linq;
using UnityEngine.Events;

namespace garagekitgames
{
    public class TestManager : UnitySingleton<TestManager>
    {
        public LevelData levelData;
        // public static TestManager instance;
        public CharacterRuntimeSet RuntimeSet;

        public GameObject[] enemy;

        public EnemyGroup[] enemyGroups;

       /* public float minSpawnTime;
        public float maxSpawnTime;*/

        [MinMaxRange(0.2f, 2)]
        public RangedFloat spawnTimeRange;

        // The enemy prefab to be spawned.
        public float spawnTime = 5f;            // How long between each spawn.
        public GameObject[] spawnPoints;         // An array of the spawn points this enemy can spawn from.

        public GameObject enemySpawnRoot;
        //public CharacterThinker mainplayer;

        //public CharacterHealth mainPlayerHealth;

        public TransformVariable playerTransform;

        public float spawnCounter;


       /* [MinMaxRange(0.2f, 2)]
        public RangedFloat enemyCountRange;*/

        public int minimumEnemies = 5;

        public int maxEnemies = 8;

        public int currentEnemies;

        public List<ObjectPool> pools;

        public ObjectPool[] poolArray;

        public int poolsize;

        private List<EZObjectPool> objectPools;

        private List<EZObjectPool> newObjectPools;
        private List<EZObjectPool> previousObjectPools;

        private List<List<EZObjectPool>> levelBasedObjectPool;

        public List<GameObject[]> enemyList;

        public FloatVariable difficultyMultiplier;

        public FloatVariable spawnTimVariable;

        public FloatVariable enemyCountVariable;

        public bool stopDoingShit;

        public string levelName;

        public IntVariable currentLevel;

        public int previousLevel;

        public EnemyGroup currentEnemyGroup;

        
       /* public IntVariable cashCollected;
        public IntVariable cashCollectedThisRound;*/

        void Awake()
        {
            //if (instance != null)
            //{
            //    Destroy(gameObject);
            //}
            //else
            //{
            //    instance = this;
            //    DontDestroyOnLoad(gameObject);
            //}


            //mainplayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterThinker>();

            
           
            enemySpawnRoot = GameObject.FindGameObjectWithTag("EnemySpawnRoot");

            /*enemy = levelData.Enemies.ToArray();
            objectPools = new List<EZObjectPool>();
            foreach (var item in enemy)
            {
                EZObjectPool objectPool = EZObjectPool.CreateObjectPool(item, item.name , poolsize, true, true, false);
                //ObjectPool pool = new ObjectPool(item, poolsize, true);
                objectPools.Add(objectPool);
            }*/

            currentEnemies = minimumEnemies;
            spawnTime = spawnTimeRange.maxValue;
            //poolArray = pools.ToArray();

            enemyGroups = levelData.enemyGroups.ToArray();

            newObjectPools = new List<EZObjectPool>();
            previousObjectPools = new List<EZObjectPool>();
            levelBasedObjectPool = new List<List<EZObjectPool>>();
            enemyList = new List<GameObject[]>();
            foreach (var item in enemyGroups)
            {
                var tempEnemyArray = item.Enemies.ToArray();
                
               // var newObjectPools = new List<EZObjectPool>();
               // foreach (var item2 in newEnemy)
               // {
               //     EZObjectPool objectPool = EZObjectPool.CreateObjectPool(item2, item2.name, poolsize, true, true, false);
                    
              //      newObjectPools.Add(objectPool);
              //  }
                enemyList.Add(tempEnemyArray);
              //  levelBasedObjectPool.Add(newObjectPools);

            }


            var newEnemy = enemyGroups[Mathf.Clamp(currentLevel.value, 0, enemyList.Count - 1)].Enemies.ToArray();
            currentEnemyGroup = enemyGroups[Mathf.Clamp(currentLevel.value, 0, enemyList.Count - 1)];
            //var newObjectPools = new List<EZObjectPool>();
            foreach (var item2 in newEnemy)
            {
                EZObjectPool objectPool = EZObjectPool.CreateObjectPool(item2, item2.name, poolsize, true, true, false);
                //ObjectPool pool = new ObjectPool(item, poolsize, true);
                newObjectPools.Add(objectPool);
            }
            //

            

        }

        public EnemyGroup GetCurrentEnemyGroup()
        {
            return currentEnemyGroup;
        }

        public float GetCurrentEnemyGroupMaxDifficultyTime()
        {
            return currentEnemyGroup.maxDifficultyTime;
        }


        public List<EZObjectPool> SetupEnemyPool(int level)
        {
            currentEnemyGroup = enemyGroups[level];
            var newEnemy = enemyGroups[level].Enemies.ToArray();

            
            var newObjectPools = new List<EZObjectPool>();
            foreach (var item2 in newEnemy)
            {
                EZObjectPool objectPool = EZObjectPool.CreateObjectPool(item2, item2.name, poolsize, true, true, false);
                //ObjectPool pool = new ObjectPool(item, poolsize, true);
                newObjectPools.Add(objectPool);
            }

            return newObjectPools;
        }

        public void OnLevelUp()
        {
            //EffectsController.Instance.slowDownTime(4, 1, 2);
            foreach (var item in RuntimeSet.Items)
            {
                var enemyBasicAI = item.GetComponent<CharacterBasicAI>();
                enemyBasicAI.OnLevelUp();
            }
            IEnumerator coroutine2 = GiveLevelUpReward(1.5f);
            StartCoroutine(coroutine2);
            IEnumerator coroutine = StartStopDoingShit(2.5f);
            StartCoroutine(coroutine);

            //GiveLevelUpReward
            

        }
        public void OnLevelChange()
        {
            Debug.Log("Current Level  : " + currentLevel.value);
            Debug.Log("Before newObjectPools Size  : " + newObjectPools.Count);

            int level = Mathf.Clamp(currentLevel.value, 0, enemyList.Count - 1);

            foreach (var item in RuntimeSet.Items)
            {
                var enemyBasicAI = item.GetComponent<CharacterBasicAI>();
                enemyBasicAI.OnLevelUp();
            }
            //currentEnemyGroup = enemyList[level]

            if (currentLevel.value > enemyList.Count - 1)
            {
                return;
            }

            if (previousObjectPools.Count > 0)
            {
                foreach (var item in previousObjectPools)
                {
                    item.ClearPool();
                    //item.DeletePool(true);
                }
                //newObjectPools[Mathf.Clamp(currentLevel.value - 2, 0, enemyList.Count - 2)].ClearPool();
            }

            

            if(newObjectPools.Count > 0)
            {
                foreach (var item in newObjectPools)
                {
                    //item.ClearPool();
                    item.DeletePool(false);
                }
                //newObjectPools[Mathf.Clamp(currentLevel.value - 2, 0, enemyList.Count - 2)].ClearPool();
            }

            previousObjectPools = newObjectPools;

           /* Debug.Log("After newObjectPools Size  : " + newObjectPools.Count);
            if (previousLevel >= 1)
            {
                Debug.Log("Trying to Delete  : " + Mathf.Clamp(previousLevel - 1, 0, enemyList.Count - 2));
                newObjectPools[Mathf.Clamp(previousLevel - 1, 0, enemyList.Count - 2)].DeletePool(true);
            }*/
            
            //newObjectPools[Mathf.Clamp(currentLevel.value - 2, 0, enemyList.Count - 2)].DeletePool(true);
            //newObjectPools.Clear();
            newObjectPools = SetupEnemyPool(Mathf.Clamp(currentLevel.value, 0, enemyList.Count - 1));
        }
        void Spawn()
        {
            //OLD 
            // Find a random index between zero and one less than the number of spawn points.
            /*int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            int enemyIndex = Random.Range(0, enemy.Length);

            objectPools[enemyIndex].TryGetNextObject(spawnPoints[spawnPointIndex].transform.position, spawnPoints[spawnPointIndex].transform.rotation);
            
            spawnCounter = 0;*/


            if(currentLevel.value > previousLevel)
            {
                OnLevelChange();
            }

            //NEW
            int level = Mathf.Clamp(currentLevel.value, 0, enemyList.Count - 1);

           // Debug.Log("Current Level  : " + level);
            int newSpawnPointIndex = Random.Range(0, spawnPoints.Length);

            int newEnemyIndex = Random.Range(0, enemyList[level].Length);

            //Debug.Log("enemyList.Count  : " + enemyList.Count);

            //levelBasedObjectPool[level][newEnemyIndex].TryGetNextObject(spawnPoints[newSpawnPointIndex].transform.position, spawnPoints[newSpawnPointIndex].transform.rotation);

            //new new
            newObjectPools[newEnemyIndex].TryGetNextObject(spawnPoints[newSpawnPointIndex].transform.position, spawnPoints[newSpawnPointIndex].transform.rotation);

            previousLevel = currentLevel.value;
        }

        // Use this for initialization
        void Start()
        {
           // enemySpawnRoot.transform.position = playerTransform.value.position;

            //if (spawnPoints == null)
            //{
            spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");

                //GameObject.FindGameObjectsWithTag("Respawn");
            //}

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

        public void KillAllEnemies()
        {
            KillAfter(0.1f);
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
            foreach (var enemy in RuntimeSet.Items)
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
            foreach (var enemy in RuntimeSet.Items)
            {
                enemy.SetStopDoingShit(value);
                var AI = enemy.GetComponent<CharacterBasicAI>();
                
                AI.SetStopDoingShit(value);
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (stopDoingShit)
            {
                return;
            }

            spawnTime = Mathf.Clamp(spawnTimeRange.minValue / difficultyMultiplier.value, spawnTimeRange.minValue, spawnTimeRange.maxValue);
            spawnTimVariable.value = spawnTime;
            currentEnemies = Mathf.RoundToInt(Mathf.Clamp(maxEnemies * difficultyMultiplier.value, minimumEnemies, maxEnemies));
            enemyCountVariable.value = currentEnemies;
            spawnCounter += Time.deltaTime;
            //enemySpawnRoot.transform.position = playerTransform.value.position;
            if (RuntimeSet.Items.Count > currentEnemies)
            {
                CancelInvoke("Spawn");

            } 
            else if (RuntimeSet.Items.Count <= currentEnemies)
            {
                
                if (spawnCounter >= spawnTime)
                {
                    Spawn();
                }
            }
        }
    }
}

