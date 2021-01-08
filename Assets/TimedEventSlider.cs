using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimedEventSlider : MonoBehaviour
{

    public Image timer;
    public float waitTime;
    public float currentTime;
    public UnityEvent OnReviveTimeOver;
    public UnityEvent OnReviveButtonReset;

    public bool revived = false;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = waitTime;
        timer = this.GetComponent<Image>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRevived(bool value)
    {
        revived = value;
    }

    public void StartTimer()
    {
        StartCoroutine(ReduceToZero());
    }

    IEnumerator ReduceToZero()
    {
        

        while (!(currentTime <= 0) && !revived)
        {
            currentTime -= Time.fixedDeltaTime;
            timer.fillAmount = currentTime / waitTime;
            Debug.Log("currentTime : " + currentTime);
            if (currentTime <= 0)
            {
                //
                Debug.Log("OnReviveTimeOver : " + currentTime);
                OnReviveTimeOver.Invoke();
            }
            yield return new WaitForFixedUpdate();
        }
        OnReviveButtonReset.Invoke();
        Debug.Log("OnReviveButtonReset : " + currentTime);
        currentTime = waitTime;
        timer.fillAmount = 1;
        revived = false;


    }
}
