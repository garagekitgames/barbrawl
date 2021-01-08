using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using garagekitgames;
using UnityEngine.Events;

namespace garagekitgames
{
    public class BackButton : MonoBehaviour
    {
        public UnityEvent OnGamePaused;
        public UnityEvent OnGameUnPaused;
        // Start is called before the first frame update
        void Start()
        {

        }



        // Update is called once per frame
        void Update()
        {
            if (GamePlayManager.Instance.gameEnded)
            {
                return;
            }
            if (!GamePlayManager.Instance.gamePaused)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    OnGamePaused.Invoke();
                }

            }
            else if (GamePlayManager.Instance.gamePaused)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    OnGameUnPaused.Invoke();
                }

            }
        }
    }

}
