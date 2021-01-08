using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

namespace garagekitgames {
    public class CharacterHitParticleEffect : MonoBehaviour
    {

        public GameObject prefab;
        public CharacterThinker character;
        public BodyPart bodyPartToSpawnBlood;

        public Transform spawnPoint;
        public bool dontSimulate = false;
        // Use this for initialization
        EZObjectPool objectPool = new EZObjectPool();
        void Start()
        {
            character = transform.root.GetComponent<CharacterThinker>();
            objectPool = EZObjectPool.CreateObjectPool(prefab, prefab.name, 10, true, true, true);
        }

        public void spawnParticle()
        {
            if(!dontSimulate)
            {
                BodyPartMono bodyPartMono2 = character.bpHolder.bodyParts[bodyPartToSpawnBlood];

                Vector3 positionToSpawn = bodyPartMono2.transform.position;
                GameObject bloodPool;
                //Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
               objectPool.TryGetNextObject(bodyPartMono2.BodyPartTransform.position, bodyPartMono2.BodyPartTransform.rotation, out bloodPool);
                //bloodPool

                //bloodPool.transform.parent = bodyPartMono2.BodyPartTransform; // bodyPartMono2.BodyPartTransform;
            }
            character.bleedNow = false;


        }
        // Update is called once per frame
        void Update()
        {
            if(character.bleedNow)
            {
                spawnParticle();
            }
        }
    }
}

