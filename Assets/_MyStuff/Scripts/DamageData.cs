using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames
{
    [System.Serializable]
    public class DamageData
    {
        public Transform collisionTransform;
        public Rigidbody collisionRigidbody;
        public Collider collisionCollider;
        public ContactPoint[] collisionContacts;
        public Vector3 relativeVelocity;
        public float velocityMagnitude;
        public float impulseMagnitude;
        public int cashToDrop;
        public int damagePow;

        public DamageData(Transform collisionTransform, Rigidbody collisionRigidbody, Collider collisionCollider, ContactPoint[] collisionContacts, Vector3 relativeVelocity, float velocityMagnitude, float impulseMagnitude, int cashToDrop, int damagePow)
        {
            this.collisionTransform = collisionTransform;
            this.collisionRigidbody = collisionRigidbody;
            this.collisionCollider = collisionCollider;
            this.collisionContacts = collisionContacts;
            this.relativeVelocity = relativeVelocity;
            this.velocityMagnitude = velocityMagnitude;
            this.impulseMagnitude = impulseMagnitude;
            this.cashToDrop = cashToDrop;
            this.damagePow = damagePow;
        }

        public Transform CollisionTransform
        {
            get
            {
                return collisionTransform;
            }

            set
            {
                collisionTransform = value;
            }
        }

        public Rigidbody CollisionRigidbody
        {
            get
            {
                return collisionRigidbody;
            }

            set
            {
                collisionRigidbody = value;
            }
        }

        public Collider CollisionCollider
        {
            get
            {
                return collisionCollider;
            }

            set
            {
                collisionCollider = value;
            }
        }

        public ContactPoint[] CollisionContacts
        {
            get
            {
                return collisionContacts;
            }

            set
            {
                collisionContacts = value;
            }
        }

        public Vector3 RelativeVelocity
        {
            get
            {
                return relativeVelocity;
            }

            set
            {
                relativeVelocity = value;
            }
        }

        public float VelocityMagnitude
        {
            get
            {
                return velocityMagnitude;
            }

            set
            {
                velocityMagnitude = value;
            }
        }

        public float ImpulseMagnitude
        {
            get
            {
                return impulseMagnitude;
            }

            set
            {
                impulseMagnitude = value;
            }
        }

        public int CashToDrop
        {
            get
            {
                return cashToDrop;
            }

            set
            {
                cashToDrop = value;
            }
        }

        public int DamagePow
        {
            get
            {
                return damagePow;
            }

            set
            {
                damagePow = value;
            }
        }
    }
}

