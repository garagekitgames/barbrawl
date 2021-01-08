using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using SO;
using UnityEngine.SceneManagement;

namespace garagekitgames
{
    public class CoinController : UnitySingleton<CoinController>
    {
        public IntVariable currentCoins;

        public UnityEvent updateScoreUI;

        public void Awake()
        {
            currentCoins.value = 0;
            //base.Awake();
        }
        /*
        void OnEnable()
        {
            //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        void OnDisable()
        {
            //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Level Loaded");
            Debug.Log(scene.name);
            Debug.Log(mode);
            currentCoins.value = 0;
            
        }*/

        public void AddCoins(int value)
        {
            currentCoins.Add(value);
        }
        
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

