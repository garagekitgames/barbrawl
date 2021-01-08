using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace garagekitgames
{
	[CreateAssetMenu(menuName = "GarageKitGames/CharacterData")]
	public class CharacterData : ScriptableObject
	{
		//public List<GameObject> Enemies;
		public GameObject inGamePlayer;
		public Vector3Variable spawnPoint;
		public GameObject menuPlayer;

		//public List<EnemyGroup> enemyGroups;
		//public bool isUnlocked;
		//public string levelName;
		//public int levelPrice;

	}
}
