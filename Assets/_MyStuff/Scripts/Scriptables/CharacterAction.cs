using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames {
    public abstract class CharacterAction : ScriptableObject
    {
        public virtual void OnInitialize(CharacterThinker character)
        {

        }
        public abstract void OnUpdate(CharacterThinker character);
        public abstract void OnFixedUpdate(CharacterThinker character);

    }


}
