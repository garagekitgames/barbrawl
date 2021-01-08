using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SO;

namespace garagekitgames
{
    public abstract class CharacterHealthManager : MonoBehaviour
    {

        public FloatReference currentHP;

        public FloatReference maxHP;

        public UnityEvent DamageEvent;
        public UnityEvent DeathEvent;

        public bool ResetHP = true;

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