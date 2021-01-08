using UnityEngine;
using UnityEngine.UI;
using SO;
using UnityEngine.Events;
//using DoozyUI;

namespace garagekitgames
{
    public class PauseButton : MonoBehaviour
    {
        public bool pause = false;
        public bool canPause = true;

        public UnityEvent onPause;
        public UnityEvent onUnPause;

        //public UIButton thisButton;

        void Start()
        {
            GetComponent<Button>().onClick.AddListener(TogglePause);
            //thisButton = this.GetComponent<UIButton>();
        }

        private void Update()
        {
            if(Time.timeScale == 1)
            {
                //thisButton.EnableButton();
                canPause = true;
            }
            else if(Time.timeScale == 0)
            {
                //thisButton.EnableButton();
                canPause = true;
            }
            else
            {
                //thisButton.DisableButton();
                canPause = false;
            }

            //if
        }

        public void TogglePause()
        {
            
            if(!pause)
            {
                pause = true;
                onPause.Invoke(); 

            }
            else if(pause)
            {
                pause = false;
                onUnPause.Invoke();

            }
            //Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        }
        public void Pause()
        {
            if(Time.timeScale == 1)
            {
                Time.timeScale = 0.0f;
            }
            
        }

        public void UnPause()
        {
            if(Time.timeScale == 0)
            {
                Time.timeScale = 1f;
            }
            
        }
    }
}
