using UnityEngine;
using System.Collections;

public class SlowDown : UnitySingletonPersistent<SlowDown>
{
    //the factor used to slow down time  
    [Range(1.0f, 10f)]
    public float slowFactor = 1.8f;
    //the new time scale  
    private float newTimeScale;
    private IEnumerator coroutine;

    public override void Awake()
    {
        base.Awake();
    }
        // Called when this script starts  
    void Start()
    {
        //calculate the new time scale  
        newTimeScale = Time.timeScale / slowFactor;
    }

    // Update is called once per frame  
    void Update()
    {
        /* Time.fixedDeltaTime = .01333f;
         Time.maximumDeltaTime = .15f;*/
        //when "Fire1" is pressed  

        if (Input.GetKeyDown(KeyCode.F1))
        {
            SwitchTime();
        }
    }
    public void slowDownTime(float slowFactorValue, float waitTime)
    {
        coroutine = TimeSlow(slowFactorValue, waitTime);
        StartCoroutine(coroutine);
    }
    private IEnumerator TimeSlow(float slowFactorValue, float waitTime)
    {
        SlowTime(slowFactorValue);
        yield return new WaitForSeconds(waitTime);
        ResetTime(slowFactorValue);
        //print("Coroutine ended: " + Time.time + " seconds");
    }

    public void SlowTime(float slowFactorValue)
    {

        if (Time.timeScale == 1.0f)
        {
            newTimeScale = Time.timeScale / slowFactorValue;
            //assign the 'newTimeScale' to the current 'timeScale'  
            Time.timeScale = newTimeScale;
            //proportionally reduce the 'fixedDeltaTime', so that the Rigidbody simulation can react correctly  
            Time.fixedDeltaTime = Time.fixedDeltaTime / slowFactorValue;
            //The maximum amount of time of a single frame  
            Time.maximumDeltaTime = Time.maximumDeltaTime / slowFactorValue;
        }

    }

    public void ResetTime(float slowFactorValue)
    {
        if (Time.timeScale != 1.0f) //the game is running in slow motion  
        {
            //reset the values  
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.fixedDeltaTime * slowFactorValue;
            Time.maximumDeltaTime = Time.maximumDeltaTime * slowFactorValue;
        }
    }

    public void SwitchTime()
    {
        
            //if the game is running normally  
            if (Time.timeScale == 1.0f)
            {
                //assign the 'newTimeScale' to the current 'timeScale'  
                Time.timeScale = newTimeScale;
                //proportionally reduce the 'fixedDeltaTime', so that the Rigidbody simulation can react correctly  
                Time.fixedDeltaTime = Time.fixedDeltaTime / slowFactor;
                //The maximum amount of time of a single frame  
                Time.maximumDeltaTime = Time.maximumDeltaTime / slowFactor;
            }
            else if (Time.timeScale == newTimeScale) //the game is running in slow motion  
            {
                //reset the values  
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = Time.fixedDeltaTime * slowFactor;
                Time.maximumDeltaTime = Time.maximumDeltaTime * slowFactor;
            }
        
    }
}