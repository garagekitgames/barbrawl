using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;


namespace garagekitgames
{
    public class CharacterCoinDrop : MonoBehaviour
    {
        public IntVariable currentCoins;

        public void Awake()
        {
            currentCoins.value = 0;
            //base.Awake();
        }

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

