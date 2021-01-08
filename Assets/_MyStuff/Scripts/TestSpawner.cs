using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;
public class TestSpawner : MonoBehaviour {

    public GameObject enemy;

    private EZObjectPool objectPool;

    public Transform spawnPoint;
    // Use this for initialization
    void Start () {
        objectPool = EZObjectPool.CreateObjectPool(enemy, enemy.name, 50, true, true, false);
    }
	
    public void SpawnGuy()
    {
        objectPool.TryGetNextObject(spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
