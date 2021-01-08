using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using garagekitgames;
public class CharacterNavMeshTarget : MonoBehaviour {

    public NavMeshAgent agent;
    public CharacterThinker character;
	// Use this for initialization
	void Start () {

        agent = transform.GetComponent<NavMeshAgent>();
        character = transform.GetComponent<CharacterThinker>();
        agent.updatePosition = false;
        agent.updateRotation = false;

    }


	
	// Update is called once per frame
	void Update () {
        
        agent.SetDestination(character.target);
		
	}
}
