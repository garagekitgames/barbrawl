using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/EnemyGroup")]
    public class EnemyGroup : ScriptableObject
    {
        public string groupName;
        public List<GameObject> Enemies;
        public float maxDifficultyTime; 
    }
}

