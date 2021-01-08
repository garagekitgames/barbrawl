using UnityEngine;
using System.Collections;
using DamageEffect;

namespace DamageEffectDemo
{
    public class Knight : MonoBehaviour
    {       
        private DamageEffectScript damageEffectScript;

        void Start()
        {
            damageEffectScript = GetComponent<DamageEffectScript>();
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            GetComponent<AudioSource>().Play();
            damageEffectScript.Blink(0, 0.25f);
        }
    }
}
