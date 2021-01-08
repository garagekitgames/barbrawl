using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames
{

    public class CharacterBodyPartHolder : MonoBehaviour
    {
        
        public Dictionary<BodyPart, BodyPartMono> bodyParts = new Dictionary<BodyPart, BodyPartMono>();
        public Dictionary<string, BodyPartMono> bodyPartsName = new Dictionary<string, BodyPartMono>();
        // public Dictionary<string, CharacterFaceDirection> faceDirections = new Dictionary<string, CharacterFaceDirection>();
        // public Dictionary<string, CharacterMaintainHeight> maintainHeights = new Dictionary<string, CharacterMaintainHeight>();
        //public BodyPart[] bps;

        public Dictionary<BodyPart, BodyPartMono> BodyParts
        {
            get
            {
                return bodyParts;
            }

            set
            {
                bodyParts = value;
            }
        }

        public Dictionary<string, BodyPartMono> BodyPartsName
        {
            get
            {
                return bodyPartsName;
            }

            set
            {
                bodyPartsName = value;
            }
        }

        public void ResetJoints(bool isRagdoll)
        {
            foreach (var value in bodyParts.Values)
            {
                value.ResetJoint(isRagdoll);
            }
        }

        // Use this for initialization
        void Start()
        {
            //int i = 0;

            //bps = new BodyPart[bodyParts.Count];
            //foreach (var bp in bodyParts)
            //{
            //    bps[i] = bp.Key;
            //    i++;
            //}
            


        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}

