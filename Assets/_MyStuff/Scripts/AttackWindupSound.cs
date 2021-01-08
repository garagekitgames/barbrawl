using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace garagekitgames
{
    public class AttackWindupSound : MonoBehaviour
    {
        public CharacterThinker character;
        // Use this for initialization
        void Start()
        {
            character = transform.root.GetComponent<CharacterThinker>();

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

