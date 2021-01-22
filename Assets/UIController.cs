using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using SO;

namespace garagekitgames
{
    public class UIController : MonoBehaviour
    {

        public UnityEvent updateCashUI;
        public UnityEvent gotEnoughCash;
        public UnityEvent notEnoughCash;

        public UnityEvent adAvailable;
        public UnityEvent adUnavailable;

        public UnityEvent continueAvailable;
        public UnityEvent continueUnavailable;

        public IntVariable cashValue;
        public IntVariable resurrectionCost;

        public bool highlightInfo;

        public Animator GameUIAnimator;

        public UnityEvent upgradeCashAvailable;
        public UnityEvent upgradeCashUnavailable;
        // Use this for initialization
        void Start()
        {
            GameUIAnimator = this.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

            
            if(GameUIAnimator != null)
            {
                GameUIAnimator.SetBool("HighLightInfo", highlightInfo);
            }

            if (cashValue.value >= 100)
            {
                upgradeCashAvailable.Invoke();
                //cashValue.value = cashValue.value - resurrectionCost.value;
                //updateCashUI.Invoke();
                // PersistableSO.Instance.Save();

                //gotEnoughCash.Invoke();
            }
            else
            {
                upgradeCashUnavailable.Invoke();
                //Display other cash buy options
                //notEnoughCash.Invoke();
            }

        }

        public void HighlightInfo(bool value)
        {
            highlightInfo = value;
        }
        public void OnGameEnd()
        {

            if(UnityAdsManager.Instance.isRewardedAdReady || cashValue.value >= resurrectionCost.value)
            {
                //continueAvailable.Invoke();
                continueUnavailable.Invoke();
                Debug.Log("ContinueAvailable");

            }
            else
            {
                continueUnavailable.Invoke();
                Debug.Log("ContinueUnAvailable");
            }


            if (UnityAdsManager.Instance.isRewardedAdReady)
            {
                //cashValue.value = cashValue.value - resurrectionCost.value;
                //updateCashUI.Invoke();
                // PersistableSO.Instance.Save();

                adAvailable.Invoke();

            }
            else
            {
                //Display other cash buy options

                adUnavailable.Invoke();
            }




            //Use this when revive is based on coins 

            if(cashValue.value >= resurrectionCost.value)
            {
                //cashValue.value = cashValue.value - resurrectionCost.value;
                //updateCashUI.Invoke();
                // PersistableSO.Instance.Save();

                gotEnoughCash.Invoke();
            }
            else
            {
                //Display other cash buy options
                notEnoughCash.Invoke();
            }


            /*if (cashValue.value >= resurrectionCost.value)
            {
                //cashValue.value = cashValue.value - resurrectionCost.value;
                //updateCashUI.Invoke();
               // PersistableSO.Instance.Save();
                gotEnoughCash.Invoke();
            }
            else
            {
                //Display other cash buy options
                notEnoughCash.Invoke();
            }*/
        }
    }

}

