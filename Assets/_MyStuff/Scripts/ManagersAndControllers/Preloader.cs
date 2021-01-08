using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace garagekitgames
{
    public class Preloader : MonoBehaviour
    {

        private CanvasGroup fadeGroup;
        private float loadTime;
        private float minimumLogoTime = 3f;
        public Slider slider;
        public string levelToLoad = "MainMenu";
        // Use this for initialization
        void Start()
        {
            fadeGroup = FindObjectOfType<CanvasGroup>();
            fadeGroup.alpha = 1;
            //Preload the game, either from server or local 

            if(Time.time < minimumLogoTime)
            {
                loadTime = minimumLogoTime;
            }
            else
            {
                loadTime = Time.time;
            }

            Invoke("LoadLevel", 1);
        }

        // Update is called once per frame
        void Update()
        {

            //Fade In 
            if(Time.time < minimumLogoTime)
            {
                fadeGroup.alpha = 1 - Time.time;
            }

            //Fade out 
            if (Time.time > minimumLogoTime && loadTime != 0)
            {
                fadeGroup.alpha = Time.time - minimumLogoTime;
                if(fadeGroup.alpha >= 1)
                {
                    //LoadLevel();
                    //Application.LoadLevel("MainMenu");
                }
            }

        }

        public void LoadLevel()
        {

            StartCoroutine(LoadAsynchronously(levelToLoad));

        }

        IEnumerator LoadAsynchronously(string sceneIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            while (operation.isDone == false)
            {

                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                slider.value = progress;
                Debug.Log("Time : " + Time.timeSinceLevelLoad);
                Debug.Log(operation.progress);
                yield return null;
            }
        }
    }
}

