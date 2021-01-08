using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace garagekitgames
{
    public class GameManager : UnitySingletonPersistent<GameManager>
    {
        public IntVariable noOfTimesPlayed;
        public LevelInfo currentLevelInfo;
        public override void Awake()
        {
            base.Awake();


            
        }
        // Use this for initialization
        void Start()
        {
            //
            PersistableSO.Instance.LoadVersion();
            noOfTimesPlayed.value = noOfTimesPlayed.value + 1;
            Debug.Log("Game Launch : " + noOfTimesPlayed.value);
            if (noOfTimesPlayed.value > 0)
            {
                PersistableSO.Instance.Load();
                Debug.Log("Game Launch Times : " + noOfTimesPlayed.value);
                PersistableSO.Instance.SaveVersion();
            }
            else
            {
                

                Debug.Log("Game Launch First Time : " + noOfTimesPlayed.value);
                //PersistableSO.Instance.Save();
                PersistableSO.Instance.SaveVersion();
            }

            /* Mandatory - set your AppsFlyer’s Developer key. */
            //AppsFlyer.setAppsFlyerKey("zcKrZYJWnrWWctCxcLNnyT");
            /* For detailed logging */
            /* AppsFlyer.setIsDebug (true); */
#if UNITY_IOS
  /* Mandatory - set your apple app ID
   NOTE: You should enter the number only and not the "ID" prefix */
  //AppsFlyer.setAppID ("YOUR_APP_ID_HERE");
  //AppsFlyer.trackAppLaunch ();
#elif UNITY_ANDROID
            ///* Mandatory - set your Android package name */
            //AppsFlyer.setAppID("com.garagekitgames.brutalbrawl");
            ///* For getting the conversion data in Android, you need to add the "AppsFlyerTrackerCallbacks" listener.*/
            //AppsFlyer.init("zcKrZYJWnrWWctCxcLNnyT", "AppsFlyerTrackerCallbacks");
#endif

        }

        private void OnEnable()
        {
            //PersistableSO.Instance.Load();
        }

        private void OnDisable()
        {
           // PersistableSO.Instance.Save();
        }

        // Update is called once per frame
        void Update()
        {

        }

        //public void purchaseEvent(string price, string value, string quantity)
        //{
        //    System.Collections.Generic.Dictionary<string, string> purchaseEvent = new
        //    System.Collections.Generic.Dictionary<string, string>();
        //    purchaseEvent.Add("af_currency", price);
        //    purchaseEvent.Add("af_revenue", value);
        //    purchaseEvent.Add("af_quantity", quantity);
        //    AppsFlyer.trackRichEvent("af_purchase", purchaseEvent);
        //}

        //public void mapSelect(string levelName)
        //{
        //    System.Collections.Generic.Dictionary<string, string> mapEvent = new
        //    System.Collections.Generic.Dictionary<string, string>();
        //    mapEvent.Add("af_mapchosen", levelName);
            
        //    AppsFlyer.trackRichEvent("af_map", mapEvent);
        //}


    }
}

