using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using garagekitgames;
using SO;
public class PromptTutorial : MonoBehaviour
{
    public bool inputDetected;
    public float timer = 0;
    public GameObject promptGO;
    public IntVariable currLevel;
    // Start is called before the first frame update
    void Start()
    {
        inputDetected = false;
       // promptGO.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //if(GamePlayManager.Instance.game)

        if (Input.GetMouseButton(0))
        {
            inputDetected = true;
            timer = 0;
            promptGO.SetActive(false);
        }
        else if(GamePlayManager.Instance.gameEnded || GamePlayManager.Instance.gamePaused)
        {
            timer = 0;
            promptGO.SetActive(false);
        }
        else
        {
            timer += Time.deltaTime;

            if(timer > Mathf.Clamp(currLevel.value, 3, 10) && !GamePlayManager.Instance.gameEnded && !GamePlayManager.Instance.gamePaused)
            {
                inputDetected = false;
                promptGO.SetActive(true);
            }
            
        }

        
    }
}
