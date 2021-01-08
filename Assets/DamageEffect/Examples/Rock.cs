using UnityEngine;
using System.Collections;

namespace DamageEffectDemo
{
    public class Rock : MonoBehaviour
    {
        private Vector3 _position;
        // Use this for initialization
        void Start()
        {
            _position = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.y < -5)
            {
                transform.position = _position;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
}
