using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace garagekitgames
{
    public class DestroyParent : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {

            if(transform.root != other.transform.root)
            {
                CharacterThinker cT = other.transform.root.GetComponent<CharacterThinker>();

                if (cT)
                {
                    if(cT.amIMainPlayer)
                    {
                        Destroy(this.gameObject);
                    }
                }
                   
                //Change to disable and use pooling 
            }
        }
    }
}

