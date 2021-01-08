using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace garagekitgames
{
	[CreateAssetMenu(menuName = "GarageKitGames/CharacterInfo")]
	public class CharacterInfo : ScriptableObject
	{
		//public List<GameObject> Enemies;
		//public GameObject Player;
		//public Vector3Variable spawnPoint;
		public string charName;
		public bool isUnlocked;
		public int charPrice;
		public int maxHealth;
		public int currentHealth;
		public int maxPow;
		public int currentPow;


	}
}
