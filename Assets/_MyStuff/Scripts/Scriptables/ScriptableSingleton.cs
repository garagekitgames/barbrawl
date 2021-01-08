using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

namespace garagekitgames
{
    public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!_instance)
                    _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();

#if UNITY_EDITOR
                if (!_instance)
                {
                    _instance = CreateFromSettings();


                   
                }


#endif
                //_instance.hideFlags = HideFlags.HideAndDontSave;
                return _instance;
            }
        }

        public static T CreateFromSettings()
        {
            //Assert.IsNotNull(settings);

            _instance = CreateInstance<T>();
            _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
            //StartUp();
            return _instance;
            /*_instance.players = new List<PlayerState>();
            foreach (var playerInfo in settings.players)
            {
                if (!playerInfo.Brain) continue;

                _instance.players.Add(new PlayerState { PlayerInfo = playerInfo });
            }*/
        }

        public virtual void StartUp()
        {

        }

    }
}

