using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using System.Linq;
using EZCameraShake;
using UnityEngine.SceneManagement;
//using DoozyUI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

namespace garagekitgames
{
    public class MainMenuManager : UnitySingleton<MainMenuManager>
    {
        public List<LevelInfo> levels;

        public List<LevelData> levelData = new List<LevelData>();

        public List<GameObject> levelCharacters = new List<GameObject>();

        public List<bool> levelLocks = new List<bool>();

        public LevelData currentLevel;

        public IntVariable selectedIndex;

        private int numObjects = 10;
        public float distanceBetweenCharacter = 2;
        public float radius = 10;
        public GameObject prefab;
        public bool lookingAtCamera = false;
        public CameraLookAt mainCam;

        public BodyPart lookAtPart;

        public GameObject playButton;
        public GameObject buyButton;

        public GameObject lockImage;

        public Sprite lockSprite;
        public Sprite unlockSprite;

        public IntVariable cashValue;

        //public UIButton playUIButton;
        //public UIButton buyUIButton;

        public UnityEvent updateCashUI;

        public UnityEvent notEnoughCash;

        public TextMeshProUGUI buttonCashValueUI;
        public Slider healthSlider;
        public Slider powerSlider;
        public Text characterName;

        public Text healthLevel;
        public Text powerLevel;

        public string statLevelPrefix = "Lvl ";
        // Use this for initialization
        void Awake()
        {
            //base.Awake();
            mainCam = GameObject.FindObjectOfType<CameraLookAt>();
            foreach (var level in levels)
            {
                //uncomment this 
               // levelData.Clear();
                levelData.Add(Resources.Load<LevelData>("LevelData/"+ level.name));
                levelLocks.Add(level.isUnlocked ? true : false);

            }
            numObjects = levels.Count;

            Vector3 center = transform.position;
            for (int i = 0; i < numObjects; i++)
            {

                // Quaternion rot = Quaternion.FromToRotation(Vector3.up, center - pos);
                radius = (distanceBetweenCharacter * numObjects) / (2 * Mathf.PI);
                float theta = (2 * Mathf.PI / numObjects) * i;
                Vector3 pos = RandomCircle2(center, radius, i, theta);
                var playerGameObject = Instantiate(levelData[i].Player, pos, Quaternion.identity);
                CharacterThinker character = playerGameObject.GetComponent<CharacterThinker>();
                character.target = new Vector3(0, 1, 0);
                //character.SetStopDoingShit(true);
                levelCharacters.Add(playerGameObject);
            }

            playButton.SetActive(true);
            //buyUIButton.
            buyButton.SetActive(false);
            lockImage.SetActive(false);

            updateCashUI.Invoke();
            //playButton.
            /* for (int i = 0; i < numObjects; ++i)
             {
                 float theta = (2 * Mathf.PI / numObjects) * i;
                 x = cos(theta);
                 z = sin(theta);
             }*/

        }


        private void OnDisable()
        {
            //CameraShaker.Instance.StopAllCoroutines();
        }

        private void Start()
        {

            //CameraShaker.Instance.Shake(CameraShakePresets.HandheldCamera).StartFadeIn(0.5f);
            //CameraShakeInstance c = new CameraShakeInstance(1f, 0.25f, 5f, 10f);
            //c.PositionInfluence = Vector3.zero;
            //c.RotationInfluence = new Vector3(1, 0.5f, 0.5f);
            CameraShaker.Instance.StartShake(1f, 0.25f, 5f);
            UpdateSelectedLevel(selectedIndex.value);
            AudioManager.instance.FadeInCaller("BGM2", 0.1f, 0.3f);
            AudioManager.instance.FadeOutCaller("BGM1", 0.1f);
            /*foreach (var characterGO in levelCharacters)
            {
                CharacterThinker character = characterGO.GetComponent<CharacterThinker>();
                character.SetStopDoingShit(true);
            }*/
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if(!lookingAtCamera)
            {
                foreach (var characterGO in levelCharacters)
                {
                    CharacterThinker character = characterGO.GetComponent<CharacterThinker>();
                    character.stopAttacking = true;
                    character.stopLooking = true;
                    character.inputDirection = Vector3.zero - character.GetComponent<CharacterThinker>().bpHolder.bodyPartsName["hip"].BodyPartTransform.position;
                }
                lookingAtCamera = true;
            }
            

        }

        public void ToggleLeft()
        {
            selectedIndex.value--;
            if(selectedIndex.value < 0)
            {
                selectedIndex.value = levelCharacters.Count - 1;
            }
            UpdateSelectedLevel(selectedIndex.value);
            //Call Update UI
            //Update the camera target
            //set the selected character // set currentLevel = selectedLevel
        }

        public void ToggleRight()
        {
            selectedIndex.value++;
            if (selectedIndex.value == levelCharacters.Count)
            {
                selectedIndex.value = 0;
            }
            UpdateSelectedLevel(selectedIndex.value);
            //Call Update UI
            //Update the camera target
            //set the selected character // set currentLevel = selectedLevel
        }

        public void UpdateSelectedLevel(int index)
        {
            AudioManager.instance.Play("NextButton");
            var cT = new CharacterThinker();
            Debug.Log("Level Updated Clicked");
            //currentLevel = levelData[index];
            currentLevel.Enemies = levelData[index].Enemies;
            currentLevel.Player = levelData[index].Player;
            currentLevel.spawnPoint = levelData[index].spawnPoint;
            currentLevel.enemyGroups = levelData[index].enemyGroups;
            currentLevel.maxHealth = levels[index].maxHealth;
            currentLevel.currentHealth = levels[index].currentHealth;
            currentLevel.maxPow = levels[index].maxPow;
            currentLevel.currentPow = levels[index].currentPow;
            currentLevel.charName = levels[index].charName;
            mainCam.target = levelCharacters[index].transform.GetComponent<CharacterThinker>().bpHolder.bodyParts[lookAtPart].BodyPartTransform;
            buttonCashValueUI.text = levelData[index].levelPrice.ToString();
            healthSlider.value = levels[index].currentHealth;
            healthSlider.minValue = 0;
            healthSlider.maxValue = levels[index].maxHealth;
            powerSlider.value = levels[index].currentPow;
            powerSlider.minValue = 0;
            powerSlider.maxValue = levels[index].maxPow;
            characterName.text = levels[index].charName;

            healthLevel.text = statLevelPrefix + levels[index].currentHealth;
            powerLevel.text = statLevelPrefix + levels[index].currentPow;

            GameManager.Instance.currentLevelInfo = levels[index];
            if (levels[index].isUnlocked)
            {
                playButton.SetActive(true);
                buyButton.SetActive(false);
                lockImage.SetActive(false);
            }
            else
            {
                playButton.SetActive(false);
                buyButton.SetActive(true);
                lockImage.SetActive(true);
            }

        }

        public void OnPlayClick()
        {
            //Debug.Log("Play Clicked");
            //Change this to load scene from level info
            AudioManager.instance.Play("PlayButton");
            //SceneManager.LoadScene(levels[selectedIndex.value].levelName);
            //SceneManager.LoadScene(levelData[selectedIndex.value].levelName);
            SceneManager.LoadScene("Level21");
            //GameManager.Instance.mapSelect(levelData[selectedIndex.value].levelName);
            AudioManager.instance.FadeOutCaller("BGM2", 0.1f);
            //Application.LoadLevel("Level1");
        }

        public void AttemptBuy()
        {
            if(cashValue.value >= levelData[selectedIndex.value].levelPrice)
            {
                Debug.Log("Cash Value : " + cashValue.value);
                Debug.Log("Cash Value : " + cashValue.value);
                cashValue.value = cashValue.value - levelData[selectedIndex.value].levelPrice;
                levels[selectedIndex.value].isUnlocked = true;
                updateCashUI.Invoke();
                
                PersistableSO.Instance.Save();
                AudioManager.instance.Play("ShopButton");
                UpdateSelectedLevel(selectedIndex.value);

            }
            else
            {
                //Display Buy Coins Panel
                notEnoughCash.Invoke();

            }
            
        }

        public void AttemptHealthBuy()
        {
            if (cashValue.value >= 100)
            {
                //levels[selectedIndex.value].currentHealth++;
                //updateCashUI.Invoke();

                if ((levels[selectedIndex.value].currentHealth + 1 <= levels[selectedIndex.value].maxHealth))
                {
                    cashValue.value = cashValue.value - 100;

                    levels[selectedIndex.value].currentHealth++;
                    updateCashUI.Invoke();

                    PersistableSO.Instance.Save();
                    AudioManager.instance.Play("ShopButton");
                    UpdateSelectedLevel(selectedIndex.value);
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
                if((levels[selectedIndex.value].currentPow + 1 <= levels[selectedIndex.value].maxPow))
                {
                    cashValue.value = cashValue.value - 100;

                    levels[selectedIndex.value].currentPow++;
                    updateCashUI.Invoke();

                    PersistableSO.Instance.Save();
                    AudioManager.instance.Play("ShopButton");
                    UpdateSelectedLevel(selectedIndex.value);
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

        Vector3 RandomCircle(Vector3 center, float radius,int i)
        {
            float ang = Random.value * 360;
            Vector3 pos;
            pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y;
            pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad); 
            return pos;
        }


        Vector3 RandomCircle2(Vector3 center, float radius, int i, float theta)
        {
            //float ang = Random.value * 360;

            //float theta = (2 * Mathf.PI / numObjects) * i;
            Vector3 pos;
            pos.x = center.x + radius * Mathf.Cos(theta);
            pos.y = center.y;
            pos.z = center.z + radius * Mathf.Sin(theta);
            return pos;
        }
    }
}

