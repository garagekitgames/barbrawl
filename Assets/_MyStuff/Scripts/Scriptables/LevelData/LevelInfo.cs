using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace garagekitgames
{
    [CreateAssetMenu(menuName = "GarageKitGames/LevelInfo")]
    public class LevelInfo : ScriptableObject
    {
        //public List<GameObject> Enemies;
        //public GameObject Player;
        //public Vector3Variable spawnPoint;
        public bool isUnlocked;
        public string levelName;
        public int levelPrice;
        public int maxHealth;
        public int currentHealth;
        public int maxPow;
        public int currentPow;
        public string charName;


    }
}
