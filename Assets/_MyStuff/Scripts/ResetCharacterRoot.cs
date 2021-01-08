using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames
{
    public class ResetCharacterRoot : MonoBehaviour
    {

        public Vector3 startRootPosition;
        public BodyPart rootPart;
        public CharacterThinker character;

        private void Awake()
        {
            character = transform.GetComponent<CharacterThinker>();

        }

        // Use this for initialization
        void Start()
        {
            BodyPartMono bodyPartMono = character.bpHolder.bodyParts[rootPart];

            startRootPosition = bodyPartMono.BodyPartTransform.localPosition;

        }

        public void resetRoot()
        {
            BodyPartMono bodyPartMono = character.bpHolder.bodyParts[rootPart];

            bodyPartMono.BodyPartTransform.localPosition = startRootPosition;

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}

