using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSimpleMove : MonoBehaviour {

    Rigidbody rb;
    Vector3 inputDirection;
    public float speed = 200f;

    public Rigidbody Rb
    {
        get
        {
            return rb;
        }

        set
        {
            rb = value;
        }
    }

    public Vector3 InputDirection
    {
        get
        {
            return inputDirection;
        }

        set
        {
            inputDirection = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        inputDirection = Vector3.zero;

    }

    private void FixedUpdate()
    {
        Rb.AddForce(InputDirection * speed);
    }
}
