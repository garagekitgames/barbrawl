using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace garagekitgames
{
    public class MenuScene : MonoBehaviour
    {

        private CanvasGroup fadeGroup;
        private float fadeInSpeed = 0.3f;

        public Transform characterPanel;
        public Transform weaponPanel;
        // Use this for initialization
        void Start()
        {
            fadeGroup = FindObjectOfType<CanvasGroup>();
           // fadeGroup.alpha = 1;
        }

        // Update is called once per frame
        void Update()
        {
           // fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;
        }


        private void InitShop()
        {

        }

        public void OnPlayClick()
        {
            Debug.Log("Play Clicked");
            //SceneManager.LoadScene("Level1");
            Application.LoadLevel("Level1");
        }

        public void OnShopClick()
        {
            Debug.Log("Shop Clicked");
        }
    }
}

