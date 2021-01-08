using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/CharacterStat")]
    public class CharacterStats : ScriptableObject
    {

        public FloatVariable currentHP;
        public FloatVariable maxHP;

        
    }
}