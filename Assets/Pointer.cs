using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var mousePosition = Input.mousePosition;
        //mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = mousePosition;//Vector2.Lerp(transform.position, mousePosition, moveSpeed);
        if (Input.GetMouseButton(0))
        {
            transform.localScale = Vector3.one / 1.5f;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
}
