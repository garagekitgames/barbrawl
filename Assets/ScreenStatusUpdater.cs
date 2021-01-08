using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.Events;

namespace garagekitgames
{
    public class ScreenStatusUpdater : MonoBehaviour
    {
        public StringVariable statusString;

        public StringVariable deathStatusString;

        public UnityEvent OnUpdateGoodStatus;
        public UnityEvent OnUpdateBadStatus;

        string[] goodStatus = {
            "WOW !",
            "GREAT !",
            "POW !",
            "GET'EM !",
            "KAPOW !",
            "BOOM !",
            "AMAZING !",
            "OMGGGG !",
            "COOL !",
            "AWESOME !"
        };

        string[] badStatus = {
            "Bad !",
            "You Missed !",
            "Weak !",
            "Focus !",
            "What ?!",
            "Shame !",
            "Quit Now !",
            "Why ?!",
            ":( !"
        };
        // Use this for initialization
        void Start()
        {

        }

        public void OnPlayerHit()
        {
            int randomTipKey = UnityEngine.Random.Range(0, goodStatus.Length);
            statusString.value = goodStatus[randomTipKey];
            //statusString.value = "Hit!";
            OnUpdateGoodStatus.Invoke();
        }

        public void OnPlayerMiss()
        {
            int randomTipKey = UnityEngine.Random.Range(0, badStatus.Length);
            statusString.value = badStatus[randomTipKey];
            OnUpdateBadStatus.Invoke();
        }

        public void OnPlayerHurt()
        {
            statusString.value = "Ouch!";
            OnUpdateBadStatus.Invoke();
        }

        public void OnPlayerDeath()
        {
            int randomTipKey = UnityEngine.Random.Range(0, badStatus.Length);

            statusString.value = badStatus[randomTipKey];
            OnUpdateBadStatus.Invoke();
        }

        // Update is called once per frame
        void Update()
        {


        }
    }
}

