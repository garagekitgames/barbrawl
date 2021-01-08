using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using garagekitgames;
using UnityEngine.Events;


public class MainMenuBackButton : MonoBehaviour
{
    public UnityEvent OnEscapePressed;
   // public UnityEvent OnGameUnPaused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscapePressed.Invoke();
        }

    }
}
