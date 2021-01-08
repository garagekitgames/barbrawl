using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SO;

namespace garagekitgames
{
    public class DiableAfterXSeconds : MonoBehaviour
    {

        //public float secs = 14f;
        private IEnumerator coroutine;

        public UnityEvent afterDeathEvent;

        public bool destroy = false;

        void Start()
        {
            //if (gameObject.activeInHierarchy)
            //    gameObject.SetActive(false);

            //StartCoroutine(LateCall());
        }

        public void DisableAfter(float sec)
        {
            coroutine = LateCall(false, sec);
            StartCoroutine(coroutine);
        }

        //public void 
        IEnumerator LateCall(bool value, float sec)
        {

            yield return new WaitForSeconds(sec);
            afterDeathEvent.Invoke();

            if(destroy)
            {
                Destroy(this.gameObject);
                

            }
            else
            {
                gameObject.SetActive(value);
            }
            
            //Do Function here...
        }
    }



}
